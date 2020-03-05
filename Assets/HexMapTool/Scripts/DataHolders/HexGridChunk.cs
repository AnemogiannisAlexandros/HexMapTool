
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    [System.Serializable]
    public class HexGridChunk
    {
        private HexMesh hexMesh;
        [SerializeField]
        HexCell[] cells;
        [SerializeField]
        List<Vector3> meshVerts;
        [SerializeField]
        List<int> meshTriangles;
        [SerializeField]
        List<Color> meshColors;
        GameObject hexChunkObj;

        public GameObject GetChunk()
        {
            return hexChunkObj;
        }
        public HexCell[] GetCells()
        {
            return cells;
        }
        public HexMesh GetMesh()
        {
            return hexMesh;
        }
        public void SetMesh(HexMesh hexMesh) 
        {
            this.hexMesh = hexMesh;
        }
        public List<Vector3> GetVerts()
        {
            return meshVerts;
        }
        public void SetVerts(List<Vector3> meshVerts)
        {
            this.meshVerts = meshVerts;
        }
        public List<int> GetTriangles()
        {
            return meshTriangles;
        }
        public void SetTriangles(List<int> meshTriangles)
        {
            this.meshTriangles = meshTriangles;
        }
        public List<Color> GetColors()
        {
            return meshColors;
        }
        public void SetColors(List<Color> meshColors)
        {
            this.meshColors = meshColors;
        }
        public void SetCells(HexCell[] cells)
        {
            this.cells = cells;
        }
        public void Init()
        {
            hexChunkObj = new GameObject("HexChunk");
            hexMesh = hexChunkObj.AddComponent<HexMesh>();
            hexChunkObj.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
            cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
            ToolData.Instance.MeshDataObj.GetChunks().Add(this);
        }
        public void UpdateChunkData(HexCell[] cells, List<Vector3> meshVerts, List<int> meshTriangles, List<Color> meshColors)
        {
            this.cells = cells;
            this.meshVerts = meshVerts;
            this.meshTriangles = meshTriangles;
            this.meshColors = meshColors;
        }
        public void InitWithData(List<Vector3> meshVerts, List<int> meshTriangles, List<Color> meshColors) 
        {
            hexChunkObj = new GameObject("HexChunk");
            hexMesh = hexChunkObj.AddComponent<HexMesh>();
            hexChunkObj.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
            cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
            hexMesh.InitWithData(meshVerts, meshTriangles, meshColors);
            ToolData.Instance.MeshDataObj.GetChunks().Add(this);
        }
        public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
	    }

    }
}
