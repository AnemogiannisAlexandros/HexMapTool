using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Originals;
using HexMesh = Originals.HexMesh;
using UnityEditor;

public class MeshPrimitiveWindow : Editor
{
    [MenuItem("GameObject/3D Object/Hex")]

    private static void CreateHex() 
    {
        GameObject hex = new GameObject();
        hex.AddComponent<MeshFilter>();
        hex.AddComponent<MeshRenderer>();
        Originals.HexMesh mesh = new Originals.HexMesh();
        hex.GetComponent<MeshFilter>().mesh = mesh.HexMeshData();
        hex.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
        hex.transform.position = Vector3.zero;
    }
}
