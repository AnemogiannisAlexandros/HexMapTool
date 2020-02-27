//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace HexMapTool
//{
//    /// <summary>
//    /// Main Window of our tool
//    /// Holds the Scriptable object that has our data.
//    /// </summary>
//    public class HexMapEditorWindow : EditorWindow
//    {
        
//        public static HexMapEditorWindow _instance;
        
//        public static HexMapEditorWindow Instance { get { return GetWindow<HexMapEditorWindow>(); } }
//        private static HexGrid grid;
        
       

//        public static HexGrid GridInstance { get { return grid; } }

//        void OnEnable()
//        {
//            if (_instance == null && _instance != this)
//            {
//                _instance = this;
//                grid = CreateInstance<HexGrid>();
                
//            }
//        }

//        [MenuItem("Window/Hex Map Tool")]
//        static void Init()
//        {
//            HexMapEditorWindow window = (HexMapEditorWindow)EditorWindow.GetWindow(typeof(HexMapEditorWindow));
//            window.Show();
            
//        }

//        private void Update()
//        {
//            grid.SetTouchedColor(ColorToolWindow.editorTable.GetTable()[selectionIndex].GetArchetypeColor());
//        }

//        //Editor Window On Gui
//        private void OnGUI()
//        {
           
//        }
//    }
//}
