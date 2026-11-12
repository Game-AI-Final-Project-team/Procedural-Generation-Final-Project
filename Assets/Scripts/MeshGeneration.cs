using Unity.Mathematics;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    [SerializeField] Vector2Int _dimensions;
    [SerializeField] EnemyStatManager.StatType _statType;
    private Vector2 _offset;
    Color32 NoiseColor(float value)
    {
        value *= 255;
        value /= 1.75f;
        return new Color32((byte)value, (byte)value, (byte)value, (byte)value);
    }

    Color32 TerrainNoiseColor(float value)
    {
        float hue = 1f - (value * 1f);
        Color32 color = Color.HSVToRGB(hue, 0.7f, 0.5f);
        return color;
    }

    [ContextMenu("Generate Debug Mesh")]
    public void DebugGenerateHealthMesh()
    {
        GenerateMesh(EnemyStatManager.statsByStatType[_statType].perlinNoise);
    }
    
    public void GenerateMesh(PerlinNoise noise)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Noise Mesh";

        _offset = _dimensions / 2;
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
                int xPosition = x - (int)_offset.x;
                int yPosition = y - (int)_offset.y;
                float noiseVal = noise.GetNoiseValue(xPosition, yPosition);
                vertices[i] = new Vector3(xPosition, yPosition, noiseVal * 3);
                colors[i] = TerrainNoiseColor(noiseVal);
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
