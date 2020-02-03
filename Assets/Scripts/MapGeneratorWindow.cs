using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGeneratorWindow : EditorWindow
{
    PrefabUtility prefab;
    [MenuItem("Tools/HexMapGenerator")]

    static void Init() 
    {
        MapGeneratorWindow window = (MapGeneratorWindow)EditorWindow.GetWindow(typeof(MapGeneratorWindow));
        window.Show();
    }

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Base Settings", EditorStyles.boldLabel);
        if (GUILayout.Button("Generate Map")) 
        {
        }
    }
}
