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
        private int chunkCountX = 4;
        [SerializeField]
        private int chunkCountZ = 3;
        [SerializeField]
        private int cellCountX = 5;
        [SerializeField]
        private int cellCountZ = 5;
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
        private Texture2D noiseSource;
        private HexGridChunk[] chunks;

        public HexGrid() 
        {
            cellCountX = 5;
            cellCountZ = 5;
            activeElevation = 1;
            defaultColor = Color.white;
        }

        private GameObject hexGrid;

        HexMesh hexMesh;

        public HexMesh GetMesh()
        {
            return hexMesh;
        }
        public void SetCells(HexCell[] value) 
        {
            cells = value;
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
            return cellCountX;
        }
        private void OnEnable()
        {
            Init();
        }
        public void Init() 
        {
            noiseSource = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/noise.png", typeof(Texture2D));
            HexMetrics.noiseSource = noiseSource;
            cardNames = new List<string>();
            chunks = new HexGridChunk[chunkCountX * chunkCountZ];
            cellCountX = chunkCountX * HexMetrics.chunkSizeX;
            cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;
        }

        void CreateCells ()
        {
		    cells = new HexCell[cellCountZ * cellCountX];

		    for (int z = 0, i = 0; z < cellCountZ; z++)
            {
			    for (int x = 0; x < cellCountX; x++)
                {
				    CreateCell(x, z, i++);
			    }   
		    }
	    }

        //Create Hex Grid with given height and width
        public void CreateGrid()
        {

            if (hexGrid == null)
            {
                hexGrid = new GameObject("Hex Grid");
                hexMesh = hexGrid.AddComponent<HexMesh>();
                hexGrid.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
                defaultColor = Color.white;
            }
            hexMesh.Init();
            cells = new HexCell[cellCountX * cellCountZ];
            //coordinates = new Vector3[height * width];
            CreateCells();
            hexMesh.Triangulate(cells);
        }
        public void LoadGrid()
        {
            if (hexGrid == null)
            {
                hexGrid = new GameObject("Hex Grid");
                hexMesh = hexGrid.AddComponent<HexMesh>();
                hexGrid.GetComponent<MeshRenderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Materials/HexMaterial.mat", typeof(Material));
                defaultColor = Color.white;
            }
            hexMesh.TriangulateWithData();
        }

        public void DestroyGrid()
        {
            hexMesh.ClearMesh();
            cells = new HexCell[0];
        }
        public void TouchCell(Vector3 position, HexCoordinates coords)
        {
            int index = coords.X + coords.Z * cellCountX + coords.Z / 2;
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
            int index = coords.X + coords.Z * cellCountX + coords.Z / 2;
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
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                    if (x < cellCountX - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                    }
                }
            }
            cells[i] = cell;
            cell.SetElevation(0);
        }
        public void RestoreDefaults()
        {
            cellCountX = 5;
            cellCountZ = 5;
            activeElevation = 0;
            touchedColor = defaultColor;
            DestroyGrid();
            Refresh();
        }

        public static int selectionIndex = 0;
        private static List<string> cardNames;
        private static bool exists = false;
        private static bool show = false;
        private string MeshName;

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
                    if (selectionIndex < cardNames.Count)
                    {
                        selectionIndex = EditorGUILayout.Popup(selectionIndex, cardNames.ToArray());
                    }
                    else 
                    {
                        selectionIndex = cardNames.Count - 1;
                    }
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
                chunkCountX = EditorGUILayout.IntField("Chunk Size x : " + chunkCountX + "*" + HexMetrics.chunkSizeX, chunkCountX);
                chunkCountZ = EditorGUILayout.IntField("Chunk Size z : " + chunkCountZ + "*" + HexMetrics.chunkSizeZ, chunkCountZ);
                activeElevation = EditorGUILayout.IntField("Current Elevation y : ", activeElevation);
                if (GUILayout.Button("Generate Map"))
                {
                    Init();
                    // Debug.Log("Generating Map");
                    CreateGrid();
                }
                if (GUILayout.Button("Save map"))
                {
                    ToolData.Save(ToolData.Instance.MeshDataObj);
                }
                if (GUILayout.Button("Load Map"))
                {
                    ToolData.Load(ToolData.Instance.MeshDataObj);
                    ToolData.Instance.Grid.LoadGrid();
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
