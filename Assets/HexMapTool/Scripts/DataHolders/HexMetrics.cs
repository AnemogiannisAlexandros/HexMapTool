using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All Available Numerical values of a hexagon
/// </summary>
namespace HexMapTool
{
    public enum HexEdgeType
    {
        Flat, Slope, Cliff
    }
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
        private static float longDiagonal = sideLength * 2;

        public const float elevationStep = 5f;

        public const float solidFactor = 0.75f;

        public const float blendFactor = 1f - solidFactor;

        public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

        public const int terracesPerSlope = 2;

        public const int terraceSteps = terracesPerSlope * 2 + 1;

        public const float horizontalTerraceStepSize = 1f / terraceSteps;

        public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
        {
            if (elevation1 == elevation2)
            {
                return HexEdgeType.Flat;
            }
            int delta = elevation2 - elevation1;
            if (delta == 1 || delta == -1)
            {
                return HexEdgeType.Slope;
            }
            return HexEdgeType.Cliff;
        }

        public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
        {
            float h = step * HexMetrics.horizontalTerraceStepSize;
            a.x += (b.x - a.x) * h;
            a.z += (b.z - a.z) * h;
            float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
            a.y += (b.y - a.y) * v;
            return a;
        }

        public static Color TerraceLerp(Color a, Color b, int step)
        {
            float h = step * HexMetrics.horizontalTerraceStepSize;
            return Color.Lerp(a, b, h);
        }

        public static Vector3 GetBridge(HexDirection direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
        }

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
