using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    public class HexMapEditorWindow : EditorWindow
    {
       HexGrid grid;

        [MenuItem("Window/Hex Map Tool")]
        static void Init()
        {
            HexMapEditorWindow window = (HexMapEditorWindow)EditorWindow.GetWindow(typeof(HexMapEditorWindow));
            window.Show();
        }

        public void OnFocus()
        {
            if (grid == null) 
            {
                grid = CreateInstance<HexGrid>();
                grid.Init();
            }
            
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Hex Map Generator", EditorStyles.boldLabel);
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
