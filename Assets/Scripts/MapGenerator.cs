using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject hexPrefab;
    private int x,z;
    private Vector3 startPos = Vector3.zero;

    private void Start()
    {

       // CreateHexMap();
    }
    public void CreateHexMap() 
    {
        for (int i = 0; i < x; i++)
        {
            Instantiate(hexPrefab, startPos, Quaternion.identity);
            for (int j = 0; j < z; j++)
            {
                startPos += new Vector3(0, 0, 1);
                Instantiate(hexPrefab, startPos, Quaternion.identity);
            }
        }
    }
}
