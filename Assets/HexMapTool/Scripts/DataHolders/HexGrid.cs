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
        private int cellCountX = 5;
        private int cellCountZ = 5;
        [SerializeField]
        private int seed = 1;
        [SerializeField]
        private int activeElevation = 0;
        private Color defaultColor;
        private Color touchedColor;
        private HexCell[] cells;
        private HexCell currentCell;
        private HexCell previousCell;
        private Texture2D noiseSource;

        private HexGridChunk[] chunks;

        public HexGrid() 
        {
            cellCountX = HexMetrics.chunkSizeX;
            cellCountZ = HexMetrics.chunkSizeZ;
            activeElevation = 1;
            defaultColor = Color.white;
        }

        private GameObject hexGrid;

        HexMesh hexMesh;

        public int GetSeed()
        {
            return seed;
        }
        public void SetSeed(int seed)
        {
            this.seed = seed;
        }
        public int GenerateSeed()
        {
            return Random.Range(1, 150);
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
        //void CreateRandomCells()
        //{
        //    cells = new HexCell[cellCountZ * cellCountX];

        //    for (int z = 0, i = 0; z < cellCountZ; z++)
        //    {
        //        for (int x = 0; x < cellCountX; x++)
        //        {
        //            CreateRandomCell(x, z, i++);
        //        }
        //    }
        //    ToolData.Instance.MeshDataObj.SetCells(cells);
        //}
        void CreateCells (bool isRandom,bool randomSeed = false)
        {
		    cells = new HexCell[cellCountZ * cellCountX];
            if (!isRandom)
            {
                for (int z = 0, i = 0; z < cellCountZ; z++)
                {
                    for (int x = 0; x < cellCountX; x++)
                    {
                        CreateCell(x, z, i++);
                    }
                }
            }
            else
            {
                int seedToUse;
                if (randomSeed)
                {
                    seedToUse = GenerateSeed();
                }
                else
                {
                    seedToUse = seed;
                }
                for (int z = 0, i = 0; z < cellCountZ; z++)
                {
                    for (int x = 0; x < cellCountX; x++)
                    {
                        CreateRandomCell(x, z, i++, seedToUse);
                    }
                }
            }

            ToolData.Instance.MeshDataObj.SetCells(cells);
        }
        //public void CreateGridWithChunksRandom(bool isRandom)
        //{
        //    ClearMap();
        //    if (hexGrid == null)
        //    {
        //        hexGrid = new GameObject("HexGrid");
        //        defaultColor = Color.white;
        //    }
        //    ToolData.Instance.MeshDataObj.SetChunks(new Vector2Int(chunkCountX, chunkCountZ));
        //    for (int i = 0; i < chunkCountX; i++)
        //    {
        //        for (int j = 0; j < chunkCountZ; j++)
        //        {
        //            //Debug.Log("Number of Chunks : " + ( j + (i) * chunkCountZ));
        //            chunks[j + (i) * chunkCountZ] = new HexGridChunk();
        //            chunks[j + (i) * chunkCountZ].Init();
        //        }
        //    }
        //    CreateCells(isRandom);
        //    int itterations = 0;
        //    foreach (HexGridChunk chunk in chunks)
        //    {
        //        chunk.GetChunk().transform.SetParent(hexGrid.transform);
        //        chunk.GetMesh().Init();
        //        for (int i = 0; i < HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ; i++)
        //        {
        //            chunk.AddCell(i, cells[i + itterations * HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ]);
        //        }
        //        chunk.GetMesh().Triangulate(chunk.GetCells(), itterations);
        //        itterations++;
        //    }
        //}
        public void CreateGridWithChunks(bool isRandom, bool randomSeed = false) 
        {
            ClearMap();
            Init();
            if (isRandom)
            {
                if (ToolData.Instance.Table.GetTableName() != "TerrainTemplate")
                {
                    ToolData.DirectLoad(ToolData.Instance.Table, "TerrainTemplate", ".dat");
                }
            }
            if (hexGrid == null) 
            {
                hexGrid = new GameObject("HexGrid");
                defaultColor = Color.white;
            }
            ToolData.Instance.MeshDataObj.SetChunks(new Vector2Int(chunkCountX, chunkCountZ));
            for (int i = 0; i < chunkCountX; i++) 
            {
                for (int j = 0; j < chunkCountZ; j++) 
                {
                    //Debug.Log("Number of Chunks : " + ( j + (i) * chunkCountZ));
                    chunks[j + (i) * chunkCountZ] = new HexGridChunk();
                    chunks[j + (i) * chunkCountZ].Init();
                }
            }
            CreateCells(isRandom, randomSeed);
            int itterations = 0;
            foreach (HexGridChunk chunk in chunks)
            {
                chunk.GetChunk().transform.SetParent(hexGrid.transform);
                chunk.GetMesh().Init();
                for (int i = 0; i < HexMetrics.chunkSizeX*HexMetrics.chunkSizeZ; i++) 
                {
                    chunk.AddCell(i, cells[i + itterations * HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ]);
                }
                chunk.GetMesh().Triangulate(chunk.GetCells(),itterations);
                itterations++;
            }
        }
        public void LoadGrid()
        {
            if (hexGrid == null)
            {
                hexGrid = new GameObject("Hex Grid");
                defaultColor = Color.white;
            }
            int loadedChunkX = ToolData.Instance.MeshDataObj.GetChunkSize().x;
            int loadedChunkZ = ToolData.Instance.MeshDataObj.GetChunkSize().y;
            for (int i = 0; i < loadedChunkX; i++)
            {
                for (int j = 0; j < loadedChunkZ; j++)
                {
                    //Debug.Log("Number of Chunks : " + (j + (i) * chunkCountZ));
                    chunks[j + (i) * chunkCountZ] = new HexGridChunk();
                }
            }
            cells = ToolData.Instance.MeshDataObj.GetCells();
            int itterations = 0;
            foreach (HexGridChunk chunk in chunks) 
            {
                chunk.InitWithData(ToolData.Instance.MeshDataObj.GetChunks()[itterations].GetVerts(),
                    ToolData.Instance.MeshDataObj.GetChunks()[itterations].GetTriangles(),
                    ToolData.Instance.MeshDataObj.GetChunks()[itterations].GetColors());
                chunk.GetChunk().transform.SetParent(hexGrid.transform);
                for (int i = 0; i < HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ; i++)
                {
                    chunk.AddCell(i, cells[i + itterations * HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ]);
                }
                chunk.GetMesh().TriangulateWithData(itterations);
                itterations++;
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
            //cells = new HexCell[cellCountX * cellCountZ];
            //coordinates = new Vector3[height * width];
            //CreateCells();
            //hexMesh.Triangulate(cells);
        }
       

        public void DestroyGrid()
        {
            DestroyImmediate(hexGrid);
            cells = new HexCell[0];
        }

        public void RefreshChunk(int index) 
        {
            chunks[index].GetMesh().Triangulate(chunks[index].GetCells(),index);
            ToolData.Instance.MeshDataObj.GetChunks()[index].SetMesh(chunks[index].GetMesh());
        }
        public void RefreshNeighbours(HexCell cell)
        {
            HexCell[] neighbors = cell.getNeighbors();
            foreach (HexCell c in neighbors) 
            {
                if (c != null)
                {
                    int cellIndex = c.GetCoordinates().X + c.GetCoordinates().Z * cellCountX + c.GetCoordinates().Z / 2;
                    cellIndex = Mathf.FloorToInt(cellIndex / (HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ));
                    RefreshChunk(cellIndex);
                }
            }
        }
        public HexCell GetCell(Vector3 position, HexCoordinates coords) 
        {
            int index = coords.X + coords.Z * cellCountX + coords.Z / 2;
            return cells[index];
        }
        public void EditCell(HexCell cell) 
        {
            int index = cell.GetCoordinates().X + cell.GetCoordinates().Z * cellCountX + cell.GetCoordinates().Z / 2;
            cell.SetColor(touchedColor);
            cell.SetElevation(activeElevation);
            index = Mathf.FloorToInt(index / (HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ));
            RefreshChunk(index);
            //Debug.Log("Editing Chunk " + index);
            RefreshNeighbours(cell);
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
            cell.SetElevation(0);
            cells[i] = cell;

        }
        private void CreateRandomCell(int x, int z, int i,int seedToUse)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.GetInnerRadius()) * 2f;
            position.z = z * (HexMetrics.GetOutterRadius()) * 1.5f;
            float h = Mathf.PerlinNoise(position.x/(x+1+ seedToUse), position.z/ (z + 1 + seedToUse)) *10;
            position.y = 0;
            // coordinates[i] = position;
            //GameObject obj = Instantiate(cellPrefab);
            HexCell cell = new HexCell(position, HexCoordinates.FromOffsetCoordinates(x, z), ToolData.Instance.Table.GetTable()[(int)h].GetArchetypeColor());

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
            cell.SetElevation((int)h);
            cells[i] = cell;

        }
        public void RestoreDefaults()
        {
            cellCountX = 5;
            cellCountZ = 5;
            activeElevation = 0;
            touchedColor = defaultColor;
            DestroyGrid();
        }

        public static int selectionIndex = 0;
        private static List<string> cardNames;
        private static bool exists = false;
        private static bool show = false;

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
                        EditorStyles.popup.fontSize = 10;
                        EditorStyles.popup.fixedHeight = 20;
                        selectionIndex = EditorGUILayout.Popup(selectionIndex, cardNames.ToArray(),GUILayout.Height(EditorStyles.popup.fixedHeight));

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
                chunkCountX = EditorGUILayout.IntField("Chunks x : " , chunkCountX);
                chunkCountZ = EditorGUILayout.IntField("Chunks z : " , chunkCountZ);
                EditorGUILayout.LabelField("Total Cells : " + (chunkCountX * HexMetrics.chunkSizeX * chunkCountZ * HexMetrics.chunkSizeZ));
                activeElevation = EditorGUILayout.IntField("Current Elevation y : ", activeElevation);
                seed = EditorGUILayout.IntField("Random Seed : ", seed);
                if (GUILayout.Button("Generate Map"))
                {
                    CreateGridWithChunks(false);
                }
                if (GUILayout.Button("Generate Random Map"))
                {
                    CreateGridWithChunks(true);
                }
                if (GUILayout.Button("Generate Random Map With Random Seed"))
                {
                    CreateGridWithChunks(true,true);
                }
                if (GUILayout.Button("Save map"))
                {
                    ToolData.Save(ToolData.Instance.MeshDataObj);
                }
                if (GUILayout.Button("Load Map"))
                {
                    if (hexGrid == null)
                    {
                        CreateGrid();
                    }
                    DestroyGrid();

                    ToolData.Load(ToolData.Instance.MeshDataObj);
                    LoadGrid();
                    
                    //ToolData.Instance.Grid.LoadGrid();
                }
                if (GUILayout.Button("Clear Map"))
                {
                    // Debug.Log("Deleting Map");
                    ClearMap();
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            //EditorUtility.SetDirty(this);
        }
        public void ClearMap()
        {
            DestroyGrid();
            ToolData.Instance.MeshDataObj.Clear();
        }
    }
}
