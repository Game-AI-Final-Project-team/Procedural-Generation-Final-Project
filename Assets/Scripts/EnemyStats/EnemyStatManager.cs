using UnityEngine;

public static class EnemyStatManager
{
    public abstract class Stat
    {
        public Stat(StatType statType, float cost, Vector2 statRange, PerlinNoise noise)
        {
            costPerStat = cost;
            statValueRange = statRange;
            this.statType = statType;
            perlinNoise = noise;
        }
        public Stat(Stat copy)
        {
            costPerStat = copy.costPerStat;
            statValueRange = copy.statValueRange;
            statType = copy.statType;
            perlinNoise = new PerlinNoise(copy.perlinNoise);
        }
        public Vector2 statValueRange;
        public float costPerStat;
        public StatType statType;
        public PerlinNoise perlinNoise;
    }

    public class StatInt : Stat
    {
        public StatInt(StatType statType, float cost, Vector2 statRange, PerlinNoise noise) : base(statType, cost, statRange, noise) { }
        public StatInt(StatInt statInt) : base(statInt.statType, statInt.costPerStat, statInt.statValueRange, statInt.perlinNoise)
        {
            statValue = statInt.statValue;
        }
        public int statValue;
    }

    public class StatFloat : Stat
    {
        public StatFloat(StatType statType, float cost, Vector2 statRange, PerlinNoise noise) : base(statType, cost, statRange, noise) { }
        public StatFloat(StatFloat statFloat) : base(statFloat.statType, statFloat.costPerStat, statFloat.statValueRange, statFloat.perlinNoise)
        {
            statValue = statFloat.statValue;
        }
        public float statValue;
    }

    public enum StatType
    {
        Health,
        Damage,
        MaxSpeed,
        Acceleration,
        RotationSpeed,
        RotationAcceleration,
        Scale,
        LAST,
    }

    static readonly int NoiseWidth = 256;
    static readonly int NoiseHeight = 256;
    static readonly float NoiseScale = 20.0f;

    public static StatInt healthStat = new StatInt(StatType.Health, 0.1f, new Vector2Int(1, 1000), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.Health));
    public static StatInt damageStat = new StatInt(StatType.Damage, 0.1f, new Vector2Int(0, 1000), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.Damage));
    public static StatFloat speedStat = new StatFloat(StatType.MaxSpeed, 15, new Vector2(0, 0.5f), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.MaxSpeed));
    public static StatFloat accelerationStat = new StatFloat(StatType.Acceleration, 7, new Vector2(1.0f, 2.0f), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.Acceleration));
    public static StatFloat rotationStat = new StatFloat(StatType.RotationSpeed, 1, new Vector2(0, float.MaxValue), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.RotationSpeed));
    public static StatFloat rotationAccelerationStat = new StatFloat(StatType.RotationAcceleration, 1, new Vector2(0, float.MaxValue), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.RotationAcceleration));
    public static StatFloat scaleFactorStat = new StatFloat(StatType.Scale, 6, new Vector2(1.0f, 2.0f), new PerlinNoise(NoiseWidth, NoiseHeight, NoiseScale, (int)StatType.Scale));
}
