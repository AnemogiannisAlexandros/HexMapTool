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


        public HexGrid Grid;
        public ColorTable Table;
        string destination;
        public void OnGui() 
        {
            if (GUILayout.Button("Save"))
            {
                Save(Grid);
                Save(Table);
            }
            if(GUILayout.Button("Load")) 
            {
                Load(Grid);
                Load(Table);
            }
        }
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
            Grid = CreateInstance<HexGrid>();
            Grid.Init();
            Table = CreateInstance<ColorTable>();
            Table.Init();
        }
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
    }

}
