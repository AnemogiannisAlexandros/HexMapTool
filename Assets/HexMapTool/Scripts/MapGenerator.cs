using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject hexPrefab;
    [SerializeField]
    private int x,z;
    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        Debug.Log("Creating Map");
        CreateHexMap();
    }
    public void CreateHexMap() 
    {
        for (int i = 0; i < z; i++)
        {
            Instantiate(hexPrefab, startPos, Quaternion.identity);
            startPos += new Vector3(0, 0, 1);
            for (int j = 0; j < x; j++)
            {
                
                Instantiate(hexPrefab, startPos, Quaternion.identity);
            }
            
        }
    }
}
