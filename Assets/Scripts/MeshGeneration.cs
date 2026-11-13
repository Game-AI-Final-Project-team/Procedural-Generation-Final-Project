using Unity.Mathematics;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    [SerializeField] Vector2Int _dimensions;
    [SerializeField] bool terrain;
    [SerializeField] EnemyStatManager.StatType _statType;
    private Vector2 _offset;
    Color32 NoiseColor(float value)
    {
        value *= 255;
        value /= 1.75f;
        return new Color32((byte)value, (byte)value, (byte)value, (byte)value);
    }

    static readonly (float height, Color32 color)[] terrainBands =
    {
        (0.00f, new Color32( 15,  40, 110, 255)), // deep water
        (0.30f, new Color32( 30,  90, 170, 255)), // shallow water
        (0.40f, new Color32( 70, 150, 200, 255)), // shoreline water
        (0.42f, new Color32(210, 195, 140, 255)), // sand
        (0.48f, new Color32( 90, 150,  60, 255)), // grass
        (0.60f, new Color32( 40, 100,  40, 255)), // forest
        (0.72f, new Color32(110,  95,  80, 255)), // rock / dirt
        (0.85f, new Color32(140, 135, 130, 255)), // high rock
        (0.92f, new Color32(245, 245, 250, 255)), // snow
    };

    Color32 HeatNoiseColor(float value)
    {
        float hue = 1f - (value * 1f);
        Color32 color = Color.HSVToRGB(hue, 0.7f, 0.5f);
        return color;
    }

    Color32 TerrainNoiseColor(float value)
    {
        value = Mathf.Clamp01(value);

        for (int i = 0; i < terrainBands.Length - 1; i++)
        {
            var a = terrainBands[i];
            var b = terrainBands[i + 1];
            if (value <= b.height)
            {
                float t = Mathf.InverseLerp(a.height, b.height, value);
                return Color32.Lerp(a.color, b.color, t);
            }
        }
        return terrainBands[terrainBands.Length - 1].color;
    }

    public void Awake()
    {
        if (terrain) GenerateMesh(EnemyStatManager.terrain);
        else GenerateMesh(EnemyStatManager.statsByStatType[_statType].perlinNoise);
    }
    
    public void GenerateMesh(PerlinNoise noise)
    {
        Mesh mesh = new Mesh();
        mesh.name = $"Noise Mesh";

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
                colors[i] = terrain ? TerrainNoiseColor(noiseVal) : HeatNoiseColor(noiseVal);
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
