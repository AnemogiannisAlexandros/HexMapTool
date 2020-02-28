using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexMapTool;

[CustomEditor(typeof(HexMesh))]
public class HexMeshCustomEditor : Editor
{
    SerializedProperty hexMesh;
    SerializedProperty vertices;
    SerializedProperty triangles;
    SerializedProperty colors;

    private HexMesh myTarget;

    public void OnEnable()
    {
        myTarget = (HexMesh)target;
        hexMesh = serializedObject.FindProperty("hexMesh");
        vertices = serializedObject.FindProperty("vertices");
        triangles = serializedObject.FindProperty("triangles");
        colors = serializedObject.FindProperty("colors");
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        EditorGUILayout.PropertyField(hexMesh);
        EditorGUILayout.PropertyField(vertices,true);
        EditorGUILayout.PropertyField(triangles, true);
        EditorGUILayout.PropertyField(colors, true);
        serializedObject.ApplyModifiedProperties();

        
        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("Runnign");
        }
    }
    public void OnSceneGUI()
    {

    }
}

