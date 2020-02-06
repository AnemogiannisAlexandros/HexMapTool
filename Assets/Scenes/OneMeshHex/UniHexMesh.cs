using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniHexMesh : MonoBehaviour
{
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    private const int hexTriangles = 6;
    private const int hexVerts = 7;
    private const int allTriangleVerts = 18;
    private const int tirangleVerts = 3;
    public void Start()
    {
        GetComponent<MeshFilter>().mesh = UniHexMeshData(3,0,6);
        
    }
    public UniHexMesh()
    {
        
    }


    public Mesh UniHexMeshData(int x, float y, int z)
    {
        Mesh mesh = new Mesh();
        newVertices = new Vector3[hexVerts * x * z];
        newTriangles = new int[hexTriangles * x * z * tirangleVerts];

        Vector3 currentOffset = Vector3.zero;
        ///NEED TO FIX
        ///

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++) 
            {
                //Vertices Calculation
                newVertices[0 + (i * z) + j * hexVerts] = new Vector3(0, 0, 0) + currentOffset;
                newVertices[1 + (i * z) + j *  hexVerts] = new Vector3(-0.25f, 0, -0.5f) + currentOffset;
                newVertices[2 + (i * z) + j *  hexVerts] = new Vector3(.25f, 0, -0.5f) + currentOffset;
                newVertices[3 + (i * z) + j *  hexVerts] = new Vector3(.5f, 0, 0) + currentOffset;
                newVertices[4 + (i * z) + j *  hexVerts] = new Vector3(.25f, 0, .5f) + currentOffset;
                newVertices[5 + (i * z) + j *  hexVerts] = new Vector3(-0.25f, 0, .5f) + currentOffset;
                newVertices[6 + (i * z) + j *  hexVerts] = new Vector3(-.5f, 0, 0) + currentOffset;
                currentOffset += new Vector3(0, 0, 1);
            }
            if (i % 2 == 0)
            {
                currentOffset = new Vector3(0.75f * i, 0, .5f);
            }
            else
            {
                currentOffset = new Vector3(0.75f * i, 0, 0);
            }
            //newVertices[0 + (i + 1) * hexVerts] = new Vector3(0, 0, 0) + new Vector3(0.5f*i, 0,0);
            //newVertices[1 + (i + 1) * hexVerts] = new Vector3(-0.25f, 0, -0.5f) + new Vector3(0.5f * i, 0, 0);
            //newVertices[2 + (i + 1) * hexVerts] = new Vector3(.25f, 0, -0.5f) + new Vector3(0.5f * i, 0, 0);
            //newVertices[3 + (i + 1) * hexVerts] = new Vector3(.5f, 0, 0) + new Vector3(0.5f * i, 0, 0);
            //newVertices[4 + (i + 1) * hexVerts] = new Vector3(.25f, 0, .5f) + new Vector3(0.5f * i, 0, 0);
            //newVertices[5 + (i + 1) * hexVerts] = new Vector3(-0.25f, 0, .5f) + new Vector3(0.5f * i, 0, 0);
            //newVertices[6 + (i + 1) * hexVerts] = new Vector3(-.5f, 0, 0) + new Vector3(0.5f * i, 0, 0);
        };

        int repetitions = 0;
        for (int i = 0; i < hexVerts*x*z; i+=hexVerts) 
        {
            newTriangles[0 + repetitions * allTriangleVerts] = 1 + i;
            newTriangles[1 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[2 + repetitions * allTriangleVerts] = 2 + i;
            newTriangles[3 + repetitions * allTriangleVerts] = 2 + i;
            newTriangles[4 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[5 + repetitions * allTriangleVerts] = 3 + i;
            newTriangles[6 + repetitions * allTriangleVerts] = 3 + i;
            newTriangles[7 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[8 + repetitions * allTriangleVerts] = 4 + i;
            newTriangles[9 + repetitions * allTriangleVerts] = 4 + i;
            newTriangles[10 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[11 + repetitions * allTriangleVerts] = 5 + i;
            newTriangles[12 + repetitions * allTriangleVerts] = 5 + i;
            newTriangles[13 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[14 + repetitions * allTriangleVerts] = 6 + i;
            newTriangles[15 + repetitions * allTriangleVerts] = 6 + i;
            newTriangles[16 + repetitions * allTriangleVerts] = 0 + i;
            newTriangles[17 + repetitions * allTriangleVerts] = 1 + i;
            repetitions++;

        }
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();

        return mesh;
    }
    public Mesh UniHexMeshData()
    {
        Mesh mesh = new Mesh();
        newVertices = new Vector3[7]
        {
            //UpperSide
            new Vector3(0,0,0),
            new Vector3(-0.25f, 0, -0.5f),
            new Vector3(.25f, 0, -0.5f),
            new Vector3(.5f, 0, 0),
            new Vector3(.25f,0,.5f),
            new Vector3(-0.25f, 0, .5f),
            new Vector3(-.5f,0,0)
        };

        newTriangles = new int[18]
        {
          1,0,2,
          2,0,3,
          3,0,4,
          4,0,5,
          5,0,6,
          6,0,1
        };
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
