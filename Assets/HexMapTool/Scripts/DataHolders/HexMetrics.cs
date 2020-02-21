using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All Available Numerical values of a hexagon
/// </summary>
namespace HexMapTool
{

    public class HexMetrics : MonoBehaviour
    {
        //All Mathematical and Geometrical Data of a Hex
        private static float outRadious = 10f;
        private static float sideLength = outRadious;
        private static float perimeter = sideLength * 6;
        private static float apothem = Mathf.Sqrt(Mathf.Pow(outRadious, 2) - Mathf.Pow(sideLength / 2, 2));
        private static float area = apothem * perimeter / 2;
        private static float shortDiagonal = Mathf.Sqrt(3) * sideLength;
        private static float inRadius = apothem;
        private static float longDiagonal = sideLength *2;


        public const float solidFactor = 0.75f;

        public const float blendFactor = 1f - solidFactor;

        public static Vector3 GetFirstSolidCorner(HexDirection direction)
        {
            return corners[(int)direction] * solidFactor;
        }

        public static Vector3 GetSecondSolidCorner(HexDirection direction)
        {
            return corners[(int)direction + 1] * solidFactor;
        }

        public static float GetOutterRadius()
        {
            return outRadious;
        }
        public static float GetInnerRadius()
        {
            return inRadius;
        }
        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction];
        }

        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1];
        }
        //all verticies of the Hexagon defined by radious(inner and outer)
        public static Vector3[] verts =
        {
        new Vector3(0f, 0f, outRadious),
        new Vector3(inRadius, 0f, 0.5f * outRadious),
        new Vector3(inRadius, 0f, -0.5f * outRadious),
        new Vector3(0f, 0f, -outRadious),
        new Vector3(-inRadius, 0f, -0.5f * outRadious),
        new Vector3(-inRadius, 0f, 0.5f * outRadious),
        new Vector3(0f, 0f, outRadious)

        };
        //All Verticies of the Hexagon defined by radious(inner and outer)
        static Vector3[] corners = 
        {
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
