using UnityEngine;

public class PerlinNoise 
{
    private int seed;
    private int pixWidth;
    private int pixHeight;
    private int OffsetX => 0;//pixWidth / 2;
    private int OffsetY => 0;//pixHeight / 2;
    private float xOffset;
    private float yOffset;
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

        xOffset = (float)(rng.NextDouble() * 1000.0);
        yOffset = (float)(rng.NextDouble() * 1000.0);

        for (int y = 0; y < pixHeight; y++)
        {
            for (int x = 0; x < pixWidth; x++)
            {
                float xCoord = xOffset + (float)x / pixWidth * scale;
                float yCoord = yOffset + (float)y / pixHeight * scale;
                
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) +
                                    0.5f * Mathf.PerlinNoise(2 * xCoord, 2 * yCoord) +
                                    0.25f * Mathf.PerlinNoise(4 * xCoord, 4 * yCoord);
                noiseGrid[x, y] = noiseValue / 1.75f;
                
            }
        }
    }

    public float GetNoiseValue(int x, int y)
    {
        x = ((x % pixWidth) + pixWidth + OffsetX) % pixWidth;
        y = ((y % pixHeight) + pixHeight + OffsetY) % pixHeight;
        return noiseGrid[x, y];
    }

    public Vector2Int GetDimensions()
    {
        return new Vector2Int(pixWidth, pixHeight);
    }

    public Vector2 GetOffset()
    {
        return new Vector2(xOffset, yOffset);
    }

    public float GetScale()
    {
        return scale;
    }

    public float GetNoiseValue(Vector2Int position) { return GetNoiseValue(position.x, position.y); }
}
