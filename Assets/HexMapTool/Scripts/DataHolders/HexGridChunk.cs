﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{

    public class HexGridChunk
    {
        HexCell[] cells;
        HexMesh hexMesh;
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
        public void Init()
        {
            hexChunkObj = new GameObject("HexChunk");
            hexMesh = hexChunkObj.AddComponent<HexMesh>();
            hexChunkObj.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
            cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
        }
        public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
	    }
    }
}
