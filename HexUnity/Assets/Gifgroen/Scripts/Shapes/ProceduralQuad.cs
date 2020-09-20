using UnityEngine;

namespace Gifgroen.Shapes
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralQuad : MonoBehaviour
    {
        private const string ShapeName = nameof(ProceduralQuad);

#pragma warning disable 649
        [SerializeField] private int xSize = 1;

        [SerializeField] private int ySize = 1;

        [SerializeField] private Vector3[] vertices;

        private Mesh _mesh;
#pragma warning restore 649
        
        private void Awake()
        {
            _mesh = new Mesh {name = ShapeName};
            GetComponent<MeshFilter>().mesh = _mesh;

            _mesh.vertices = vertices = GenerateVertices(xSize, ySize);
            _mesh.triangles = GenerateTriangleIndexes(xSize, ySize);
            _mesh.uv = GenerateUvs(xSize, ySize);
            _mesh.tangents = GenerateTangents(xSize, ySize);

            _mesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            if (vertices == null || vertices.Length == 0)
            {
                return;
            }

            Gizmos.color = Color.black;
            foreach (var vertex in vertices)
            {
                Gizmos.DrawSphere(transform.position + vertex, 0.1f);
            }
        }

        private static Vector3[] GenerateVertices(int newX, int newY)
        {
            var X = (newX + 1);
            Vector3[] newVertices = new Vector3[X * (newY + 1)];
            for (int y = 0; y <= newY; y++)
            {
                int yOffset = y * X;
                for (int x = 0; x <= newX; x++)
                {
                    int index = x + yOffset;
                    newVertices[index] = new Vector3(x, y);
                }
            }

            return newVertices;
        }

        private static int[] GenerateTriangleIndexes(int newX, int newY)
        {
            int[] tris = new int[(newX + 1) * (newY + 1) * 6];

            for (int y = 0, ti = 0; y < newY; y++)
            {
                var yOffset = y * (newX + 1);
                for (int x = 0; x < newX; x++, ti += 6)
                {
                    var xOffset = yOffset + x;
                    tris[ti] = xOffset;
                    tris[ti + 1] = tris[ti + 4] = xOffset + newX + 1;
                    tris[ti + 2] = tris[ti + 3] = xOffset + 1;
                    tris[ti + 5] = xOffset + newX + 2;
                }
            }

            return tris;
        }

        private Vector2[] GenerateUvs(int newX, int newY)
        {
            Vector2[] uvs = new Vector2[(newX + 1) * (newY + 1)];
            for (int y = 0; y <= newY; y++)
            {
                for (int x = 0; x <= newX; x++)
                {
                    int index = x + y * (newX + 1);
                    uvs[index] = new Vector2((float) x / newX, (float) y / newY);
                }
            }

            return uvs;
        }

        private Vector4[] GenerateTangents(int newX, int newY)
        {
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            Vector4[] tangents = new Vector4[(newX + 1) * (newY + 1)];
            for (int y = 0; y <= newY; y++)
            {
                for (int x = 0; x <= newX; x++)
                {
                    int index = x + y * (newX + 1);
                    tangents[index] = tangent;
                }
            }

            return tangents;
        }
    }
}