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
            Depopulation(area, probability,wPopulation);
            Depopulation(surroundingArea, probability,dPopulation);
            DeFoodCoin(area, surroundingArea);
        }
        else
        {
            randomEventTips.text += GetAreaName(surroundingArea) + "战胜" + GetAreaName(area) + "\n";
            Depopulation(surroundingArea, probability, wPopulation);
            Depopulation(area,probability,dPopulation);
            DeFoodCoin(surroundingArea, area);
        }

    }
    private void Depopulation(GameObject area, double probability,int population)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            int pChange = Convert.ToInt16(population * area.GetComponent<AreaScript>().areaDetail.Effectiveness["War"]);
            int oPopulation = area.GetComponent<AreaScript>().areaDetail.population;
            area.GetComponent<AreaScript>().areaDetail.population -= pChange;
            if(area.GetComponent<AreaScript>().areaDetail.population < 1)
                area.GetComponent<AreaScript>().areaDetail.population = 1;
            int pRealChange = area.GetComponent<AreaScript>().areaDetail.population - oPopulation;
            //VisualizeEvent(area.transform, "population", pRealChange);
        }
    }
    private void DeFoodCoin(GameObject winArea,GameObject defeatArea)
    {
        float wFChange = winArea.GetComponent<AreaScript>().areaDetail.population * wFoodRatio * winArea.GetComponent<AreaScript>().areaDetail.Effectiveness["War"];
        winArea.GetComponent<AreaScript>().areaDetail.food -= wFChange;
        //VisualizeEvent(winArea.transform, "food", wFChange);
        float wCChange = winArea.GetComponent<AreaScript>().areaDetail.population * wCoinRatio * winArea.GetComponent<AreaScript>().areaDetail.Effectiveness["War"];
        winArea.GetComponent<AreaScript>().areaDetail.coin -= wCChange;
        //VisualizeEvent(winArea.transform, "coin", wCChange);
        float dFChange = (defeatArea.GetComponent<AreaScript>().areaDetail.population + winArea.GetComponent<AreaScript>().areaDetail.population) * dFoodRatio * defeatArea.GetComponent<AreaScript>().areaDetail.Effectiveness["War"];
        defeatArea.GetComponent<AreaScript>().areaDetail.food -= dFChange;
       //VisualizeEvent(defeatArea.transform, "food", dFChange);
        float dCChange = (defeatArea.GetComponent<AreaScript>().areaDetail.population + winArea.GetComponent<AreaScript>().areaDetail.population) * dCoinRatio * defeatArea.GetComponent<AreaScript>().areaDetail.Effectiveness["War"];
        defeatArea.GetComponent<AreaScript>().areaDetail.coin -= dCChange;
        //VisualizeEvent(defeatArea.transform,"coin",dCChange);
    }
}
