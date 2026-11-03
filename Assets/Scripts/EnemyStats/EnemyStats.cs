using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static EnemyStatManager;

public class EnemyStats
{
    private Dictionary<StatType, Stat> stats = new()
    {
        { StatType.Health, new StatInt(healthStat)},
        { StatType.Damage, new StatInt(damageStat)},
        { StatType.MaxSpeed, new StatFloat(speedStat)},
        { StatType.Acceleration, new StatFloat(accelerationStat)},
        { StatType.RotationSpeed, new StatFloat(rotationStat)},
        { StatType.RotationAcceleration, new StatFloat(rotationAccelerationStat)},
        { StatType.Scale, new StatFloat(scaleFactorStat)},
    };

    private float _enemyPointsGiven = 0;
    private float _pointsUsed = 0;

    private readonly float Contrast = 2.0f;
    private const float Epsilon = 0.0001f;

    public void Initialize(float points, Vector2Int position)
    {
        AllocatePoints(points, position);
    }

    public void AllocatePoints(float points, Vector2Int position)
    {
        _enemyPointsGiven = points;
        _pointsUsed = 0f;

        int numStats = (int)StatType.LAST;
        float[] weights = new float[numStats];  // How much each stat is valued at (not the cost to buy it)
        float[] capacity = new float[numStats]; // The maximum points possible for a stat
        float[] budget = new float[numStats];   // How much can be spent on a stat
        bool[] allStats = new bool[numStats];   // List of every stat
        bool[] floatStats = new bool[numStats]; // List of the float stats

        for (int i = 0; i < numStats; i++)
        {
            Stat stat = stats[(StatType)i];
            weights[i] = Mathf.Pow(stat.perlinNoise.GetNoiseValue(position), Contrast);
            capacity[i] = MaxPoints(stat);
            allStats[i] = true;
            floatStats[i] = stat is StatFloat;
        }

        float leftover = Distribute(points, budget, capacity, weights, allStats);

        float[] progressToNextUnit = new float[numStats];
        for (int i = 0; i < numStats; i++)
        {
            if (stats[(StatType)i] is StatInt statInt)
            {
                float exactUnits = budget[i] / statInt.costPerStat;
                int units = Mathf.FloorToInt(exactUnits + Epsilon);
                float spent = units * statInt.costPerStat;

                leftover += budget[i] - spent;
                progressToNextUnit[i] = exactUnits - units;
                budget[i] = spent;
            }
            else
            {
                progressToNextUnit[i] = -1f;
            }
        }

        while (true)
        {
            int best = -1;
            for (int i = 0; i < numStats; i++)
            {
                if (progressToNextUnit[i] < 0f) continue;

                float cost = ((StatInt)stats[(StatType)i]).costPerStat;

                if (cost > leftover + Epsilon) continue;
                if (budget[i] + cost > capacity[i] + Epsilon) continue;

                if (best == -1 || progressToNextUnit[i] > progressToNextUnit[best]) best = i;
            }
            if (best == -1) break;

            float bestCost = ((StatInt)stats[(StatType)best]).costPerStat;
            budget[best] += bestCost;
            leftover -= bestCost;
            progressToNextUnit[best] = -1f;
        }

        Distribute(leftover, budget, capacity, weights, floatStats);

        for (int i = 0; i < numStats; i++)
        {
            SetStat((StatType)i, budget[i]);
            _pointsUsed += budget[i];
        }
    }

    float Distribute(float amount, float[] budget, float[] capacity, float[] weights, bool[] eligible)
    {
        int n = budget.Length;
        bool[] open = new bool[n];
        for (int i = 0; i < n; i++)
            open[i] = eligible[i] && budget[i] < capacity[i] - Epsilon;

        while (amount > Epsilon)
        {
            float weightSum = 0f;
            int openCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (!open[i]) continue;
                weightSum += weights[i];
                openCount++;
            }
            if (openCount == 0) break;

            bool equalSplit = weightSum <= Epsilon;
            float roundAmount = amount;
            bool cappedAny = false;

            for (int i = 0; i < n; i++)
            {
                if (!open[i]) continue;
                float share = roundAmount * (equalSplit ? 1f / openCount : weights[i] / weightSum);
                float room = capacity[i] - budget[i];
                if (share >= room)
                {
                    budget[i] = capacity[i];
                    amount -= room;
                    open[i] = false;
                    cappedAny = true;
                }
            }

            if (!cappedAny)
            {
                for (int i = 0; i < n; i++)
                {
                    if (!open[i]) continue;
                    budget[i] += roundAmount * (equalSplit ? 1f / openCount : weights[i] / weightSum);
                }
                amount = 0f;
            }
        }

        return Mathf.Max(0f, amount);
    }

    float MaxPoints(Stat stat)
    {
        float range = Mathf.Max(0f, stat.statValueRange.y - stat.statValueRange.x);
        switch (stat)
        {
            case StatInt statInt:
                return Mathf.FloorToInt(range + Epsilon) * statInt.costPerStat;
            case StatFloat statFloat:
                return range * statFloat.costPerStat;
        }
        return 0f;
    }

    void SetStat(StatType statType, float value)
    {
        Stat stat = stats[statType];
        Vector2 range = stat.statValueRange;
        switch (stat)
        {
            case StatInt statInt:
                int units = Mathf.RoundToInt(value / statInt.costPerStat);
                statInt.statValue = Mathf.Clamp(Mathf.RoundToInt(range.x) + units, Mathf.RoundToInt(range.x), Mathf.FloorToInt(range.y));
                break;
            case StatFloat statFloat:
                statFloat.statValue = Mathf.Clamp(range.x + value / statFloat.costPerStat, range.x, range.y);
                break;
        }
    }

    public Stat GetStat(StatType statType)
    {
        return stats[statType];
    }

    public void PrintStats()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Total Points: {_enemyPointsGiven}\n");
        for (int i = 0; i < (int)StatType.LAST; i++)
        {
            switch (stats[(StatType)i])
            {
                case StatInt statInt:
                    str.Append($"{statInt.statType}: {statInt.statValue}\n");
                    break;
                case StatFloat statFloat:
                    str.Append($"{statFloat.statType}: {statFloat.statValue}\n");
                    break;
            }
        }
        Debug.Log(str);
    }
}
