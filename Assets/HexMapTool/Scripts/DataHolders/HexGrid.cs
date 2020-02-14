using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

namespace HexMapTool
{
    /// <summary>
    /// Our Hex Grid
    /// </summary>
    
        [CreateAssetMenu(fileName = "HexMap Data",menuName = "Hexmap/Data")]
    public class HexGrid : ScriptableObject
    {
        [SerializeField]
        private int width = 1;
        [SerializeField]
        private int height = 1;
        [SerializeField]
        private Color defaultColor;
        [SerializeField]
        private Color touchedColor;
        //[SerializeField]
        //private GameObject cellPrefab;
        //[SerializeField]
        //private GameObject cellLabelPrefab;
        //[SerializeField]
        //private Canvas gridCanvas;

        //Canvas canvas;
        //GameObject hexCellHolder;
        GameObject hexGrid;
        HexCell[] cells;
        HexMesh hexMesh;
        //public Vector3[] coordinates;

        public HexMesh GetMesh()
        {
            return hexMesh;
        }
        public HexCell[] GetCells()
        {
            return cells;
        }
        public Color GetDefaultColor()
        {
            return defaultColor;
        }
        public Color GetTouchedColor()
        {
            return touchedColor;
        }
        public int GetWidth()
        {
            return width;
        }
        public GameObject Init() 
        {
            hexGrid = new GameObject("Hex Grid");
            hexMesh = hexGrid.AddComponent<HexMesh>();
            hexGrid.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat",typeof(Material));
            hexMesh.Init();
           // canvas = Instantiate(gridCanvas,hexGrid.transform);
           // canvas.transform.position += new Vector3(0, 0.1f, 0);
           // hexCellHolder = new GameObject("Hex Cells");
           // hexCellHolder.transform.SetParent(hexGrid.transform);
            return hexGrid;
        }

        //Create Hex Grid with given height and width
        public void CreateGrid()
        {

            cells = new HexCell[height * width];
            //coordinates = new Vector3[height * width];
            int i = 0;
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
            hexMesh.Triangulate(cells);

        }

        public void DestroyGrid()
        {
            //for (int i = canvas.transform.childCount-1; i >= 0; i--)
            //{
            //    DestroyImmediate(canvas.transform.GetChild(i).gameObject);
            //}
            //for (int i = hexCellHolder.transform.childCount-1; i >= 0; i--)
            //{
            //    DestroyImmediate(hexCellHolder.transform.GetChild(i).gameObject);
            //}
            hexMesh.GetComponent<MeshFilter>().sharedMesh.Clear();
        }
        public void TouchCell(Vector3 position, HexCoordinates coords)
        {
            int index = coords.X + coords.Z * width + coords.Z / 2;
            HexCell cell = cells[index];
            cell.SetColor(touchedColor);
            hexMesh.Triangulate(cells);
        }
        private void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.GetInnerRadius()) * 2f;
            position.z = z * (HexMetrics.GetOutterRadius()) * 1.5f;
            //float h = Mathf.PerlinNoise(position.x, position.z);
            position.y = 0;
            // coordinates[i] = position;
            //GameObject obj = Instantiate(cellPrefab);
            cells[i] = new HexCell(position, HexCoordinates.FromOffsetCoordinates(x, z), defaultColor);
            //cell.transform.SetParent(hexCellHolder.transform, false);
            //cell.transform.localPosition = position;
            //cell.SetWorldCoordinates(position);
            //cell.SetCoordinates(HexCoordinates.FromOffsetCoordinates(x, z));
            //cell.SetColor(defaultColor);



            //Text label = Instantiate(cellLabelPrefab).GetComponent<Text>();
            //label.rectTransform.SetParent(canvas.transform, false);
            //label.rectTransform.anchoredPosition =
            //new Vector2(position.x, position.z);
            //label.text = cell.GetCoordinates().ToStringOnSeparateLines();



        }
        public void OnGui()
        {
            width = EditorGUILayout.IntField(width);
            height = EditorGUILayout.IntField(height);
            defaultColor = EditorGUILayout.ColorField(defaultColor);
            touchedColor = EditorGUILayout.ColorField(touchedColor);
        }

    }
}
