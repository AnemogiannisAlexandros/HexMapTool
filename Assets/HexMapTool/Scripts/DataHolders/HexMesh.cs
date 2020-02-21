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
        void Triangulate(HexCell cell)
        {
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                Triangulate(d, cell);
            }
        }
        //Create a triangle by getting the cell's center
        //and loop around it's 6 triangles
        //we Use 6 triangles instead of 4 for algorithmic simplicity
		void Triangulate(HexDirection direction ,HexCell cell)
		{
            Vector3 center = cell.GetWorldCoordinates();
            Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
            Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);

			AddTriangle(center, v1,v2);
            AddTriangleColor(cell.GetCellColor());

            Vector3 v3 = center + HexMetrics.GetFirstCorner(direction);
            Vector3 v4 = center + HexMetrics.GetSecondCorner(direction);

            AddQuad(v1, v2, v3, v4);

            HexCell prevNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
            HexCell neighbor = cell.GetNeighbor(direction) ?? cell;
            HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;

            AddQuadColor(
        cell.GetCellColor(),
        cell.GetCellColor(),
        (cell.GetCellColor() + prevNeighbor.GetCellColor() + neighbor.GetCellColor()) / 3f,
        (cell.GetCellColor() + neighbor.GetCellColor() + nextNeighbor.GetCellColor()) / 3f
        );

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
        //Add Different color to each vertex
        //FirstColor is for the center Vertex of the Hexagon
        void AddTriangleColor(Color c1, Color c2, Color c3)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
        }
        //Add Same Color to each Vertex
        void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	    }
        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
        {
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);
            colors.Add(c4);
        }
    }
}