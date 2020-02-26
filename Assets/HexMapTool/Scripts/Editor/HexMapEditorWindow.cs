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
        
        public static HexMapEditorWindow _instance;
        private static int selectionIndex = 0;
        private static List<string> cardNames;
        private static bool hidden = true;
        public static HexMapEditorWindow Instance { get { return GetWindow<HexMapEditorWindow>(); } }
        private static HexGrid grid;
        
        private Texture2D texture;

        public static HexGrid GridInstance { get { return grid; } }

        void OnEnable()
        {
            if (_instance == null && _instance != this)
            {
                _instance = this;
                grid = CreateInstance<HexGrid>();
                cardNames = new List<string>();
                texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/ColorTool/ColorBackground.jpg", typeof(Texture2D));
            }
        }

        [MenuItem("Window/Hex Map Tool")]
        static void Init()
        {
            HexMapEditorWindow window = (HexMapEditorWindow)EditorWindow.GetWindow(typeof(HexMapEditorWindow));
            window.Show();
            
        }

        private void Update()
        {
            grid.SetTouchedColor(ColorToolWindow.editorTable.GetTable()[selectionIndex].GetArchetypeColor());
        }

        //Editor Window On Gui
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Hex Map Generator", EditorStyles.boldLabel);
            //grid = (HexGrid)EditorGUILayout.ObjectField(grid, typeof(HexGrid), true);
            hidden = ColorToolWindow.editorTable.GetTable().Count > 0? false : true ;
            if (!hidden)
            {
                grid.OnGui();
                cardNames.Clear();
                foreach (ColorArchetype c in ColorToolWindow.editorTable.GetTable())
                {
                    cardNames.Add(c.GetArchetypeName());
                }
                selectionIndex = EditorGUILayout.Popup(selectionIndex, cardNames.ToArray());
                Color[] colors = texture.GetPixels();
                for (int x = 0; x < colors.Length; x++)
                {
                    colors[x] = ColorToolWindow.editorTable.GetTable()[selectionIndex].GetArchetypeColor();
                }
                texture.SetPixels(colors);
                texture.Apply();
                GUILayout.Label(texture);
            }            
            if (GUILayout.Button("Generate Map"))
            {
                grid.Init();

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
