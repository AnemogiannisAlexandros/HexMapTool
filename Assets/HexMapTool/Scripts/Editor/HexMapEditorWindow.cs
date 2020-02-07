using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    public class HexMapEditorWindow : EditorWindow
    {
        static HexGrid grid;
        private static Object gridScriptableObject;
        [MenuItem("Window/Hex Map Tool")]
        static void Init()
        {
            HexMapEditorWindow window = (HexMapEditorWindow)EditorWindow.GetWindow(typeof(HexMapEditorWindow));
            window.Show();
            gridScriptableObject = AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Scripts/Editor/HexMap Data.asset", typeof(HexGrid));
            grid = (HexGrid)gridScriptableObject;
            grid.Init();
        }

        public void OnFocus()
        {
            //if (grid == null) 
            //{
                
            //}
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Hex Map Generator", EditorStyles.boldLabel);
            gridScriptableObject =  EditorGUILayout.ObjectField(gridScriptableObject, typeof(HexGrid), true);
            if (GUILayout.Button("Generate Map"))
            {
                Debug.Log("Generating Map");
                grid.CreateGrid();
            }
            if (GUILayout.Button("Clear Map"))
            {
                Debug.Log("Deleting Map");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
