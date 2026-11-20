using UnityEditor;
using UnityEngine;

public class StatCreationWindow : EditorWindow
{
    private string statName = "New Stat";
    private string statKey = "new_stat";
    private Texture2D statIcon = null;
    private float statCostPerUnit = 1.0f;
    private float statMinimumValue = 0.0f;
    private float statMaximumValue = 100.0f;
    private bool clampToInteger = false;


    [MenuItem("Tools/Create Stat")]
    public static void ShowWindow()
    {
        GetWindow<StatCreationWindow>("Stat Creation");
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        statName = EditorGUILayout.TextField("Stat Name", statName);
        statKey = EditorGUILayout.TextField("Stat Key", statKey);
        statIcon = (Texture2D)EditorGUILayout.ObjectField("Stat Texture", statIcon, typeof(Texture2D), false);
        statCostPerUnit = EditorGUILayout.FloatField("Stat Cost per Unit", statCostPerUnit);
        statMinimumValue = EditorGUILayout.FloatField("Stat Minimum Value", statMinimumValue);
        statMaximumValue = EditorGUILayout.FloatField("Stat Maximum Value", statMaximumValue);
        clampToInteger = EditorGUILayout.Toggle("Integer Only", clampToInteger);
        if (GUILayout.Button("Create"))
        {
            // 1. Instantiate the ScriptableObject in memory
            EnemyStatData asset = ScriptableObject.CreateInstance<EnemyStatData>();
            asset.statName = statName;
            asset.key = statKey;
            asset.statIcon = statIcon;
            asset.costPerUnitStat = statCostPerUnit;
            asset.minimumValue = statMinimumValue;
            asset.maximumValue = statMaximumValue;
            asset.clampToInteger = clampToInteger;

            // 2. Generate a safe, non-overwriting path and save to disk
            string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Stats/{statName}.asset");
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset; 

            // if (EditorUtility.DisplayDialog("Confirmation", "Message", "Okay"))
            // {
                
            // }
        }
    }
}
