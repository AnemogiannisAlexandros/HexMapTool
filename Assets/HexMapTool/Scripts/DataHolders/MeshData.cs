using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTool
{
    [Serializable]
    public class MeshData : ScriptableObject
    {
        [SerializeField]
        List<HexGridChunk> chunks;
        [SerializeField]
        private HexCell[] cells;
        [SerializeField]
        private int chunkSizeX, chunkSizeZ;

        public MeshData()
        {
            this.chunks = new List<HexGridChunk>();
        }
        public MeshData(List<HexGridChunk> chunks)
        {
            this.chunks = chunks;
        }
        public MeshData(List<HexGridChunk> chunks, HexCell[] cells, int chunkSizeX, int chunkSizeZ)
        {
            this.chunks = chunks;
            this.cells = cells;
            this.chunkSizeX = chunkSizeX;
            this.chunkSizeZ = chunkSizeZ;
        }
        public List<HexGridChunk> GetChunks()
        {
            return this.chunks;
        }
        public HexCell[] GetCells() 
        {
            return cells;
        }
        public Vector2Int GetChunkSize() 
        {
            return new Vector2Int(chunkSizeX, chunkSizeZ);
        }
        public void SetCells(HexCell[] cells) 
        {
            this.cells = cells;
        }
        public void SetChunks(List<HexGridChunk> chunks) 
        {
            this.chunks = chunks;
        }
        public void SetChunks(Vector2Int chunkSize) 
        {
            this.chunkSizeX = chunkSize.x;
            this.chunkSizeZ = chunkSize.y;
        }
        public void Clear()
        {
            chunks = new List<HexGridChunk>();
            cells = new HexCell[0];
            chunkSizeX = 0;
            chunkSizeZ = 0;
        }
    }
}
