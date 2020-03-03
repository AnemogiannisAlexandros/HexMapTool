using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HexMapTool
{
    [Serializable]
    public class MeshData : ScriptableObject
    {
        [SerializeField]
        private List<Vector3> vertices;
        [SerializeField]
        private List<int> triangles;
        [SerializeField]
        private List<Color> colors;

        public MeshData()
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        }
        public MeshData(List<Vector3> verts, List<int> tris, List<Color> col)
        {
            this.vertices = verts;
            this.triangles = tris;
            this.colors = col;
        }
        public List<Vector3> GetVertices()
        {
            return vertices;
        }
        public List<int> GetTriangles()
        {
            return triangles;
        }
        public List<Color> GetColors()
        {
            return colors;
        }
        public void Clear()
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        }
        public void UpdateMesh(List<Vector3> verts, List<int> tris, List<Color> col)
        {
            this.vertices = verts;
            this.triangles = tris;
            this.colors = col;
        }
    }
}
