using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTool
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField]
        private HexCoordinates coordinates;

        public HexCoordinates GetCoordinates()
        {
            return coordinates;
        }
        public void SetCoordinates(HexCoordinates newCoords)
        {
            this.coordinates = newCoords;
        }
    }
}
