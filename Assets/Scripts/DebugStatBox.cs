using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static EnemyStatManager;

public class DebugStatBox : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] TextMeshProUGUI statsText;

    void Update()
    {
        int x = (int)playerTransform.position.x;
        int y = (int)playerTransform.position.y;

        StringBuilder str = new StringBuilder();
        str.Append($"Coordinates: ({x}, {y})\n");
        str.Append("Stat Weights At Player Location:\n");

        for (int i = 0; i < (int)StatType.LAST; i++)
        {
            str.Append($"{statsToReadableString[(StatType)i]}: {statsByStatType[(StatType)i].perlinNoise.GetNoiseValue(x, y).ToString("F2")}\n");
        }
        statsText.text = str.ToString();
    }
}
