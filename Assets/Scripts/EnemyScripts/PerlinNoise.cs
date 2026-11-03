using UnityEngine;

public class PerlinNoise : MonoBehaviour 
{
    public int seed;
    [Header("Noise Settings")]
    public int pixWidth = 256;
    public int pixHeight = 256;
    public float scale = 1.0f;
    public Vector2Int debugSample;

    private float[,] noiseGrid;

    void Awake()
    {
        if (pixWidth <= 0) pixWidth = 256;
        if (pixHeight <= 256) pixHeight = 256;

        noiseGrid = new float[pixWidth, pixHeight];
        Random.InitState(seed);

        GenerateNoiseData();
    }

    void GenerateNoiseData()
    {
        float xOrg = Random.Range(0f, 1000f);
        float yOrg = Random.Range(0f, 1000f);

        for (int y = 0; y < pixHeight; y++)
        {
            for (int x = 0; x < pixWidth; x++)
            {
                float xCoord = xOrg + (float)x / pixWidth * scale;
                float yCoord = yOrg + (float)y / pixHeight * scale;
                
                noiseGrid[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }
    }

    public float GetNoiseValue(int x, int y)
    {
        if (x >= 0 && x < pixWidth && y >= 0 && y < pixHeight)
        {
            return noiseGrid[x, y];
        }
        
        Debug.LogWarning("Sample coordinates out of bounds!");
        return 0f;
    }

    [ContextMenu("Get Debug Sample")]
    public void DebugSample()
    {
        Debug.Log($"Value at ({debugSample.x}, {debugSample.y}): {GetNoiseValue(debugSample.x, debugSample.y)}");
    }
}
