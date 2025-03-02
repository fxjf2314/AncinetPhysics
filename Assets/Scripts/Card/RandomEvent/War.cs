using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "War", menuName = "Card/RandomEvent/War")]
public class War : RandomEvent
{
    public int population;
    [Header("�˿ڶ��һ�����һ��population��ֵ�����ӻ�ʤ���ʵ���")]
    public float probabilityChange;
    [Header("˫��ʧȥ�˿ڵĸ���")]
    public float probability;
    public double pEffectiveness;
    public double fEffectiveness;
    public double cEffectiveness;
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        randomEventTips.text += GetAreaName(area) + "��" + GetAreaName(surroundingArea) + "����ս��" + "\n";
        float winProbability=0.5f;
        int thisPopulation=area.GetComponent<AreaScript>().areaDetail.population;
        int thatPopulation= surroundingArea.GetComponent<AreaScript>().areaDetail.population;
        int n = thisPopulation - thatPopulation;
        winProbability = Mathf.Clamp(winProbability + n * probabilityChange,0.0f,1.0f);
        if (Random.Range(0f, 1f) <= winProbability)
        {
            randomEventTips.text += GetAreaName(area) + "սʤ" + GetAreaName(surroundingArea)+"\n";
        }
        else
        {
            randomEventTips.text += GetAreaName(surroundingArea) + "սʤ" + GetAreaName(area) + "\n";
        }
        depopulation(area, probability);
        depopulation(surroundingArea, probability);
    }
    private void depopulation(GameObject area, double probability)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            if (area.GetComponent<AreaScript>().areaDetail.population > 1)
            {
                int pChange = Convert.ToInt16(1 * pEffectiveness);
                area.GetComponent<AreaScript>().areaDetail.population-= pChange;
                if (pChange > 0)
                {
                    randomEventTips.text = GetAreaName(area) + "����" + pChange + "���˿�" + "\n";
                } 
                else if(pChange < 0)
                {
                    randomEventTips.text = GetAreaName(area) + "����" + pChange + "���˿�" + "\n";
                }
            }
        }
    }
}
