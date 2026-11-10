
using UnityEngine;
using UnityEngine.UI;
using static EnemyStatManager;

public class PerlinNoiseVisualization : MonoBehaviour
{
    [SerializeField] StatType statType;
    private Image image;
    private PerlinNoise noise;
    private Texture2D noiseTexture;
    private Color[] pix;
    void Start()
    {
        Stat stat = statsByStatType[statType];
        Debug.Log($"{gameObject.name} test 1 {stat.perlinNoise.GetNoiseValue(0, 0)}");
        noise = new PerlinNoise(stat.perlinNoise);
        Debug.Log($"{gameObject.name} test 2 {noise.GetNoiseValue(0, 0)}");
        noiseTexture = new Texture2D(noise.GetDimensions().x, noise.GetDimensions().y);
        pix = new Color[noise.GetDimensions().x * noise.GetDimensions().y];

        image = GetComponent<Image>();
        image.enabled = true;

        CalcNoise();

        Rect rect = new Rect(0, 0, noiseTexture.width, noiseTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        image.sprite = Sprite.Create(noiseTexture, rect, pivot);
    }

    [ContextMenu("Recalc Noise")]
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
