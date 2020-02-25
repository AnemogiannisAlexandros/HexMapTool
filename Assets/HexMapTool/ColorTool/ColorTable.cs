using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace HexMapTool
{
    public class ColorTable : ScriptableObject ,IColorTable
    {
        List<ColorArchetype> chosenColors = new List<ColorArchetype>();

        public ColorArchetype FindColorArchetype(ColorArchetype archetype)
        {
            foreach (ColorArchetype c in chosenColors)
            {
                if (c.GetArchetypeName() == archetype.GetArchetypeName())
                {
                    return c;
                }
            }
            return null;
        }

        public ColorArchetype GetColorArchetype(int index)
        {
            return chosenColors[index];
        }
        
    }

    public interface IColorTable
    {
        ColorArchetype GetColorArchetype(int index);
        ColorArchetype FindColorArchetype(ColorArchetype archetype);
    }
}
