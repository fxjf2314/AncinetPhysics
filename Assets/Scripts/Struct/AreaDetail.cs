using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct AreaDetail 
{
    
    public string areaName;
    public int oPopulation;
    public int population;
    public bool isCoin;
    public float oCoin;
    public float coin;
    public float oFood;
    public float food;
    public Sprite areaIcon;
    public GameObject[] surroundingAreas;
    public Dictionary<string,float> Effectiveness;

    /*public AreaDetail(string name, int population, bool YinShua,float coin, float food, Sprite icon, GameObject[] surroundingAreas = null)
    {
        this.areaName = name;
        this.population = population;
        this.coin = coin;
        this.food = food;
        this.areaIcon = icon;
        this.surroundingAreas = surroundingAreas ?? new GameObject[0];

        this.isCoin = YinShua;

        this.Effectiveness = new Dictionary<string, float>()
            {
            {"War", 1f },
            {"EarthQuake",1f },
            {"Flood",1f },
            {"DustStorm",1f },
            {"Fog",1f }
        };

    }*/
}
