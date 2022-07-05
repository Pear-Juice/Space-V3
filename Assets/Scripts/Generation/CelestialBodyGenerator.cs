using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Generation
{
    public class CelestialBodyGenerator : MonoBehaviour
    {
        public bool autoUpdate = false;
        [Range(2,255)]
        public int resolution = 10;
        
        [Header("Colors")]
        public LandColorSettings landColorSettings;
        public AtmosphereColorSettings atmosphereColorSettings;

        [Header("Shaders")]
        public GameObject postProcessingBox;
        public Shader landShader;
        private Material landMaterial;
        public Shader postProcessingShader;
        private Material postProcessingMaterial;

        [Header("Shape")]
        public float radius;
        public Shape shape;
        
        [SerializeField, HideInInspector]
        private MeshFilter[] meshFilters;
        public MeshRenderer[] meshRenderers;
        private TerrainFace[] terrainFaces;

        public void updateLOD(int res)
        {
            resolution = res;
        
            generateMesh();
        }

        private float oldResolution;
        private float oldRadius;
        public void OnValidate()
        {
            if (!autoUpdate)
                return;

            if (shape && (oldResolution != resolution || oldRadius != radius))
            {
                shape.radius = radius;

                oldRadius = radius;
                oldResolution = resolution;
                
                initializeSurface();
                generateMesh();
            }
            
            initializePostProcessing();
            setPostProcessingMaterialProperties();
            setLandMaterialProperties();
        }

        public void setLandMaterialProperties()
        {
            if (!landShader)
                return;
            
            #region SetMaterialColors
            
            landMaterial = new Material(landShader);
            landMaterial.SetColor("topColor", landColorSettings.topColor);
            landMaterial.SetFloat("topLevel", landColorSettings.topLevel);
            landMaterial.SetColor("middleColor", landColorSettings.middleColor);
            landMaterial.SetFloat("middleLevel", landColorSettings.middleLevel);
            landMaterial.SetColor("bottomColor", landColorSettings.bottomColor);
            landMaterial.SetFloat("grassSpeckleLevel", landColorSettings.speckleLevel);
            landMaterial.SetFloat("grassSpeckleSize", landColorSettings.speckleSize);
            landMaterial.SetFloat("warpLevel1", landColorSettings.warpLevel1);
            landMaterial.SetFloat("warpSize1", landColorSettings.warpSize1);
            landMaterial.SetFloat("warpLevel2", landColorSettings.warpLevel2);
            landMaterial.SetFloat("warpSize2", landColorSettings.warpSize2);
            landMaterial.SetFloat("planetRadius", shape.radius);
            
            landMaterial.SetColor("atmosphereLightColor", atmosphereColorSettings.lightColor);
            landMaterial.SetColor("atmosphereDarkColor", atmosphereColorSettings.darkColor);
            landMaterial.SetFloat("atmosphereRadius", atmosphereColorSettings.radius);
            landMaterial.SetFloat("atmosphereThickness", atmosphereColorSettings.thickness);

            #endregion

            for (int i = 0; i < 6; i++)
            {
                meshFilters[i].transform.GetComponent<MeshRenderer>().material = landMaterial;
            }
        }

        public void setPostProcessingMaterialProperties()
        {
            #region SetMaterialColors
            
            postProcessingMaterial.SetColor("atmosphereLightColor", atmosphereColorSettings.lightColor);
            postProcessingMaterial.SetColor("atmosphereDarkColor", atmosphereColorSettings.darkColor);
            postProcessingMaterial.SetFloat("atmosphereRadius", atmosphereColorSettings.radius);
            postProcessingMaterial.SetFloat("atmosphereThickness", atmosphereColorSettings.thickness);

            #endregion
        }

        public void initializePostProcessing()
        {
            if (postProcessingBox == null)
            {
                var prefab = Resources.Load("Post Processing Box");
                postProcessingBox = Instantiate((GameObject)prefab, transform);
                postProcessingBox.name = "Post Processing Box";
            }

            postProcessingBox.transform.localScale = Vector3.one * (radius + atmosphereColorSettings.radius) * 60;

            postProcessingMaterial = new Material(postProcessingShader);
            postProcessingBox.transform.GetComponent<MeshRenderer>().material = postProcessingMaterial;
        }
        
        public void initializeSurface()
        {
            if (meshFilters == null || meshFilters.Length == 0)
                meshFilters = new MeshFilter[6];
            terrainFaces = new TerrainFace[6];
            if (meshRenderers == null || meshRenderers.Length == 0)
                meshRenderers = new MeshRenderer[6];

            Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};
        
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject($"Mesh {i}");
                    meshObj.transform.SetParent(transform);
                    meshObj.transform.localPosition = new Vector3();
                    meshObj.transform.localScale = Vector3.one;
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].sharedMesh = new Mesh();
                    meshRenderers[i] = meshObj.AddComponent<MeshRenderer>();
                    
                    meshObj.AddComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;
                }

                terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i], shape);
            }
        }

        public void generateMesh()
        {
            foreach (TerrainFace terrainFace in terrainFaces)
            {
                terrainFace.constructMesh();
            }
        }
        
        [Serializable]
        public class LandColorSettings
        {
            public Color topColor;
            public float topLevel;
            public Color middleColor;
            public float middleLevel;
            public Color bottomColor;

            public float speckleLevel;
            public float speckleSize;
            public float warpLevel1;
            public float warpSize1;
            public float warpLevel2;
            public float warpSize2;

        }

        [Serializable]
        public class AtmosphereColorSettings
        {
            public Color lightColor;
            public Color darkColor;
            public float radius;
            public float thickness;
        }

        [Serializable]
        public class OceanColorSettings
        {
            public Color lightColor;
            public Color darkColor;
        }
    }
}
