using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "Scriptable Objects/EnemyStat")]
public class EnemyStatData : ScriptableObject
{
    [Tooltip("The name of this stat. Does not have to be unique.")]
    public string statName;
    [Tooltip("A key to represent this stat. Must be unqiue. Duplicate key stats will be skipped.")]
    public string key;
    [Tooltip("An icon that will be shown to represent this stat.")]
    public Texture2D statIcon;
    [Header("Stat Properties")]
    [Tooltip("How many arbitrary points are required to increase this stat by 1.")]
    public float costPerUnitStat;
    [Tooltip("The minimum value for this stat. Stats below minimum value will always be allocated to first.")]
    public float minimumValue;
    [Tooltip("The maximum value for this stat.")]
    public float maximumValue;
    [Tooltip("If true, fractional point values will not be used to increment this stat between integer values.")]
    public bool clampToInteger;
}
