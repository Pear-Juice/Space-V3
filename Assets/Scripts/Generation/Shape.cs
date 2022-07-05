using System;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

namespace Generation
{
    public class Shape : MonoBehaviour
    {
        public CelestialBodyGenerator celestialBodyGenerator;
        [HideInInspector]
        public float radius;
        public List<Layer> layers;

        private void OnValidate()
        {
            if (!celestialBodyGenerator || !celestialBodyGenerator.autoUpdate)
                return;
            
            celestialBodyGenerator.initializeSurface();
            celestialBodyGenerator.generateMesh();
        }

        public Vector3 getPosition(Vector3 pos)
        {
            float height = radius;
            float prevHeight = 1;
            
            foreach (Layer layer in layers)
            {
                if (!layer.enabled)
                    continue;

                float newHeight = layer.getHeight(pos, radius);
                if (layer.elevation >= 0)
                {
                    if (newHeight > prevHeight)
                    {
                        if (layer.exponent != -1)
                            height += Mathf.Pow(layer.exponent, newHeight - prevHeight);
                        else
                            height += newHeight - prevHeight;

                        prevHeight = newHeight;
                    }
                }
            }

            Vector3 newPos = pos * height;
            return newPos;
        }

        [Serializable]
        public class Layer
        {
            public enum Type
            {
                Normal,
                Ridge,
                Warped,
            }
            private float height;
        
            public Type type;
            public bool enabled;
            public float elevation = 1;
            public float offset;
            public float roughness;
            public float minPrevHeight;
            public float exponent;
        
            public float getHeight(Vector3 pos, float radius)
            {
                switch (type)
                {
                    case Type.Normal:
                    {
                        height = (sampleNoise(pos * roughness + Vector3.one * offset) + 1) * .5f * elevation;
                        break;
                    }
                    case Type.Ridge:
                    {
                        height = (1 - Math.Abs(sampleNoise(pos * roughness + Vector3.one * offset)) + 1) * .5f * elevation;
                        break;
                    }
                    case Type.Warped:
                    {
                        break;
                    }
                }

                return height;
            }
        }

        static float sampleNoise(Vector3 pos)
        {
            float noiseSum = 0;
            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < 5; i++)
            {
                noiseSum += Perlin.Noise(pos * frequency) * amplitude;
                
                frequency *= 2;
                amplitude *= .5f;
            }

            return noiseSum;
        }

        public static Vector3 sphereifyCube(Vector3 v)
        {
            float x2 = v.x * v.x;
            float y2 = v.y * v.y;
            float z2 = v.z * v.z;
            
            Vector3 s;
            s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
            s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
            s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

            return s;
        }
    }
}
