using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.EditorCoroutines.Editor;


namespace HexMapTool
{
    /// <summary>
    /// Mesh of a Hexagon Map
    /// </summary>
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class HexMesh : MonoBehaviour
	{

		Mesh hexMesh;
		List<Vector3> vertices;
		List<int> triangles;
        List<Color> colors;
        MeshCollider meshCollider;

        //Create a mesh
		public void Init()
		{
			GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            //hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
			triangles = new List<int>();
            colors =  new List<Color>();
        }

        //Clear any mesh Data
        //Calculate Mesh from cell Data
        //assing collider
		public void Triangulate(HexCell[] coords)
		{
			hexMesh.Clear();
			vertices.Clear();
			triangles.Clear();
            colors.Clear();
            for (int i = 0; i < coords.Length; i++)
			{
				Triangulate(coords[i]);
			}
			hexMesh.vertices = vertices.ToArray();
			hexMesh.triangles = triangles.ToArray();
            hexMesh.colors = colors.ToArray();
            //hexMesh.RecalculateNormals();
            if (meshCollider == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
            }
        }

        //Create a triangle by getting the cell's center
        //and loop around it's 6 triangles
        //we Use 6 triangles instead of 4 for algorithmic simplicity
		void Triangulate(HexCell cell)
		{
            Vector3 center = cell.GetWorldCoordinates();
            for (int i = 0; i < 6; i++)
			{
				AddTriangle(
				center,
				center + HexMetrics.verts[i],
				center + HexMetrics.verts[i+1]
				);
                AddTriangleColor(cell.GetCellColor());
            }
        }
        //Create Triange at given Vectors
		void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			int vertexIndex = vertices.Count;
			vertices.Add(v1);
			vertices.Add(v2);
			vertices.Add(v3);
			triangles.Add(vertexIndex);
			triangles.Add(vertexIndex + 1);
			triangles.Add(vertexIndex + 2);
		}
        void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}
	}
}