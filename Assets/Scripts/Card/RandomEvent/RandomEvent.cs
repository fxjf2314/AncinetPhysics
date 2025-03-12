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
    [SerializeField]
    private double probability;

    public double Probability
    {
        get { return probability; }
        set { probability = value;}
    }

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
    private AreaProbability[] theAreaProbability;

    private void OnEnable()
    {
        theAreaProbability = new AreaProbability[areasProbability.Length];
        Array.Copy(areasProbability, theAreaProbability, areasProbability.Length);
    }
    private void OnDisable()
    {
        Array.Copy(theAreaProbability,areasProbability, areasProbability.Length);
    }
    public void Use()
    {
        foreach (AreaProbability areaProbability in areasProbability)
        {
            if(Random.Range(0f, 1f) <= areaProbability.Probability)
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
    public int GetAreaIndex(GameObject area) 
    {
        int index = 0;
        for (index = 0; index < areasProbability.Length; index++)
        {
            if (areasProbability[index].area == area)
                return index;
        }
        return -1;
    }
}
