using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatLoader
{
    static readonly string path = "Stats";

    static Dictionary<string, EnemyStatData> keyToStatData = new Dictionary<string, EnemyStatData>();

    private static bool initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        List<EnemyStatData> allEnemyStats = Resources.LoadAll<EnemyStatData>(path).ToList();
        foreach (EnemyStatData stat in allEnemyStats)
        {
            keyToStatData[stat.key] = stat;
        }
        initialized = true;
    }

    public static EnemyStatData GetEnemyStatData(string statKey)
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("Stats are not initialized yet!");
            return null;
        }

        if (keyToStatData.ContainsKey(statKey))
        {
            return keyToStatData[statKey];
        }
        else
        {
            Debug.LogWarning($"There is no stat with a key of {statKey}!");
            return null;
        }
    }

    public static List<EnemyStatData> GetAllEnemyStatData()
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("Stats are not initialized yet!");
            return null;
        }

        if (keyToStatData.Values.Count <= 0)
        {
            Debug.LogWarning("No Stats were found!");
            return null;
        }

        return keyToStatData.Values.ToList();
    }

    public static bool IsInitialized() { return initialized; } 
}
