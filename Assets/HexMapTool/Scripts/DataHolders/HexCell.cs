﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTool
{
    public enum HexDirection
    {
        NE,
        E,
        SE,
        SW,
        W,
        NW
    }
    public static class HexDirectionExtensions
    {

        public static HexDirection Opposite(this HexDirection direction)
        {
            return (int)direction < 3 ? (direction + 3) : (direction - 3);
        }
        public static HexDirection Previous(this HexDirection direction)
        {
            return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
        }

        public static HexDirection Next(this HexDirection direction)
        {
            return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
        }
    }
    /// <summary>
    /// What data a "HexCell" must Hold for itself
    /// </summary>
    [System.Serializable]
    public class HexCell
    {
        [System.NonSerialized]
        HexCell[] neighbors;
        [SerializeField]
        private Vector3 worldCoordinates;
        [SerializeField]
        private HexCoordinates coordinates;
        [SerializeField]
        private Color cellColor;
        [SerializeField]
        private int elevation;
         
        //Constructor
        public HexCell()
        {

        }
        //Constructor
        public HexCell(Vector3 worldCoords,HexCoordinates hexCoords,Color cellCollor)
        {
            this.worldCoordinates = worldCoords;
            this.coordinates = hexCoords;
            this.cellColor = cellCollor;
            this.neighbors = new HexCell[6];
        }

        public HexEdgeType GetEdgeType(HexDirection direction)
        {
            return HexMetrics.GetEdgeType(
                elevation, neighbors[(int)direction].elevation
            );
        }

        public HexEdgeType GetEdgeType(HexCell otherCell)
        {
            return HexMetrics.GetEdgeType(
                elevation, otherCell.elevation
            );
        }

        public float GetElevation() 
        {
            return elevation;
        }
        public void SetElevation(int elevation) 
        {
            this.elevation = elevation;
            Vector3 position = GetWorldCoordinates();
            position.y = elevation * HexMetrics.elevationStep;
            position.y +=
                (HexMetrics.SampleNoise(position).y * 2f - 1f) *
                HexMetrics.elevationPerturbStrength;
            SetWorldCoordinates(position);
        }
        public HexCell[] getNeighbors()
        {
            return neighbors;
        }
        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }
        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        }
        public Vector3 GetWorldCoordinates()
        {
            return worldCoordinates;
        }
        public HexCoordinates GetCoordinates()
        {
            return coordinates;
        }
        public Color GetCellColor()
        {
            return cellColor;
        }
        public void SetWorldCoordinates(Vector3 coords)
        {
            worldCoordinates = coords;
        }
        public void SetCoordinates(HexCoordinates newCoords)
        {
            this.coordinates = newCoords;
        }
        public void SetColor(Color color)
        {
            this.cellColor = color;
        }
    }
}
