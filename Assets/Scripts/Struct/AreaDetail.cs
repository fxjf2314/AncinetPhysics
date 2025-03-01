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
    public int population;
    public float coin;
    public float food;
    public Sprite areaIcon;
    public GameObject[] surroundingAreas;

    public AreaDetail(string name, int population, float coin, float food, Sprite icon, GameObject[] surroundingAreas = null)
    {
        this.areaName = name;
        this.population = population;
        this.coin = coin;
        this.food = food;
        this.areaIcon = icon;
        this.surroundingAreas = surroundingAreas ?? new GameObject[0]; 
    }

}
