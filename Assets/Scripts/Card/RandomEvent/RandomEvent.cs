using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct AreaProbability
{
    public GameObject area;
    [Tooltip("地区发生这种事件的概率")]
    [Range(0f, 1f)]
    public double probability;

    public AreaProbability(GameObject area, double probability)
    {
        this.area = area;
        this.probability = probability;
    }
}
public class RandomEvent : Card
{
    [Header("地区和地区发生概率,顺序为中 北 西南 西 南")]
    public AreaProbability[] areasProbability;
    public TextMeshProUGUI randomEventTips;
    public GameObject randomEventTipsButton;

    public void Use()
    {
        foreach (AreaProbability areaProbability in areasProbability)
        {
            if(Random.Range(0f, 1f) <= areaProbability.probability)
            {
                if (!randomEventTipsButton.activeSelf)
                {
                    randomEventTipsButton.SetActive(true);
                    ButtonsManager.MyInstance.isHappenEvent = true;
                }
                Use(areaProbability.area);
            }
        }
    }
    public virtual void Use(GameObject area)
    {

    }
    public string GetAreaName(GameObject area)
    {
        return area.GetComponent<AreaScript>().areaDetail.areaName;
    }
}
