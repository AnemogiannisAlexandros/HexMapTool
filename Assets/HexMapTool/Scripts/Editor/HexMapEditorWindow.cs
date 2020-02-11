using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    public class HexMapEditorWindow : EditorWindow
    {
        static HexGrid grid;
        private static GameObject gridObject;
        [MenuItem("Window/Hex Map Tool")]
        static void Init()
        {
            HexMapEditorWindow window = (HexMapEditorWindow)EditorWindow.GetWindow(typeof(HexMapEditorWindow));
            window.Show();
            
        }


        public void OnFocus()
        {
            if (gridObject == null)
            {
                grid = (HexGrid)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/HexMap Data.asset", typeof(HexGrid));
                //grid = CreateInstance<HexGrid>();
                gridObject = grid.Init();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Hex Map Generator", EditorStyles.boldLabel);
            grid =  (HexGrid)EditorGUILayout.ObjectField(grid, typeof(HexGrid), true);
            grid.OnGui();
            if (GUILayout.Button("Generate Map"))
            {
               // Debug.Log("Generating Map");
                grid.CreateGrid();
            }
            if (GUILayout.Button("Clear Map"))
            {
               // Debug.Log("Deleting Map");
                grid.DestroyGrid();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
