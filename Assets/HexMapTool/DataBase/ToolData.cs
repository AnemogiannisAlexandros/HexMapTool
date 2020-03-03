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

        private void Awake()
        {
            if (m_toolData == null)
            {
                m_toolData = this;
            }
            else 
            {
                DestroyImmediate(this);
            }
        }

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
            if (!Directory.Exists(Application.persistentDataPath + "/GridSaves")) 
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/GridSaves");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/ColorTableSaves")) 
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/ColorTableSaves");
                ColorTable preset01 = new ColorTable("RGB", new ColorArchetype("Red", Color.red), new ColorArchetype("Green", Color.green), new ColorArchetype("Blue", Color.blue)); ;
                ColorTable preset02 = new ColorTable("YCM", new ColorArchetype("Yellow", Color.yellow), new ColorArchetype("Cyan", Color.cyan), new ColorArchetype("Magenta", Color.magenta)); ;
                SaveNoPanel("RGB",preset01);
                SaveNoPanel("YCM",preset02);
            }
            if (!Directory.Exists(Application.persistentDataPath + "/MeshGridData"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/MeshGridData");
            }

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
        private void SaveNoPanel(string fileName,ScriptableObject serializeable) 
        {
            string destination;
            switch (serializeable)
            {
                case HexGrid gr:
                    {
                        destination = Application.persistentDataPath + "/GridSaves/";
                        break;
                    }
                case ColorTable tb:
                    {
                        destination = Application.persistentDataPath + "/ColorTableSaves/";
                        break;
                    }
                case null:
                    {
                        destination = Application.persistentDataPath;
                        break;
                    }
                default:
                    {
                        destination = Application.persistentDataPath;
                        break;
                    }

            }
            Debug.Log(destination);
            string json = JsonUtility.ToJson(serializeable);




            string path = destination + fileName + ".dat";
            FileStream file;
            int i = 0;
            while (File.Exists(path)) 
            {
                path = destination + fileName + i + ".dat";
                i++;
            }
            file = File.Create(path);
            File.SetAttributes(path, FileAttributes.Normal);
            string data = json;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
        //Saves Data of type ScriptableObject from  a file created at  Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
        public static void Save(ScriptableObject serializeable)
        {
            string destination;
            switch (serializeable)
            {
                case HexGrid gr:
                    {
                        destination = Application.persistentDataPath + "/GridSaves";
                        break;
                    }
                case ColorTable tb:
                    {
                        destination = Application.persistentDataPath + "/ColorTableSaves";
                        break;
                    }
                case null:
                    {
                        destination = Application.persistentDataPath;
                        break;
                    }
                default:
                    {
                        destination = Application.persistentDataPath;
                        break;
                    }

            }
           string json = JsonUtility.ToJson(serializeable);
            

          

            string path = EditorUtility.SaveFilePanel("Save "+ serializeable.GetType().ToString(), destination, serializeable.GetType().ToString(), "dat");
            FileStream file;

            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                file = File.OpenWrite(path);
            }
            else
            {
                file = File.Create(path);
                File.SetAttributes(path, FileAttributes.Normal);

            }
            string data = json;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
        //Loads Data of type ScriptableObject from  a file created at  Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
        public static void Load(ScriptableObject serializeable) 
        {
            FileStream file;
            string path;
            switch (serializeable)
            {
                case HexGrid gr:
                    {
                        path = Application.persistentDataPath + "/GridSaves";
                        break;
                    }
                case ColorTable tb:
                    {
                        path = Application.persistentDataPath + "/ColorTableSaves";
                        break;
                    }
                default:
                    {
                        path = Application.persistentDataPath;
                        break;
                    }

            }
            string destination = EditorUtility.OpenFilePanel("Load " + serializeable.GetType().ToString(), path, "dat");
            //destination = Application.persistentDataPath + "/" + serializeable.GetType().ToString() + ".dat";
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
            string json = data;
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
