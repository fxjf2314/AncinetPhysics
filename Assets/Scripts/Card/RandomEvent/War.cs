using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "War", menuName = "Card/RandomEvent/War")]
public class War : RandomEvent
{
    [Header("战胜方减少人口数")]
    public int wPopulation;
    [Header("战胜方减少收成的人口系数")]
    public float wFoodRatio;
    [Header("战胜方减少产出的人口系数")]
    public float wCoinRatio;
    [Header("战败方减少人口数")]
    public int dPopulation;
    [Header("战败方减少收成的人口系数")]
    public float dFoodRatio;
    [Header("战败方少产出的人口系数")]
    public float dCoinRatio;
    [Header("人口多的一方多出一个population的值所增加获胜概率的量")]
    public float probabilityChange;
    [Header("双方失去人口的概率")]
    public float probability;
    public float pEffectiveness;
    public float fEffectiveness;
    public float cEffectiveness;

    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        randomEventTips.text += GetAreaName(area) + "向" + GetAreaName(surroundingArea) + "发起战争" + "\n";
        float winProbability=0.5f;
        int thisPopulation=area.GetComponent<AreaScript>().areaDetail.population;
        int thatPopulation= surroundingArea.GetComponent<AreaScript>().areaDetail.population;
        int n = thisPopulation - thatPopulation;
        winProbability = Mathf.Clamp(winProbability + n * probabilityChange,0.0f,1.0f);
        if (Random.Range(0f, 1f) <= winProbability)
        {
            randomEventTips.text += GetAreaName(area) + "战胜" + GetAreaName(surroundingArea)+"\n";
            depopulation(area, probability,wPopulation);
            depopulation(surroundingArea, probability,dPopulation);
            deFoodCoin(area, surroundingArea);
        }
        else
        {
            randomEventTips.text += GetAreaName(surroundingArea) + "战胜" + GetAreaName(area) + "\n";
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
