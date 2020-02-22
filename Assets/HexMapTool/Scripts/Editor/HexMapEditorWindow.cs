using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    /// <summary>
    /// Main Window of our tool
    /// Holds the Scriptable object that has our data.
    /// </summary>
    public class HexMapEditorWindow : EditorWindow
    {
        private static HexMapEditorWindow _instance;

        public static HexMapEditorWindow Instance { get { return GetWindow<HexMapEditorWindow>(); } }
        public static HexGrid grid;
        private static GameObject gridObject;
        void OnEnable()
        {
            _instance = this;
            grid = (HexGrid)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/HexMap Data.asset", typeof(HexGrid));
        }

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
                //grid = CreateInstance<HexGrid>();
                gridObject = grid.Init();
            }
        }

        //Editor Window On Gui
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
