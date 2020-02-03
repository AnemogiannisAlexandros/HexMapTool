using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HexAttributes : MonoBehaviour
{
    private float apothem, xSize, ySize, zSize, sideLength, area, perimeter,longDiagonal,shortDiagonal,outRadious,inRadious;

    public void OnEnable()
    {
        hideFlags = HideFlags.HideInInspector;
        //Default instantiation scale of 1,1,1
        Vector3 scale = GetComponent<Transform>().localScale;
        xSize = scale.x;
        ySize = scale.y;
        zSize = scale.z;

        //side of hexagon is xSize/2 by definition
        sideLength = xSize / 2;
        //OuterRadious
        outRadious = sideLength;
        //perimeter of our hexagon
        perimeter = sideLength * 6;
        //apothem given by knowing the sideLength
        apothem = Mathf.Sqrt(Mathf.Pow(outRadious, 2) - Mathf.Pow(sideLength / 2, 2));
        //Get Area by apothem and perimeter
        area = apothem * perimeter / 2;
        // Diagonal d 
        longDiagonal = sideLength * 2;
        //short diagonal s = root3 * sideLength
        shortDiagonal = Mathf.Sqrt(3) * sideLength;

        //InnerRadious
        //inRadious = Mathf.Sqrt(3 / 2) * sideLength; Or equals to apothem
        inRadious = apothem;

        Debug.LogFormat("{0} ,{1} , {2} , {3} , {4} , {5} , {6}",sideLength,perimeter,area,longDiagonal,shortDiagonal,outRadious,inRadious);
    }
}
