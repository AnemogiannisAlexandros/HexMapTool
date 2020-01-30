using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : EditorWindow
{
    [MenuItem("Tools/HexMapGenerator")]

    static void Init() 
    {
        MapGenerator window = (MapGenerator)EditorWindow.GetWindow(typeof(MapGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Base Settings", EditorStyles.boldLabel);
    }
}
