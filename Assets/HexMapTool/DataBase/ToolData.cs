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
        public MeshData MeshDataObj;
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
                if (GUILayout.Button("Clear All"))
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
                ColorTable preset03 = new ColorTable("RPGTemplate", new ColorArchetype("Heal", new Color(0.515f, 1.0f, .131f, 1.0f)),new ColorArchetype("Do Damage", new Color(.7f,.31f,.3f,1f)),
                new ColorArchetype("Quest",new Color(1f,.95f,.155f,1f)), new ColorArchetype("Dialogue",new Color(0.731f,1f,0.992f,1f)),
                new ColorArchetype("City", new Color(0.641f,0.641f,0.641f,1f)),new ColorArchetype("Vendor",new Color(0.425f,0.606f,0.990f,1f)),
                new ColorArchetype("AMBUSH!",new Color(1f,0.156f,0f,1f)), new ColorArchetype("Treasure",new Color(0.820f,0.401f,0.181f,1f)),
                new ColorArchetype("Secret", new Color(0.834f,0.291f,0.981f,1f)),new ColorArchetype("Teleport", new Color(0.990f,0.658f,0.22f,1f)),
                new ColorArchetype("Difficult Terrain",new Color(0.962f,0.925f,0.658f,1f)));
                ColorTable preset04 = new ColorTable("TerrainTemplate", new ColorArchetype("Deep Sea", new Color(0.1033f, 0.2333f, 0.8113f, 1.0f)), new ColorArchetype("Light Sea", new Color(0.0317f, 0.7895f, 0.9622f, 1f)),
                new ColorArchetype("Sand", new Color(0.8584f, .8012f, .5709f, 1f)), new ColorArchetype("Light Green", new Color(0.6458f, 1f, 0.4198f, 1f)),
                new ColorArchetype("Deep Green", new Color(0.0942f, 0.7830f, 0.0849f, 1f)), new ColorArchetype("Deeper Green", new Color(0.2114f, 0.4811f, 0.1339f, 1f)),
                new ColorArchetype("Light Mountain", new Color(.7547f, 0.5588f, 2669f, 1f)), new ColorArchetype("Mountain", new Color(0.820f, 0.401f, 0.181f, 1f)),
                new ColorArchetype("High Mountain", new Color(0.4433f, 0.2185f, 0.1150f, 1f)), new ColorArchetype("Rock", new Color(0.45f, 0.45f, 0.45f, 1f)),
                new ColorArchetype("Difficult Terrain", new Color(1f, 1f, 1f, 1f)));
                DirectSave(preset01.GetTableName(), preset01);
                DirectSave(preset02.GetTableName(), preset02);
                DirectSave(preset03.GetTableName(), preset03);
                DirectSave(preset04.GetTableName(), preset04);

            }
            if (!Directory.Exists(Application.persistentDataPath + "/MeshDataSaves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/MeshDataSaves");
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
                    MeshDataObj = (MeshData)AssetDatabase.LoadAssetAtPath(destination + "/MeshData.asset", typeof(MeshData));
                }
                else
                {
                    Grid = CreateInstance<HexGrid>();
                    Table = CreateInstance<ColorTable>();
                    MeshDataObj = CreateInstance<MeshData>();
                    AssetDatabase.CreateAsset(Grid, destination + "/GridData.asset");
                    AssetDatabase.CreateAsset(Table, destination + "/TableData.asset");
                    AssetDatabase.CreateAsset(MeshDataObj, destination + "/MeshData.asset");
                }
            }
            else
            {
                AssetDatabase.CreateFolder("HexMapTool", "Database");
                AssetDatabase.CreateAsset(Grid, destination + "/GridData.asset");
                AssetDatabase.CreateAsset(Table, destination + "/TableData.asset");
                AssetDatabase.CreateAsset(MeshDataObj, destination + "/MeshData.asset");
            }
            Grid.Init();
            Table.Init();
        }
        private void DirectSave(string fileName,ScriptableObject serializeable) 
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
                case MeshData md:
                    {
                        destination = Application.persistentDataPath + "/MeshDataSaves";
                        break;
                    }
                default:
                    {
                        destination = Application.persistentDataPath;
                        break;
                    }

            }
            //Debug.Log(destination);
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
                case MeshData md:
                    {
                        destination = Application.persistentDataPath + "/MeshDataSaves";
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
        public static void DirectLoad(ScriptableObject serializeable, string fileName, string extention)
        {
            FileStream file;
            string path;
            switch (serializeable)
            {
                case HexGrid gr:
                    {
                        path = Application.persistentDataPath + "/GridSaves/";
                        break;
                    }
                case ColorTable tb:
                    {
                        path = Application.persistentDataPath + "/ColorTableSaves/";
                        break;
                    }
                case MeshData md:
                    {
                        path = Application.persistentDataPath + "/MeshDataSaves/";
                        break;
                    }
                default:
                    {
                        path = Application.persistentDataPath;
                        break;
                    }

            }
            string destination = path + fileName + extention;
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
            //Debug.Log(json); 
            JsonUtility.FromJsonOverwrite(json, serializeable);
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
                case MeshData md:
                    {
                        path = Application.persistentDataPath + "/MeshDataSaves";
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
            //Debug.Log(json);
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
