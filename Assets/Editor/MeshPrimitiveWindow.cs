using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshPrimitiveWindow : Editor
{
    [MenuItem("GameObject/3D Object/Hex")]



    private static void CreateHex() 
    {
        GameObject hex = new GameObject();
        hex.name = "Hex";
        hex.AddComponent<MeshFilter>();
        hex.AddComponent<MeshRenderer>();
        HexMesh mesh = new HexMesh();
        hex.GetComponent<MeshFilter>().mesh = mesh.HexMeshData();
        hex.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
        hex.transform.position = Vector3.zero;
        hex.transform.rotation = Quaternion.Euler(0, 90, 0);
        hex.AddComponent<HexAttributes>();

        //Create the necessary Geometry for our Mesh
        //And the coresponding Folder.
    }
}
