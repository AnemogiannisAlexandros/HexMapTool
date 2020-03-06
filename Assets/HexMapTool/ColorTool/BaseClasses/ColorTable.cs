using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;
using UnityEditorInternal;

namespace HexMapTool
{
    public interface IColorTable
    {
        ColorArchetype GetColorArchetype(int index);
        ColorArchetype FindColorArchetype(ColorArchetype archetype);
    }

    public class ColorTable : ScriptableObject, IColorTable
    {
        #region Data
        private string tableName;
        [SerializeField]
        private List<ColorArchetype> chosenColors = new List<ColorArchetype>();
        ColorTableCustomEditor editor;
        #endregion

        #region Constructors
        public ColorTable(string tableName ,params Color[] colors) 
        {
            this.tableName = tableName;
            foreach (Color c in colors) 
            {
                chosenColors.Add(new ColorArchetype("SomeName",c));
            }
        }
        public ColorTable(string tableName,params ColorArchetype[] archetypes) 
        {
            this.tableName = tableName;
            foreach (ColorArchetype c in archetypes) 
            {
                chosenColors.Add(c);
            }
        }
        public ColorTable() 
        {
            chosenColors.Add(new ColorArchetype());
        }
        #endregion

      
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            if (editor == null) 
            {
                editor = new ColorTableCustomEditor(this);
                editor.Init();
            }
        }
        #region Getters_Setters_HelperFunctions
        public string GetTableName()
        {
            return tableName;
        }
        public List<ColorArchetype> GetTable()
        {
            return chosenColors;
        }
        public void SetTable(List<ColorArchetype> table)
        {
            this.chosenColors = table;
        }
        public void AddColor()
        {    
            chosenColors.Add(new ColorArchetype());
        }
        public void RemoveColor(ColorArchetype archetype)
        {
            chosenColors.Remove(archetype);
        }
        public ColorArchetype FindColorArchetype(ColorArchetype archetype)
        {
            foreach (ColorArchetype c in chosenColors)
            {
                if (c.GetArchetypeName() == archetype.GetArchetypeName())
                {
                    return c;
                }
            }
            return null;
        }

        public ColorArchetype GetColorArchetype(int index)
        {
            return chosenColors[index];
        }
        #endregion Getters_Setters
        public void OnGui()
        {
            editor.OnGui();
        }
    }
   
    /// <summary>
    /// Custom Editor of ColorTable Class
    /// </summary>
    public class ColorTableCustomEditor
    {
        #region Data
        private ScriptableObject target;
        private SerializedObject colorProperty;
        private bool show;
        SerializedProperty archetypeList;
        private ReorderableList reorderableList;
        SerializedObject so;
        #endregion
        #region Constructors
        public ColorTableCustomEditor()
        {

        }
        public ColorTableCustomEditor(ScriptableObject obj)
        {
            target = obj;
        }
        #endregion
       
        //Creates our Reorderable List of Custom ColorArchetypes
        public void Init()
        {
            so = new SerializedObject(target);
            

            archetypeList = so.FindProperty("chosenColors");

            reorderableList = new ReorderableList(so, archetypeList) 
            {
                draggable = true,
                displayAdd = true,
                displayRemove = true,
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Color List"); },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    SerializedProperty element = archetypeList.GetArrayElementAtIndex(index);
                    SerializedProperty Name = element.FindPropertyRelative("archetypeName");
                    SerializedProperty Color = element.FindPropertyRelative("color");
                    SerializedProperty Behaviour = element.FindPropertyRelative("colorJob");
                    SerializedProperty Data = element.FindPropertyRelative("jobData");

                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Name);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Color);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Behaviour);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Data);
                    rect.y += EditorGUIUtility.singleLineHeight;
                },
                elementHeight = EditorGUIUtility.singleLineHeight * 5,
                onAddCallback = list =>
                {
                    int index = list.serializedProperty.arraySize;

                    list.serializedProperty.arraySize++;
                    list.index = index;
                    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                    SerializedProperty Name = element.FindPropertyRelative("archetypeName");
                    SerializedProperty Color = element.FindPropertyRelative("color");
                    SerializedProperty Behaviour = element.FindPropertyRelative("colorJob");
                    SerializedProperty Data = element.FindPropertyRelative("jobData");

                    Name.stringValue = "";
                    Color.colorValue = new Color(1, 1, 1, 1);
                    Behaviour = null;
                    Data = null;
                    show = true;
                }

            };
           
        }
        //On Gui Method of our Color Table Class
        public void OnGui()
        {
            so.Update();
            colorProperty = new SerializedObject(ToolData.Instance.Grid);
            SerializedProperty property = colorProperty.FindProperty("touchedColor");
            show = EditorGUILayout.Foldout(show, "Color Card Collection",true);
            if (show)
            {
                if (GUILayout.Button("Load Presets")) 
                {
                    ToolData.Load(ToolData.Instance.Table);
                }
                if (GUILayout.Button("Save Collection"))
                {
                    ToolData.Save(ToolData.Instance.Table);
                }
                reorderableList.DoLayoutList();
            }
            so.ApplyModifiedProperties();
        }
    }
}
