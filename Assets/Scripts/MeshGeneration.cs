using Unity.Mathematics;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    [SerializeField] Vector2Int _dimensions;

    Color32 NoiseColor(int x, int y, float frequency)
    {
        var value = 
                Mathf.PerlinNoise(1 * x / frequency, y / frequency) +
                0.5f * Mathf.PerlinNoise(2 * x / frequency, y / frequency) +
                0.25f * Mathf.PerlinNoise(4 * x / frequency, y / frequency);

        value *= 255;
        value /= 1.75f;
        return new Color32((byte)value, (byte)value, (byte)value, (byte)value);
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[_dimensions.x * _dimensions.y];
        int[] triangles = new int[_dimensions.x * _dimensions.y];

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        gameObject.AddComponent<MeshFilter>().mesh = mesh;

        //Color32[] colors = new Color32[_dimensions.x, _dimensions.y];
    }
}
