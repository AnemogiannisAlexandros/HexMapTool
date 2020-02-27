using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexMapTool 
{
    public class ToolWindow : EditorWindow
    {
        private ToolData myToolData;
        [MenuItem("Window/HexTool")]
        static void InitTool()
        {
            ToolWindow window = (ToolWindow)EditorWindow.GetWindow(typeof(ToolWindow));
            //window.titleContent = new GUIContent("Hex Tool","Hey!");
            window.Show();
        }
        public void OnEnable()
        {

            Debug.Log("First Time");
            myToolData = CreateInstance<ToolData>();
            myToolData.Init();
        }
        public void OnGUI()
        {
            myToolData.OnGui();
            myToolData.Grid.OnGui();
            myToolData.Table.OnGui();
        }
    }
}