using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
namespace HexMapTool
{
    public class HexGrid : ScriptableObject
    {
        [SerializeField]
        private int width = 6;
        [SerializeField]
        private int height = 6;
        private HexCell cellPrefab;
        private GameObject cellLabelPrefab;

        GameObject hexGrid;
        Canvas gridCanvas;
        HexCell[] cells;
        HexMesh hexMesh;

        public void Init() 
        {
            hexGrid = new GameObject("Hex Grid");
            GameObject cell = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/HexCell.prefab", typeof(GameObject));
            cellPrefab = cell.GetComponent<HexCell>();
            Debug.Log(cellPrefab.name);
            GameObject Canvas = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/Hex Canvas.prefab", typeof(GameObject));
            Debug.Log(Canvas.name);
            gridCanvas = Canvas.GetComponent<Canvas>();
            gridCanvas.transform.SetParent(hexGrid.transform);
            hexMesh = new GameObject("Hex Mesh").AddComponent<HexMesh>();
            hexMesh.transform.SetParent(hexGrid.transform);
            cellLabelPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/Hex Canvas.prefab", typeof(GameObject));
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

            HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
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
