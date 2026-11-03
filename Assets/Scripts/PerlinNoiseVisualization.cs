
using UnityEngine;
using UnityEngine.UI;
using static EnemyStatManager;

public class PerlinNoiseVisualization : MonoBehaviour
{
    public StatType statType;
    private Image image;
    private PerlinNoise noise;
    private Texture2D noiseTexture;
    private Color[] pix;
    void Start()
    {
        image.enabled = true;
        Stat stat = statsByStatType[statType];
        noise = new PerlinNoise(stat.perlinNoise);
        noiseTexture = new Texture2D(noise.GetDimensions().x, noise.GetDimensions().y);
        pix = new Color[noise.GetDimensions().x * noise.GetDimensions().y];

        image = GetComponent<Image>();
        image.material.mainTexture = noiseTexture;

        CalcNoise();
    }

    void CalcNoise()
    {
        int y = 0;

        while (y < noiseTexture.height)
        {
            int x = 0;
            while (x < noiseTexture.width)
            {
                float sample = noise.GetNoiseValue(x, y);
                pix[y * noiseTexture.width + x] = new Color(sample, sample, sample, sample);
                x++;
            }
            y++;
        }

        noiseTexture.SetPixels(pix);
        noiseTexture.Apply();
    }
}
