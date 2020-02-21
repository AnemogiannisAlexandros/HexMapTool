using System;
using UnityEngine;
//[System.Serializable]
//public struct HexVectorPosition
//{
//    [SerializeField]
//    private float x, y, z;

//    public float X { get { return x; } set { } }

//    public float Z { get { return z; } set { } }

//    public float Y { get { return y; } set { } }
//    public HexVectorPosition(float x, float y, float z)
//    {
//        this.x = x;
//        this.y = y;
//        this.z = z;
//    }
//}
namespace HexMapTool
{
    //Will Hold HexMesh Coordinate in the Hex Coordinate System
    //More Can be found at https://www.redblobgames.com/grids/hexagons/

    [System.Serializable]
    public struct HexCoordinates
    {

        [SerializeField]
        private int x, z;

        public int X { get { return x; } }

        public int Z { get { return z; } }

        public int Y { get { return -X - Z; } }

        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
        public static HexCoordinates FromOffsetCoordinates(int x, int z)
        {
            return new HexCoordinates(x - z / 2, z);
        }
        public override string ToString()
        {
            return "(" +
                X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }

        public string ToStringOnSeparateLines()
        {
            return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
        }

        //Get HexCoordinates from world position;
        public static HexCoordinates FromPosition(Vector3 worldPosition)
        {
            float x = worldPosition.x / (HexMetrics.GetInnerRadius() * 2f);
            float y = -x;
            float offset = worldPosition.z / (HexMetrics.GetOutterRadius() * 3f);
            x -= offset;
            y -= offset;
            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);
            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY)
                {
                    iZ = -iX - iY;
                }
                Debug.LogWarning("rounding error!");
            }
            return new HexCoordinates(iX, iZ);
        }
    }
}