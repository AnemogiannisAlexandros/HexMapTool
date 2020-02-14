//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace HexMapTool
//{
//    //Inspector window of HexCell if we ever assign it as a monobehaviour
//    [CustomEditor(typeof(HexCell))]
//    [CanEditMultipleObjects]
//    public class HexCellEditor : Editor
//    {
//        SerializedProperty coordinates;
//        private HexCoordinates cellCoordinates;
//        private void OnEnable()
//        {
//            coordinates = serializedObject.FindProperty("coordinates");
//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();
//            EditorGUILayout.PropertyField(coordinates);
//            serializedObject.ApplyModifiedProperties();
//        }
//    }

//    [CustomPropertyDrawer(typeof(HexCoordinates))]
//    public class HexCoordinatesDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            HexCoordinates coordinates = new HexCoordinates(
//            property.FindPropertyRelative("x").intValue,
//            property.FindPropertyRelative("z").intValue
//        );

//            position = EditorGUI.PrefixLabel(position, label);
//            GUI.Label(position, coordinates.ToString());
//        }
//    }
//}
