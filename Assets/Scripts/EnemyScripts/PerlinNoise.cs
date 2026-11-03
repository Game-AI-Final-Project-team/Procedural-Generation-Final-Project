using UnityEngine;

public class PerlinNoise 
{
    private int seed;
    private int pixWidth = 256;
    private int pixHeight = 256;
    private float scale = 1.0f;

    private float[,] noiseGrid;

    public PerlinNoise(int width, int height, float scale, int seed)
    {
        pixWidth = width;
        pixHeight = height;
        this.scale = scale;
        this.seed = seed;

        noiseGrid = new float[pixWidth, pixHeight];
        GenerateNoiseData();
    }

    public PerlinNoise(PerlinNoise copy)
    {
        pixWidth = copy.pixWidth;
        pixHeight = copy.pixHeight;
        scale = copy.scale;
        seed = copy.seed;

        noiseGrid = (float[,])copy.noiseGrid.Clone();
    }

    void GenerateNoiseData()
    {
        System.Random rng = new System.Random(seed);

        float xOrg = (float)(rng.NextDouble() * 1000.0);
        float yOrg = (float)(rng.NextDouble() * 1000.0);

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
        x = ((x % pixWidth) + pixWidth) % pixWidth;
        y = ((y % pixHeight) + pixHeight) % pixHeight;
        return noiseGrid[x, y];
    }

    public float GetNoiseValue(Vector2Int position) { return GetNoiseValue(position.x, position.y); }
}
