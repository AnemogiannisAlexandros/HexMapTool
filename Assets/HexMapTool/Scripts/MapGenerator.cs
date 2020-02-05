using UnityEngine;
using UnityEditor;
public class MapGenerator : ScriptableObject
{
    public GameObject hexPrefab { get; set; }
    public Vector2 size { get; set; }
    public Vector3 startPos { get; set; }

    private Transform mapParent;

    public void Init() 
    {
        hexPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/HexMapTool/Prefabs/Hex.prefab", typeof(GameObject));
        size = new Vector2(1, 1);
        startPos = Vector3.zero;
    }

    public void CreateHexMap()
    {
        if (mapParent == null)
        {
            GameObject obj = new GameObject("Hex Map");
            mapParent = obj.transform;

        }
        Vector3 currentPos = startPos;
        for (int i = 1; i <= size.x; i++)
        {
            for (int j = 1; j <= size.y; j++)
            {
                Instantiate(hexPrefab, currentPos, Quaternion.identity, mapParent);
                currentPos += new Vector3(0, 0, hexPrefab.transform.localScale.z * 2);
            }
            if (i % 2 == 0)
            {
                currentPos = new Vector3(1.5f * i, 0, 0);
            }
            else
            {
                currentPos = new Vector3(1.5f * i, 0, 1);
            }
        }
    }
    public void ClearMap() 
    {
        for (int i = mapParent.childCount-1; i >= 0; i--)
        {
            DestroyImmediate(mapParent.GetChild(i).gameObject);
        }
    }
}
