using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTool
{

    public class HexMetrics : MonoBehaviour
    {
        private static float outRadious = 10f;
        private static float sideLength = outRadious;
        private static float perimeter = sideLength * 6;
        private static float apothem = Mathf.Sqrt(Mathf.Pow(outRadious, 2) - Mathf.Pow(sideLength / 2, 2));
        private static float area = apothem * perimeter / 2;
        private static float shortDiagonal = Mathf.Sqrt(3) * sideLength;
        private static float inRadius = apothem;
        private static float longDiagonal = sideLength *2;

        public static float GetOutterRadius()
        {
            return outRadious;
        }
        public static float GetInnerRadius()
        {
            return inRadius;
        }
        public static Vector3[] verts = {
        new Vector3(0f, 0f, outRadious),
        new Vector3(inRadius, 0f, 0.5f * outRadious),
        new Vector3(inRadius, 0f, -0.5f * outRadious),
        new Vector3(0f, 0f, -outRadious),
        new Vector3(-inRadius, 0f, -0.5f * outRadious),
        new Vector3(-inRadius, 0f, 0.5f * outRadious),
        new Vector3(0f, 0f, outRadious)
    };
    }
}
