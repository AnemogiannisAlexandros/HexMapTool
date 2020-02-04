using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGeneratorWindow : EditorWindow
{
    PrefabUtility prefab;
    [MenuItem("Tools/HexMapGenerator")]

    static void Init() 
    {
        MapGeneratorWindow window = (MapGeneratorWindow)EditorWindow.GetWindow(typeof(MapGeneratorWindow));
        window.Show();
        //Create Appropriate Folder and meshes if needed.
       
        if (!AssetDatabase.IsValidFolder("Assets/HexMapTool"))
        {
            
            //Create apropriate Folders
            AssetDatabase.CreateFolder("Assets", "HexMapTool");
            AssetDatabase.CreateFolder("Assets/HexMapTool", "Mesh");
            AssetDatabase.CreateFolder("Assets/HexMapTool", "Prefabs");
            AssetDatabase.CreateFolder("Assets/HexMapTool", "Materials");

            //Create the Mesh as an asset.
            HexMesh mesh = new HexMesh();
            AssetDatabase.CreateAsset(mesh.HexMeshData(), "Assets/HexMapTool/Mesh/Hex.asset");
            //Create Default Material for prefab
            Material mat = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(mat, "Assets/HexMapTool/Prefabs/HexMat.mat");
            //Create Object
            GameObject obj = CreateHex();
            //Save Prefab
            PrefabUtility.SaveAsPrefabAsset(obj, "Assets/HexMapTool/Prefabs/Hex.prefab");
            //Destroy Objcect from Scene
            DestroyImmediate(obj);
            //Save
            AssetDatabase.SaveAssets();
        }
    }
    private static GameObject CreateHex()
    {
        GameObject hex = new GameObject();
        hex.name = "Hex";
        hex.AddComponent<MeshFilter>();
        hex.AddComponent<MeshRenderer>();
        hex.GetComponent<MeshFilter>().mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Mesh/Hex.asset",typeof(Mesh));
        hex.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/HexMat.mat", typeof(Material)) as Material;
        hex.transform.position = Vector3.zero;
        hex.transform.rotation = Quaternion.Euler(0, 90, 0);
        hex.AddComponent<HexAttributes>();
        return hex;
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Base Settings", EditorStyles.boldLabel);
        if (GUILayout.Button("Generate Map")) 
        {
        }
    }
}
