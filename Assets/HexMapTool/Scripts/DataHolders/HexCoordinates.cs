﻿using UnityEngine;
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
}