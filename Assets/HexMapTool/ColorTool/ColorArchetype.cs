﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorArchetype : IColorArchetype
{
    string archetypeName;
    Color color;
    ColorBehaviour colorJob;
    ColorBehaviourData jobData;

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
}
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
