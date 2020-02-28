﻿using UnityEngine;
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
        private Mesh hexMesh;
        private List<Vector3> vertices;
        private List<int> triangles;
        private List<Color> colors;
        private MeshCollider meshCollider;

        private MeshData meshData;
        //Create a mesh
        public void Init()
		{
            hexMesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = hexMesh;
            //hexMesh.name = "Hex Mesh";
            vertices = new List<Vector3>();
			triangles = new List<int>();
            colors =  new List<Color>();
            meshData = new MeshData();
        }
        public MeshData GetMeshData()
        {
            return meshData;
        }
        public void ClearMesh() 
        {
            meshData.Clear();
            hexMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();
            if (meshCollider != null) 
            {
                meshCollider.sharedMesh.Clear();
            }
        }
        //Clear any mesh Data
        //Calculate Mesh from cell Data
        //assing collider
		public void Triangulate(HexCell[] coords)
		{
            ClearMesh();
            if (coords != null)
            {

                for (int i = 0; i < coords.Length; i++)
                {
                    Triangulate(coords[i]);
                }
            }
            meshData.UpdateMesh(vertices, triangles, colors);
            hexMesh.vertices = meshData.GetVertices().ToArray();
            hexMesh.triangles = meshData.GetTriangles().ToArray();
            hexMesh.colors = meshData.GetColors().ToArray();
            //Debug.Log(hexMesh.vertices.Length);
            // hexMesh.RecalculateBounds();
            //hexMesh.RecalculateNormals();
            if (meshCollider == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
            }
            meshCollider.sharedMesh = hexMesh;
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
            v3.y = v4.y = neighbor.GetElevation() * HexMetrics.elevationStep;

            if (cell.GetEdgeType(direction) == HexEdgeType.Slope)
            {
                TriangulateEdgeTerraces(v1, v2, cell, v3, v4, neighbor);
            }
            else
            {
                AddQuad(v1, v2, v3, v4);
                AddQuadColor(cell.GetCellColor(), neighbor.GetCellColor());
            }

            HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
            if (direction <= HexDirection.E && nextNeighbor != null)
            {
                Vector3 v5 = v2 + HexMetrics.GetBridge(direction.Next());
                v5.y = nextNeighbor.GetElevation() * HexMetrics.elevationStep;

                if (cell.GetElevation() <= neighbor.GetElevation())
                {
                    if (cell.GetElevation() <= nextNeighbor.GetElevation())
                    {
                        TriangulateCorner(v2, cell, v4, neighbor, v5, nextNeighbor);
                    }
                    else
                    {
                        TriangulateCorner(v5, nextNeighbor, v2, cell, v4, neighbor);
                    }
                }
                else if (neighbor.GetElevation() <= nextNeighbor.GetElevation())
                {
                    TriangulateCorner(v4, neighbor, v5, nextNeighbor, v2, cell);
                }
                else
                {
                    TriangulateCorner(v5, nextNeighbor, v2, cell, v4, neighbor);
                }
            }

        }

        void TriangulateCorner(
        Vector3 bottom, HexCell bottomCell,
        Vector3 left, HexCell leftCell,
        Vector3 right, HexCell rightCell
    )
        {
            HexEdgeType leftEdgeType = bottomCell.GetEdgeType(leftCell);
            HexEdgeType rightEdgeType = bottomCell.GetEdgeType(rightCell);

            if (leftEdgeType == HexEdgeType.Slope)
            {
                if (rightEdgeType == HexEdgeType.Slope)
                {
                    TriangulateCornerTerraces(
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                }
                else if (rightEdgeType == HexEdgeType.Flat)
                {
                    TriangulateCornerTerraces(
                        left, leftCell, right, rightCell, bottom, bottomCell
                    );
                }
                else
                {
                    TriangulateCornerTerracesCliff(
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                }
            }
            else if (rightEdgeType == HexEdgeType.Slope)
            {
                if (leftEdgeType == HexEdgeType.Flat)
                {
                    TriangulateCornerTerraces(
                        right, rightCell, bottom, bottomCell, left, leftCell
                    );
                }
                else
                {
                    TriangulateCornerCliffTerraces(
                        bottom, bottomCell, left, leftCell, right, rightCell
                    );
                }
            }
            else if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
            {
                if (leftCell.GetElevation() < rightCell.GetElevation())
                {
                    TriangulateCornerCliffTerraces(
                        right, rightCell, bottom, bottomCell, left, leftCell
                    );
                }
                else
                {
                    TriangulateCornerTerracesCliff(
                        left, leftCell, right, rightCell, bottom, bottomCell
                    );
                }
            }
            else
            {
                AddTriangle(bottom, left, right);
                AddTriangleColor(bottomCell.GetCellColor(), leftCell.GetCellColor(), rightCell.GetCellColor());
            }
        }
        void TriangulateCornerCliffTerraces(
        Vector3 begin, HexCell beginCell,
        Vector3 left, HexCell leftCell,
        Vector3 right, HexCell rightCell
    )
        {
            float b = 1f / (leftCell.GetElevation() - beginCell.GetElevation());
            if (b < 0)
            {
                b = -b;
            }
            Vector3 boundary = Vector3.Lerp(begin, left, b);
            Color boundaryColor = Color.Lerp(beginCell.GetCellColor(), leftCell.GetCellColor(), b);

            TriangulateBoundaryTriangle(
                right, rightCell, begin, beginCell, boundary, boundaryColor
            );

            if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
            {
                TriangulateBoundaryTriangle(
                    left, leftCell, right, rightCell, boundary, boundaryColor
                );
            }
            else
            {
                AddTriangle(left, right, boundary);
                AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
            }
        }
        void TriangulateCornerTerracesCliff(
        Vector3 begin, HexCell beginCell,
        Vector3 left, HexCell leftCell,
        Vector3 right, HexCell rightCell
    )
        {
            float b = 1f / (rightCell.GetElevation() - beginCell.GetElevation());
            if (b < 0)
            {
                b = -b;
            }
            Vector3 boundary = Vector3.Lerp(begin, right, b);
            Color boundaryColor = Color.Lerp(beginCell.GetCellColor(), rightCell.GetCellColor(), b);

            TriangulateBoundaryTriangle(
            begin, beginCell, left, leftCell, boundary, boundaryColor
        );
            if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
            {
                TriangulateBoundaryTriangle(
                    left, leftCell, right, rightCell, boundary, boundaryColor
                );
            }
            else
            {
                AddTriangle(left, right, boundary);
                AddTriangleColor(leftCell.GetCellColor(), rightCell.GetCellColor(), boundaryColor);
            }
        }

        void TriangulateBoundaryTriangle(
        Vector3 begin, HexCell beginCell,
        Vector3 left, HexCell leftCell,
        Vector3 boundary, Color boundaryColor
    )
        {
            Vector3 v2 = HexMetrics.TerraceLerp(begin, left, 1);
            Color c2 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);

            AddTriangle(begin, v2, boundary);
            AddTriangleColor(beginCell.GetCellColor(), c2, boundaryColor);

            for (int i = 2; i < HexMetrics.terraceSteps; i++)
            {
                Vector3 v1 = v2;
                Color c1 = c2;
                v2 = HexMetrics.TerraceLerp(begin, left, i);
                c2 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                AddTriangle(v1, v2, boundary);
                AddTriangleColor(c1, c2, boundaryColor);
            }

            AddTriangle(v2, left, boundary);
            AddTriangleColor(c2, leftCell.GetCellColor(), boundaryColor);
        }

        void TriangulateCornerTerraces(
        Vector3 begin, HexCell beginCell,
        Vector3 left, HexCell leftCell,
        Vector3 right, HexCell rightCell
    )
        {
            Vector3 v3 = HexMetrics.TerraceLerp(begin, left, 1);
            Vector3 v4 = HexMetrics.TerraceLerp(begin, right, 1);
            Color c3 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), 1);
            Color c4 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), rightCell.GetCellColor(), 1);

            AddTriangle(begin, v3, v4);
            AddTriangleColor(beginCell.GetCellColor(), c3, c4);

            for (int i = 2; i < HexMetrics.terraceSteps; i++)
            {
                Vector3 v1 = v3;
                Vector3 v2 = v4;
                Color c1 = c3;
                Color c2 = c4;
                v3 = HexMetrics.TerraceLerp(begin, left, i);
                v4 = HexMetrics.TerraceLerp(begin, right, i);
                c3 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), leftCell.GetCellColor(), i);
                c4 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), rightCell.GetCellColor(), i);
                AddQuad(v1, v2, v3, v4);
                AddQuadColor(c1, c2, c3, c4);
            }

            AddQuad(v3, v4, left, right);
            AddQuadColor(c3, c4, leftCell.GetCellColor(), rightCell.GetCellColor());
        }

        void TriangulateEdgeTerraces(
        Vector3 beginLeft, Vector3 beginRight, HexCell beginCell,
        Vector3 endLeft, Vector3 endRight, HexCell endCell
    )
        {
            Vector3 v3 = HexMetrics.TerraceLerp(beginLeft, endLeft, 1);
            Vector3 v4 = HexMetrics.TerraceLerp(beginRight, endRight, 1);
            Color c2 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), endCell.GetCellColor(), 1);

            AddQuad(beginLeft, beginRight, v3, v4);
            AddQuadColor(beginCell.GetCellColor(), c2);

            for (int i = 2; i < HexMetrics.terraceSteps; i++)
            {
                Vector3 v1 = v3;
                Vector3 v2 = v4;
                Color c1 = c2;
                v3 = HexMetrics.TerraceLerp(beginLeft, endLeft, i);
                v4 = HexMetrics.TerraceLerp(beginRight, endRight, i);
                c2 = HexMetrics.TerraceLerp(beginCell.GetCellColor(), endCell.GetCellColor(), i);
                AddQuad(v1, v2, v3, v4);
                AddQuadColor(c1, c2);
            }

            AddQuad(v3, v4, endLeft, endRight);
            AddQuadColor(c2, endCell.GetCellColor());
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