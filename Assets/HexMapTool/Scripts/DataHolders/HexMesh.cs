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
            hexMesh = new Mesh();
            GetComponent<MeshFilter>().mesh = hexMesh;
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
                //Debug.Log(hexMesh.vertices.Length);
                // hexMesh.RecalculateBounds();
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
            if (vertices.Count < 65000)
            {
                AddTriangle(center, v1, v2);
                AddTriangleColor(cell.GetCellColor());

                if (direction == HexDirection.NE)
                {
                    TriangulateConnection(direction, cell, v1, v2);
                }
                if (direction <= HexDirection.SE)
                {
                    TriangulateConnection(direction, cell, v1, v2);
                }
            }
            else
            {
                //To Do Another Implementation
            }


            //Vector3 bridge = HexMetrics.GetBridge(direction);
            //Vector3 v3 = v1 + bridge;
            //Vector3 v4 = v2 + bridge;

            //AddQuad(v1, v2, v3, v4);

            //HexCell prevNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
            //HexCell neighbor = cell.GetNeighbor(direction) ?? cell;
            //HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;

            //Color bridgeColor = (cell.GetCellColor() + neighbor.GetCellColor()) * 0.5f;
            //AddQuadColor(cell.GetCellColor(),bridgeColor);

            //AddTriangle(v1, center + HexMetrics.GetFirstCorner(direction), v3);
            //AddTriangleColor(
            //    cell.GetCellColor(),
            //    (cell.GetCellColor() + prevNeighbor.GetCellColor() + neighbor.GetCellColor()) / 3f,
            //    bridgeColor
            //);
            //AddTriangle(v2, v4, center + HexMetrics.GetSecondCorner(direction));
            //AddTriangleColor(
            //    cell.GetCellColor(),
            //    bridgeColor,
            //    (cell.GetCellColor() + neighbor.GetCellColor() + nextNeighbor.GetCellColor()) / 3f
            //);

        }

        void TriangulateConnection(
        HexDirection direction, HexCell cell, Vector3 v1, Vector3 v2
    )
        {
            HexCell neighbor = cell.GetNeighbor(direction);
            if (neighbor == null)
            {
                return;
            }
            Vector3 bridge = HexMetrics.GetBridge(direction);
            Vector3 v3 = v1 + bridge;
            Vector3 v4 = v2 + bridge;

            AddQuad(v1, v2, v3, v4);
            AddQuadColor(cell.GetCellColor(), neighbor.GetCellColor());

            HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
            if (direction <= HexDirection.E && nextNeighbor != null)
            {
                AddTriangle(v2, v4, v2 + HexMetrics.GetBridge(direction.Next()));
                AddTriangleColor(cell.GetCellColor(), neighbor.GetCellColor(), nextNeighbor.GetCellColor());
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
        void AddQuadColor(Color c1, Color c2)
        {
            colors.Add(c1);
            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c2);
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