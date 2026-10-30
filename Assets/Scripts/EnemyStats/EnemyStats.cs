using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public class Stat
    {
        public Stat(StatType statType, float cost, Vector2 statRange)
        {
            costPerStat = cost;
            statValueRange = statRange;
            this.statType = statType;
        }
        public int statValue;
        public Vector2 statValueRange;
        public float costPerStat;
        public StatType statType;
    }

    public class StatFloat : Stat
    {
        public StatFloat(StatType statType, float cost, Vector2 statRange) : base(statType, cost, statRange) { }
        public new float statValue;
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

    private Stat _healthStat = new Stat(StatType.Health, EnemyStatManager.HealthCostPerStat, EnemyStatManager.HealthStatRange);
    private StatFloat _damageStat = new StatFloat(StatType.Damage, EnemyStatManager.DamageCostPerStat, EnemyStatManager.DamageRange);
    private StatFloat _speedStat = new StatFloat(StatType.MaxSpeed, EnemyStatManager.MaxSpeedCostPerStat, EnemyStatManager.MaxSpeedRange);
    private StatFloat _accelerationStat = new StatFloat(StatType.Acceleration, EnemyStatManager.AccelerationCostPerStat, EnemyStatManager.AccelerationRange);
    private StatFloat _rotationStat = new StatFloat(StatType.RotationSpeed, EnemyStatManager.RotationalCostPerStat, EnemyStatManager.RotationalRange);
    private StatFloat _rotationAccelerationStat = new StatFloat(StatType.RotationAcceleration, EnemyStatManager.RotationalAccelerationCostPerStat, EnemyStatManager.RotationalAccelerationRange);
    private StatFloat _scaleFactorStat = new StatFloat(StatType.Scale, EnemyStatManager.ScaleFactorCostPerStat, EnemyStatManager.ScaleFactorRange);

    public int Health => _healthStat.statValue;
    public float Damage => _damageStat.statValue;
    public float MaxSpeed => _speedStat.statValue;
    public float Acceleration => _accelerationStat.statValue;
    public float MaxRotationalSpeed => _rotationStat.statValue;
    public float MaxRotationalAcceleration => _rotationAccelerationStat.statValue;
    public float ScaleFactor => _scaleFactorStat.statValue;

    private float _enemyPointsGiven = 0;
    private float _pointsUsed = 0;

    public void Initialize(float points)
    {
        _enemyPointsGiven = points;
        AllocatePoints(points);
    }

    public void AllocatePoints(float points)
    {
        
    }
}
