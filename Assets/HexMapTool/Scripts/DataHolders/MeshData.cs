using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTool
{
    [Serializable]
    public class MeshData : ScriptableObject
    {
        [SerializeField]
        List<HexMesh> chunkMeshes;
        [SerializeField]
        private HexCell[] cells;
        [SerializeField]
        private int chunkSizeX, chunkSizeZ;

        public MeshData()
        {
            this.chunkMeshes = new List<HexMesh>();
        }
        public MeshData(List<HexMesh> chunkMeshes)
        {
            this.chunkMeshes = chunkMeshes;
        }
        public MeshData(List<HexMesh> chunkMeshes, HexCell[] cells, int chunkSizeX, int chunkSizeZ)
        {
            this.chunkMeshes = chunkMeshes;
            this.cells = cells;
            this.chunkSizeX = chunkSizeX;
            this.chunkSizeZ = chunkSizeZ;
        }
        public List<HexMesh> GetChunkMeshes()
        {
            return this.chunkMeshes;
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
        public void SetChunkMeshes(List<HexMesh> chunkMeshes) 
        {
            this.chunkMeshes = chunkMeshes;
        }
        public void SetChunks(Vector2Int chunkSize) 
        {
            this.chunkSizeX = chunkSize.x;
            this.chunkSizeZ = chunkSize.y;
        }
        public void Clear()
        {
            chunkMeshes = new List<HexMesh>();
            cells = new HexCell[0];
            chunkSizeX = 0;
            chunkSizeZ = 0;
        }
    }
}
