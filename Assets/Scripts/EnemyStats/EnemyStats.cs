using System;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStatManager;

public class EnemyStats
{
    private Dictionary<StatType, Stat> stats = new()
    {
        { StatType.Health, new StatInt(healthStat)},
        { StatType.Damage, new StatFloat(damageStat)},
        { StatType.MaxSpeed, new StatFloat(speedStat)},
        { StatType.Acceleration, new StatFloat(accelerationStat)},
        { StatType.RotationSpeed, new StatFloat(rotationStat)},
        { StatType.RotationAcceleration, new StatFloat(rotationAccelerationStat)},
        { StatType.Scale, new StatFloat(scaleFactorStat)},
    };

    private float _enemyPointsGiven = 0;
    private float _pointsUsed = 0;

    private readonly float Contrast = 2.0f;
    private readonly float MinPerStat = 1.0f;

    public void Initialize(float points, Vector2Int position)
{
    AllocatePoints(points, position);
}

public void AllocatePoints(float points, Vector2Int position)
{
    _enemyPointsGiven = points;
    _pointsUsed = 0f;

    int numStats = (int)StatType.LAST;
    float[] weights = new float[numStats];
    float sum = 0f;

    for (int i = 0; i < numStats; i++)
    {
        float weight = stats[(StatType)i].perlinNoise.GetNoiseValue(position);
        weight = Mathf.Pow(weight, Contrast);
        weights[i] = weight;
        sum += weight;
    }

    if (sum <= 0.0001f)
    {
        for (int i = 0; i < numStats; i++) weights[i] = 1f;
        sum = numStats;
    }

    float minPerStat = Mathf.Min(MinPerStat, points / numStats);
    float distributable = points - minPerStat * numStats;

    float[] budget = new float[numStats];
    for (int i = 0; i < numStats; i++)
        budget[i] = minPerStat + distributable * (weights[i] / sum);

    float leftover = 0f;
    float[] progressToNextUnit = new float[numStats];
    for (int i = 0; i < numStats; i++)
    {
        if (stats[(StatType)i] is StatInt statInt)
        {
            float exactUnits = budget[i] / statInt.costPerStat;
            int units = Mathf.FloorToInt(exactUnits);
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
            StatInt statInt = (StatInt)stats[(StatType)i];
            if (statInt.costPerStat > leftover + 0.0001f) continue;
            if (best == -1 || progressToNextUnit[i] > progressToNextUnit[best]) best = i;
        }
        if (best == -1) break;

        float cost = ((StatInt)stats[(StatType)best]).costPerStat;
        budget[best] += cost;
        leftover -= cost;
        progressToNextUnit[best] = -1f;
    }

    if (leftover > 0.0001f)
    {
        float floatWeightSum = 0f;
        int floatCount = 0;
        for (int i = 0; i < numStats; i++)
        {
            if (stats[(StatType)i] is StatFloat)
            {
                floatWeightSum += weights[i];
                floatCount++;
            }
        }

        if (floatCount > 0)
        {
            for (int i = 0; i < numStats; i++)
            {
                if (stats[(StatType)i] is not StatFloat) continue;
                float share = floatWeightSum > 0.0001f ? weights[i] / floatWeightSum : 1f / floatCount;
                budget[i] += leftover * share;
            }
            leftover = 0f;
        }
    }

    for (int i = 0; i < numStats; i++)
    {
        SetStat((StatType)i, budget[i]);
        _pointsUsed += budget[i];
    }

    Debug.Log($"{_enemyPointsGiven:F2} / {_pointsUsed:F2} (unspent: {leftover:F2})");
}

void SetStat(StatType statType, float value)
{
    Stat stat = stats[statType];
    switch (stat)
    {
        case StatInt statInt:
            statInt.statValue = Mathf.RoundToInt(value / statInt.costPerStat);
            break;
        case StatFloat statFloat:
            statFloat.statValue = value / statFloat.costPerStat;
            break;
    }
}
}
