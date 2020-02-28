using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace HexMapTool 
{

    public class ToolData : ScriptableObject
    {
        private static ToolData m_toolData;

        public static ToolData Instance { get { return m_toolData; } }
        string json;

        private bool show;

        public HexGrid Grid;
        public ColorTable Table;
        string destination;

        //Tool Gui. Implements the Gui of HexGrid And Color Table
        public void OnGui() 
        {
            show = EditorGUILayout.Foldout(show, "Tool Functions", true);
            if (show)
            {
                if (GUILayout.Button("Save"))
                {
                    Save(Grid);
                    Save(Table);
                }
                if (GUILayout.Button("Load"))
                {
                    Load(Grid);
                    Load(Table);
                }
                if (GUILayout.Button("Clear"))
                {
                    Clear();
                }
            }        
        }
        //Tool Initialization. Creates appropriate folders and files the first time it runs, and loads if the paths and objects already exist
        public void Init()
        {

            if (m_toolData == null)
            {
                m_toolData = this;
            }
            else 
            {
                DestroyImmediate(this);
            }
            destination = "Assets/HexMapTool/DataBase";
            if (AssetDatabase.IsValidFolder(destination))
            {
                if (AssetDatabase.LoadAssetAtPath(destination + "/GridData.asset", typeof(HexGrid)) != null)
                {
                    Grid = (HexGrid)AssetDatabase.LoadAssetAtPath(destination + "/GridData.asset", typeof(HexGrid));
                    Table = (ColorTable)AssetDatabase.LoadAssetAtPath(destination + "/TableData.asset", typeof(ColorTable));
                }
                else
                {
                    Grid = CreateInstance<HexGrid>();
                    Table = CreateInstance<ColorTable>();
                    AssetDatabase.CreateAsset(Grid, destination + "/GridData.asset");
                    AssetDatabase.CreateAsset(Table, destination + "/TableData.asset");
                }
            }
            else
            {
                AssetDatabase.CreateFolder("HexMapTool", "Database");
                AssetDatabase.CreateAsset(Grid, destination + "/GridData.asset");
                AssetDatabase.CreateAsset(Table, destination + "/TableData.asset");
            }
            Grid.Init();
            Table.Init();
        }
        //Saves Data of type ScriptableObject from  a file created at  Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
        public void Save(ScriptableObject serializeable)
        {
            json = JsonUtility.ToJson(serializeable);

            destination = Application.persistentDataPath + "/" + serializeable.GetType().ToString()  + ".dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenWrite(destination);
            }
            else
            {
                file = File.Create(destination);
            }
            string data = json;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
        //Loads Data of type ScriptableObject from  a file created at  Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
        public void Load(ScriptableObject serializeable) 
        {
            FileStream file;
            destination = Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
            if (File.Exists(destination)) 
            {
                file = File.OpenRead(destination);
            } 
            else
            {
                Debug.LogError("File not found");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            string data = (string)bf.Deserialize(file);
            file.Close();
            json = data;
            Debug.Log(json);
            JsonUtility.FromJsonOverwrite(json, serializeable);    
        }
        //Calls the appropriate methods on each Scriptable object to clear its current data and return to the default state.
        public void Clear()
        {
            Grid.RestoreDefaults();
            Table.GetTable().Clear();
        }
    }

}
