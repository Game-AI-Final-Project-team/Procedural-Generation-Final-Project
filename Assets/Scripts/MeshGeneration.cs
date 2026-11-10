using Unity.Mathematics;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    private readonly float xSpacing = 1.0f;
    private Vector2Int _dimensions;
    Color32 NoiseColor(float value)
    {
        value *= 255;
        value /= 1.75f;
        return new Color32((byte)value, (byte)value, (byte)value, (byte)value);
    }

    [ContextMenu("Generate Debug Mesh")]
    public void DebugGenerateHealthMesh()
    {
        GenerateMesh(EnemyStatManager.healthStat.perlinNoise);
    }
    public void GenerateMesh(PerlinNoise noise)
    {
        Mesh mesh = new Mesh();
        mesh.name = "noiseMesh";

        _dimensions = noise.GetDimensions();
        int width = _dimensions.x;
        int height = _dimensions.y;
        int rows = _dimensions.x + 1;
        int columns = _dimensions.y + 1;
        Vector3[] vertices = new Vector3[rows * columns];
        Color32[] colors = new Color32[vertices.Length];
        int[] triangles = new int[width * height * 6];

        int i = 0;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                float noiseVal = noise.GetNoiseValue(x, y);
                vertices[i] = new Vector3(x, y, noiseVal * 3);
                colors[i] = NoiseColor(noiseVal);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++; 
        }

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors32 = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
