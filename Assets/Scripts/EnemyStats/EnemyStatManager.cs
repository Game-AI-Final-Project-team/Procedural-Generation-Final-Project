using UnityEngine;

public static class EnemyStatManager
{
    public static readonly float HealthCostPerStat = 1;
    public static readonly Vector2Int HealthStatRange = new Vector2Int(1, int.MaxValue);
    public static readonly float DamageCostPerStat = 1;
    public static readonly Vector2 DamageRange = new Vector2(0, float.MaxValue);
    public static readonly float MaxSpeedCostPerStat = 1;
    public static readonly Vector2 MaxSpeedRange = new Vector2(0, float.MaxValue);
    public static readonly float AccelerationCostPerStat = 1;
    public static readonly Vector2 AccelerationRange = new Vector2(0, float.MaxValue);
    public static readonly float RotationalCostPerStat = 1;
    public static readonly Vector2 RotationalRange = new Vector2(0, float.MaxValue);
    public static readonly float RotationalAccelerationCostPerStat = 1;
    public static readonly Vector2 RotationalAccelerationRange = new Vector2(0, float.MaxValue);
    public static readonly float ScaleFactorCostPerStat = 1;
    public static readonly Vector2 ScaleFactorRange = new Vector2(0, float.MaxValue);
}
