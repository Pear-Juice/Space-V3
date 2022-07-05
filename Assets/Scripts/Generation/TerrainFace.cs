using UnityEngine;

namespace Generation
{
    public class TerrainFace
    {
        private Mesh mesh;
        private int resolutuon;
        private Vector3 localUp;
        private Vector3 axisA;
        private Vector3 axisB;
        private Shape shape;

        public TerrainFace(Mesh mesh, int resolutuon, Vector3 localUp, Shape shape)
        {
            this.mesh = mesh;
            this.resolutuon = resolutuon;
            this.localUp = localUp;
            this.shape = shape;

            axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            axisB = Vector3.Cross(localUp, axisA);
        }

        public void constructMesh()
        {
            Vector3[] vertices = new Vector3[resolutuon * resolutuon];
            int[] triangles = new int[(resolutuon - 1) * (resolutuon - 1) * 6];
            int triIndex = 0;

            for (int y = 0; y < resolutuon; y++)
            {
                for (int x = 0; x < resolutuon; x++)
                {
                    int i = x + y * resolutuon;
                    Vector2 percent = new Vector2(x, y) / (resolutuon - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                    Vector3 pointOnUnitSphere = Shape.sphereifyCube(pointOnUnitCube);

                        pointOnUnitSphere = shape.getPosition(pointOnUnitSphere);

                        vertices[i] = pointOnUnitSphere;

                    if (x != resolutuon - 1 && y != resolutuon - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolutuon + 1;
                        triangles[triIndex + 2] = i + resolutuon;
                    
                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolutuon + 1;

                        triIndex += 6;
                    }
                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}
