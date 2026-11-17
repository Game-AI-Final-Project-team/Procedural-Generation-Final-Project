using System.IO;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class MyEditorWindow : EditorWindow
{
    private ScriptableObject targetAsset;
    private SerializedObject serializedAsset;

    private string newItemName = "New Sword";
    private int newItemValue = 100;
    private bool value = true;

    [MenuItem("Tools/Text File Editor")]
    public static void ShowWindow()
    {
        GetWindow<MyEditorWindow>("File Editor");
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        newItemName = EditorGUILayout.TextField("Item Name", newItemName);
        value = EditorGUILayout.Toggle("DASHUPUISDNi", value);
        if (GUILayout.Button("Create"))
        {
            // 1. Instantiate the ScriptableObject in memory
            TestScriptableObject asset = ScriptableObject.CreateInstance<TestScriptableObject>();
            asset.stringValue = newItemName;
            asset.value = value;

            // 2. Generate a safe, non-overwriting path and save to disk
            string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Stats/{newItemName}.asset");
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset; 
        }
    }
}
