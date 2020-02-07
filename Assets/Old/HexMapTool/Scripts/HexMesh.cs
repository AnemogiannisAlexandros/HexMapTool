using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexMesh
{
    public HexMesh() 
    {
        HexMeshData();
    }
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    public Mesh HexMeshData()
    {
        Mesh mesh = new Mesh();
        newVertices = new Vector3[12]
        {
            //UpperSide
            new Vector3(0,0,0),
            new Vector3(1, 0, 0),
            new Vector3(1.5f, 0, 1),
            new Vector3(1, 0, 2),
            new Vector3(0,0,2),
            new Vector3(-0.5f, 0, 1),
            //LowerSide
            new Vector3(0,-.25f,0),
            new Vector3(1, -.25f, 0),
            new Vector3(1.5f, -.25f, 1),
            new Vector3(1, -.25f, 2),
            new Vector3(0,-.25f,2),
            new Vector3(-0.5f, -.25f, 1)
        };


        newTriangles = new int[60]
        {
            //TopSide
          0,5,1,
          5,2,1,
          5,4,2,
          4,3,2,
          //LowerSide
          6,7,11,
          11,7,8,
          11,8,10,
          10,8,9,
          //All Sides
          6,1,7,
          6,0,1,

          7,2,8,
          7,1,2,

          8,3,9,
          8,2,3,

          9,4,10,
          9,3,4,

          10,5,11,
          10,4,5,

          11,0,6,
          11,5,0
        };

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
