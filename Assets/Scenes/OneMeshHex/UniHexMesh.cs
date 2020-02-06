using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniHexMesh : MonoBehaviour
{
    public UniHexMesh()
    {
        UniHexMeshData();
    }
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;
    public Mesh UniHexMeshData(Vector3Int scaleValues)
    {
        Mesh mesh = new Mesh();
        newVertices = new Vector3[6 * scaleValues.x * scaleValues.z];

        for (int i = 0; i < scaleValues.x; i++)
        {
            newVertices[0 + i] = new Vector3(0, 0, 0);
            newVertices[1 + i] = new Vector3(1, 0, 0);
            newVertices[2 + i] = new Vector3(1.5f, 0, 1);
            newVertices[3 + i] = new Vector3(1, 0, 2);
            newVertices[4 + i] = new Vector3(0, 0, 2);
            newVertices[5 + i] = new Vector3(-0.5f, 0, 1);
        };
            

        return mesh;
    }
    public Mesh UniHexMeshData()
    {
        Mesh mesh = new Mesh();
        newVertices = new Vector3[6]
        {
            //UpperSide
            new Vector3(0,0,0),
            new Vector3(1, 0, 0),
            new Vector3(1.5f, 0, 1),
            new Vector3(1, 0, 2),
            new Vector3(0,0,2),
            new Vector3(-0.5f, 0, 1),
        };

        newTriangles = new int[12]
        {
          0,5,1,
          5,2,1,
          5,4,2,
          4,3,2,
        };

        return mesh;
    }
}
