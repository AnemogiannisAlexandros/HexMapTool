using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTool
{
    /// <summary>
    /// What data a "HexCell" must Hold for itself
    /// </summary>
    public class HexCell
    {
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
        }

        [SerializeField]
        private Vector3 worldCoordinates;
        [SerializeField]
        private HexCoordinates coordinates;
        [SerializeField]
        private Color cellColor;

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
