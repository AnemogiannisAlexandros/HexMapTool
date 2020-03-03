using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

namespace HexMapTool
{
    public interface IColorArchetype
    {
        string GetArchetypeName();
        Color GetArchetypeColor();
        ColorBehaviour GetColorBehaviour();
        ColorBehaviourData GetColorBehaviourData();

        void SetArchetypeName(string name);
        void SetArchetypeColor(Color color);
        void SetColorBehaviour(ColorBehaviour colorBehaviour);
        void SetColorBehaviourData(ColorBehaviourData colorBehaviourData);
    }
    [Serializable]
    public class ColorArchetype : IColorArchetype
    {
        #region Data
        [SerializeField]
        private string archetypeName;
        [SerializeField]
        private Color color;
        [SerializeField]
        private ColorBehaviour colorJob;
        [SerializeField]
        private ColorBehaviourData jobData;
        #endregion

        #region Constructors
        public ColorArchetype()
        {
            archetypeName = "SomeName";
            color = Color.white;
        }
        public ColorArchetype(string colorName,Color col) 
        {
            archetypeName = colorName;
            color = col;
        }
        #endregion
        #region Getters_Setters
        public Color GetArchetypeColor()
        {
            return color;
        }

        public string GetArchetypeName()
        {
            return archetypeName;
        }

        public ColorBehaviour GetColorBehaviour()
        {
            return colorJob;
        }

        public ColorBehaviourData GetColorBehaviourData()
        {
            return jobData;
        }

        public void SetArchetypeColor(Color color)
        {
            this.color = color;
        }

        public void SetArchetypeName(string name)
        {
            this.archetypeName = name;
        }

        public void SetColorBehaviour(ColorBehaviour colorBehaviour)
        {
            this.colorJob = colorBehaviour;
        }

        public void SetColorBehaviourData(ColorBehaviourData colorBehaviourData)
        {
            this.jobData = colorBehaviourData;
        }
        #endregion
    }
    
}
