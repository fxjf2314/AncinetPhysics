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
            Depopulation(area, probability,wPopulation);
            Depopulation(surroundingArea, probability,dPopulation);
            DeFoodCoin(area, surroundingArea);
        }
        else
        {
            randomEventTips.text += GetAreaName(surroundingArea) + "սʤ" + GetAreaName(area) + "\n";
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
