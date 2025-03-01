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
    [Tooltip("�������������¼��ĸ���")]
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
    [Header("�����͵�����������,˳��Ϊ�б�������")]
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
