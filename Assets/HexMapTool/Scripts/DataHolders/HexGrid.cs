using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
namespace HexMapTool
{
    /// <summary>
    /// Our Hex Grid
    /// </summary>
    
        [CreateAssetMenu(fileName = "HexMap Data",menuName = "Hexmap/Data")]
    public class HexGrid : ScriptableObject
    {
        [SerializeField]
        private int width = 6;
        [SerializeField]
        private int height = 6;
        [SerializeField]
        private GameObject cellPrefab;
        [SerializeField]
        private GameObject cellLabelPrefab;
        [SerializeField]
        Canvas gridCanvas;

        GameObject hexGrid;
        HexCell[] cells;
        HexMesh hexMesh;

        public void Init() 
        {
            hexGrid = new GameObject("Hex Grid");
            hexMesh = hexGrid.AddComponent<HexMesh>();
        }

        public void CreateGrid()
        {

            cells = new HexCell[height * width];
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

        private void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.GetInnerRadius()) * 2f;
            position.y = 0f;
            position.z = z * (HexMetrics.GetOutterRadius()) * 1.5f;

            GameObject obj = Instantiate(cellPrefab);
            HexCell cell = obj.GetComponent<HexCell>();
            cells[i] = cell;
            cell.transform.SetParent(hexGrid.transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            Text label = Instantiate(cellLabelPrefab).GetComponent<Text>();
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLines();

           

        }
    }
}
