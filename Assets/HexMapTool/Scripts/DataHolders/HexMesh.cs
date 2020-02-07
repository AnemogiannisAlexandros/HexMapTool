using UnityEngine;
using System.Collections.Generic;

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
        MeshCollider meshCollider;

		public void Init()
		{
			GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            //hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
			triangles = new List<int>();
		}

		public void Triangulate(HexCell[] cells)
		{
			hexMesh.Clear();
			vertices.Clear();
			triangles.Clear();
			for (int i = 0; i < cells.Length; i++)
			{
				Triangulate(cells[i]);
			}
			hexMesh.vertices = vertices.ToArray();
			hexMesh.triangles = triangles.ToArray();
			hexMesh.RecalculateNormals();
            if (meshCollider == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
            }
        }

		void Triangulate(HexCell cell)
		{

			Vector3 center = cell.transform.localPosition;
			for (int i = 0; i < 6; i++)
			{
				AddTriangle(
				center,
				center + HexMetrics.verts[i],
				center + HexMetrics.verts[i+1]
				);
			}
		}
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
	}
}