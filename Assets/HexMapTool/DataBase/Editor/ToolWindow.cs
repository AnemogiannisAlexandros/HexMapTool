using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexMapTool 
{
    public class ToolWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private ToolData myToolData;
        [MenuItem("Window/HexTool")]
        static void InitTool()
        {
            ToolWindow window = (ToolWindow)EditorWindow.GetWindow(typeof(ToolWindow));
            //window.titleContent = new GUIContent("Hex Tool","Hey!");
            window.Show();
        }
        //Initialize ToolData
        public void OnEnable()
        {

            //Debug.Log("First Time");
            myToolData = CreateInstance<ToolData>();
            myToolData.Init();
        }
        //Tool OnGui
        public void OnGUI()
        {
            myToolData.OnGui();
            GuiLine();
            myToolData.Grid.OnGui();
            GuiLine();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            myToolData.Table.OnGui();
            GuiLine();
            EditorGUILayout.EndScrollView();
        }
        //Single Line Separator
        public void GuiLine(int i_height = 2)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(.65f, .65f, .65f, 1));
        }
    }
}