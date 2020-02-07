using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGeneratorWindow : EditorWindow
{
    MapGenerator gen;
    private Object obj;
    [MenuItem("Window/HexMapGenerator")]
    static void Init()
    {
        MapGeneratorWindow window = (MapGeneratorWindow)EditorWindow.GetWindow(typeof(MapGeneratorWindow));
        window.Show();
    }
    public void OnFocus()
    {
        if (gen == null) 
        {
            Debug.Log("Generating Scriptable Object");
            gen = CreateInstance<MapGenerator>();
            gen.Init();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Base Settings", EditorStyles.boldLabel);
        gen.size = EditorGUILayout.Vector2Field("Map Size", gen.size);
        gen.hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab",gen.hexPrefab, typeof(GameObject), true);
        gen.startPos = EditorGUILayout.Vector3Field("Starting Position", gen.startPos);
        if (GUILayout.Button("Generate Map")) 
        {
            gen.CreateHexMap();
        }
        if (GUILayout.Button("Clear Map")) 
        {
            gen.ClearMap();
        }
    }
}
