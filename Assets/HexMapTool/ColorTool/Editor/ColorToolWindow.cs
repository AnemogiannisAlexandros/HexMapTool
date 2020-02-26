using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    public class ColorToolWindow : EditorWindow
    {
        public static ColorTable editorTable;
        private SerializedObject colorProperty;

        [MenuItem("Window/Color Table")]
        static void Init()
        {
            ColorToolWindow window = (ColorToolWindow)EditorWindow.GetWindow(typeof(ColorToolWindow));
            window.Show();            
        }

        public void OnEnable()
        {
            editorTable = CreateInstance<ColorTable>();
            editorTable.Init();
        }

        private void OnGUI()
        {
            colorProperty = new SerializedObject(HexMapEditorWindow.GridInstance);
            SerializedProperty property = colorProperty.FindProperty("touchedColor");
            
            //editorTable = (ColorTable)EditorGUILayout.ObjectField(editorTable, typeof(ColorTable), false);
            editorTable.OnGui();

        }
    }
}
