using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class EnemyStatManager
{
    public abstract class Stat
    {
        public Stat(string statName, string statKey, float cost, Vector2 statRange, PerlinNoise noise)
        {
            costPerStat = cost;
            statValueRange = statRange;
            this.statName = statName;
            this.statKey = statKey;
            perlinNoise = noise;
        }
        public Stat(Stat copy)
        {
            costPerStat = copy.costPerStat;
            statValueRange = copy.statValueRange;
            statName = copy.statName;
            perlinNoise = new PerlinNoise(copy.perlinNoise);
        }
        public Vector2 statValueRange;
        public float costPerStat;
        public string statName;
        public string statKey;
        public PerlinNoise perlinNoise;
    }

    public class StatInt : Stat
    {
        public StatInt(string statName, string statKey, float cost, Vector2 statRange, PerlinNoise noise) : base(statName, statKey, cost, statRange, noise) { }
        public StatInt(StatInt statInt) : base(statInt.statName, statInt.statKey, statInt.costPerStat, statInt.statValueRange, statInt.perlinNoise)
        {
            statValue = statInt.statValue;
        }
        public int statValue;
    }

    public class StatFloat : Stat
    {
        public StatFloat(string statName, string statKey, float cost, Vector2 statRange, PerlinNoise noise) : base(statName, statKey, cost, statRange, noise) { }
        public StatFloat(StatFloat statFloat) : base(statFloat.statName, statFloat.statKey, statFloat.costPerStat, statFloat.statValueRange, statFloat.perlinNoise)
        {
            statValue = statFloat.statValue;
        }
        public float statValue;
    }

    public static bool DebugMode = true;
    public static int NoiseWidth = 1024;
    static readonly int NoiseHeight = 1024;
    static readonly float NoiseScale = 10.0f;
    static readonly int GlobalSeedOffset = 20;

    public static PerlinNoise terrain = new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, GlobalSeedOffset - 1);

    private static bool initialized = false;
    private static Dictionary<string, Stat> keyToStat = new Dictionary<string, Stat>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Initialize()
    {
        List<EnemyStatData> enemyStatDatas = StatLoader.GetAllEnemyStatData();

        int i = 0;
        foreach (EnemyStatData statData in enemyStatDatas)
        {
            if (statData.clampToInteger)
            {
                StatInt statInt = new StatInt(statData.statName, statData.key, statData.costPerUnitStat, new Vector2Int((int)statData.minimumValue, (int)statData.maximumValue), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, i + GlobalSeedOffset));
                keyToStat[statData.key] = statInt;
            }
            else
            {
                StatFloat statFloat = new StatFloat(statData.statName, statData.key, statData.costPerUnitStat, new Vector2(statData.minimumValue, statData.maximumValue), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, i + GlobalSeedOffset));
                keyToStat[statData.key] = statFloat;
            }
            i++;
        }

        foreach(Stat stat in keyToStat.Values)
        {
            Debug.Log($"Stat Name: {stat.statName} Stat cost per stat: {stat.costPerStat}");
        }

        initialized = true;
    }

    public static Stat GetStatData(string statKey)
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("Stats are not initialized yet!");
            return null;
        }

        if (keyToStat.ContainsKey(statKey))
        {
            return keyToStat[statKey];
        }
        else
        {
            Debug.LogWarning($"There is no stat with a key of {statKey}!");
            return null;
        }
    }

    public static Stat GetStatData(int index)
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("Stats are not initialized yet!");
            return null;
        }

        if (StatCount >= index && index >= 0)
        {
            return keyToStat.Values.ToList()[index];
        }
        else
        {
            Debug.LogWarning($"Index out of bounds!");
            return null;
        }
    }

    public static List<Stat> GetAllStatData()
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("Stats are not initialized yet!");
            return null;
        }

        if (keyToStat.Values.Count <= 0)
        {
            Debug.LogWarning("No Stats were found!");
            return null;
        }

        return keyToStat.Values.ToList();
    }

    public static int StatCount => keyToStat.Values.Count;

    public static bool IsInitialized() { return initialized; } 
}
