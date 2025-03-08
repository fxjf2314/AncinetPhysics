using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "War", menuName = "Card/RandomEvent/War")]
public class War : RandomEvent
{
    [Header("սʤ�������˿���")]
    public int wPopulation;
    [Header("սʤ�������ճɵ��˿�ϵ��")]
    public float wFoodRatio;
    [Header("սʤ�����ٲ������˿�ϵ��")]
    public float wCoinRatio;
    [Header("ս�ܷ������˿���")]
    public int dPopulation;
    [Header("ս�ܷ������ճɵ��˿�ϵ��")]
    public float dFoodRatio;
    [Header("ս�ܷ��ٲ������˿�ϵ��")]
    public float dCoinRatio;
    [Header("�˿ڶ��һ�����һ��population��ֵ�����ӻ�ʤ���ʵ���")]
    public float probabilityChange;
    [Header("˫��ʧȥ�˿ڵĸ���")]
    public float probability;
    public float pEffectiveness;
    public float fEffectiveness;
    public float cEffectiveness;

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
            depopulation(area, probability,wPopulation);
            depopulation(surroundingArea, probability,dPopulation);
            deFoodCoin(area, surroundingArea);
        }
        else
        {
            randomEventTips.text += GetAreaName(surroundingArea) + "սʤ" + GetAreaName(area) + "\n";
            depopulation(surroundingArea, probability, wPopulation);
            depopulation(area,probability,dPopulation);
            deFoodCoin(surroundingArea, area);
        }

    }
    private void depopulation(GameObject area, double probability,int population)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            AreaDetail areaDetail = area.GetComponent<AreaScript>().areaDetail;
            int pChange = Convert.ToInt16(population*pEffectiveness);
            areaDetail.population -= pChange;
            if(areaDetail.population < 1)
                areaDetail.population = 1;
        }
    }
    private void deFoodCoin(GameObject winArea,GameObject defeatArea)
    {
        AreaDetail wAreaDetail=winArea.GetComponent<AreaScript>().areaDetail;
        int wAreaPopulation=wAreaDetail.population;
        AreaDetail dAreaDetail=defeatArea.GetComponent<AreaScript>().areaDetail;
        int dAreaPopulation =dAreaDetail.population;
        wAreaDetail.food -= wAreaPopulation * wFoodRatio;
        wAreaDetail.coin -= wAreaPopulation * wCoinRatio;
        dAreaDetail.food -= (dAreaPopulation+wAreaPopulation) * dFoodRatio;
        dAreaDetail.coin -= (dAreaPopulation + wAreaPopulation) * dCoinRatio;
    }
}
