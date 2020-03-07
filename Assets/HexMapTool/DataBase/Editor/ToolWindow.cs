using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexMapTool 
{
    public class ToolWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private ToolData myToolData;
        private bool canRun = false;
        [MenuItem("Window/HexTool %#F1")]
        static void InitTool()
        {
            ToolWindow window = (ToolWindow)EditorWindow.GetWindow(typeof(ToolWindow));
            //window.titleContent = new GUIContent("Hex Tool","Hey!");
            window.Show();
        }
        //Initialize ToolData
        public void OnEnable()
        {

            if (myToolData == null)
            {
                myToolData = CreateInstance<ToolData>();
                myToolData.Init();
                canRun = true;
            }
        }
        private void Awake()
        {
            if (myToolData == null)
            {
                myToolData = CreateInstance<ToolData>();
                myToolData.Init();
                canRun = true;
            }

        }
        //Tool OnGui
        public void OnGUI()
        {
            if (canRun)
            {
                myToolData.OnGui();
                GuiLine();
                myToolData.Grid.OnGui();
                GuiLine();
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                myToolData.Table.OnGui();
                GuiLine();
                EditorGUILayout.EndScrollView();
            }
        }
        //Single Line Separator
        public void GuiLine(int i_height = 2)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(.65f, .65f, .65f, 1));
        }
        public void OnDestroy()
        {
            canRun = false;
        }
    }
}