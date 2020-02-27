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
    
    public class HexGrid : ScriptableObject
    {
        [SerializeField]
        private int width = 1;
        [SerializeField]
        private int height = 1;
        [SerializeField]
        private int activeElevation = 0;
        [SerializeField]
        private Color defaultColor;
        [SerializeField]
        private Color touchedColor;
        [SerializeField]
        private HexCell[] cells;
        private HexCell currentCell;
        private HexCell previousCell;

        public HexGrid() 
        {
            width = 5;
            height = 5;
            activeElevation = 1;
            defaultColor = Color.white;
            Init();
        }

        private GameObject hexGrid;

        HexMesh hexMesh;

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
        public void SetTouchedColor(Color color)
        {
            touchedColor = color;
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
            if (hexGrid == null)
            {
                hexGrid = new GameObject("Hex Grid");
                hexMesh = hexGrid.AddComponent<HexMesh>();
                hexGrid.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
                defaultColor = Color.white;
                texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/ColorTool/ColorBackground.jpg", typeof(Texture2D));
            }
            cardNames = new List<string>();
            return hexGrid;
        }

        //Create Hex Grid with given height and width
        public void CreateGrid()
        {
            hexMesh.Init();
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
            hexMesh.GetComponent<MeshFilter>().sharedMesh.Clear();
            hexMesh.GetComponent<MeshCollider>().sharedMesh.Clear();
            cells = new HexCell[0];
        }
        public void TouchCell(Vector3 position, HexCoordinates coords)
        {
            int index = coords.X + coords.Z * width + coords.Z / 2;
            HexCell cell = cells[index];
            cell.SetColor(touchedColor);
            hexMesh.Triangulate(cells);
        }
        public void Refresh() 
        {
            hexMesh.Triangulate(cells);
        }
        public HexCell GetCell(Vector3 position, HexCoordinates coords) 
        {
            int index = coords.X + coords.Z * width + coords.Z / 2;
            return cells[index];
        }
        public void EditCell(HexCell cell) 
        {
            cell.SetColor(touchedColor);
            cell.SetElevation(activeElevation);
            Refresh();
        }
        private void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.GetInnerRadius()) * 2f;
            position.z = z * (HexMetrics.GetOutterRadius()) * 1.5f;
           // float h = Mathf.PerlinNoise(position.x*5, position.z*5);
            position.y = 0;
            // coordinates[i] = position;
            //GameObject obj = Instantiate(cellPrefab);
            HexCell cell = new HexCell(position, HexCoordinates.FromOffsetCoordinates(x, z), defaultColor);

            //Avoid Setting Neighbor on the first leftmost HexCell
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                    if (x < width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                    }
                }
            }
            cells[i] = cell;
        }
        public void RestoreDefaults()
        {
            width = 5;
            height = 5;
            activeElevation = 0;
            touchedColor = defaultColor;
            Refresh();
        }

        public static int selectionIndex = 0;
        private static List<string> cardNames;
        private static bool exists = false;
        private static bool show = false;
        private Texture2D texture;

        //Internal Implementation of EdiotrWinodw onGUI.
        public void OnGui()
        {
           
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginVertical();
            show = EditorGUILayout.Foldout(show, "Hex Map Generator", true);

            exists = ToolData.Instance.Table.GetTable().Count > 0 ? true : false;

            //grid = (HexGrid)EditorGUILayout.ObjectField(grid, typeof(HexGrid), true);
            if (show)
            {
                if (exists)
                {
                    cardNames.Clear();
                    foreach (ColorArchetype c in ToolData.Instance.Table.GetTable())
                    {
                        cardNames.Add(c.GetArchetypeName());
                    }
                    selectionIndex = EditorGUILayout.Popup(selectionIndex, cardNames.ToArray());
                    touchedColor = ToolData.Instance.Table.GetTable()[selectionIndex].GetArchetypeColor();
                    GUILayout.Label("Current Selected Color Preview : ");
                    Rect rect = EditorGUILayout.GetControlRect(false, 15);
                    rect.height = 15;
                    EditorGUI.DrawRect(rect, touchedColor);
                    //Color[] colors = texture.GetPixels();
                    //for (int x = 0; x < colors.Length; x++)
                    //{
                    //    colors[x] = ToolData.Instance.Table.GetTable()[selectionIndex].GetArchetypeColor();
                    //}
                    //texture.SetPixels(colors);
                    //texture.Apply();
                    //GUILayout.Label(texture, GUILayout.Width(1000));

                }
                width = EditorGUILayout.IntField("Width x : ", width);
                height = EditorGUILayout.IntField("Length z : ", height);
                activeElevation = EditorGUILayout.IntField("Current Elevation y : ", activeElevation);
                if (GUILayout.Button("Generate Map"))
                {
                    Init();
                    // Debug.Log("Generating Map");
                    CreateGrid();
                }
                if (GUILayout.Button("Clear Map"))
                {
                    // Debug.Log("Deleting Map");
                    DestroyGrid();
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            //EditorUtility.SetDirty(this);
        }

    }
}
