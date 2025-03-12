using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Disaster : Card
{
    public string title;
    public Sprite[] images; // �洢����ͼƬ
    public double cValue;
    private double pValue;
    private int count;
    public int population;
    [Tooltip("ʧȥ�˿ڵĸ���")]
    public double probability;
    [Tooltip("ʧȥ������ϵ��")]
    public float coinRatio;
    [Tooltip("ʧȥ�ճɵ�ϵ��")]
    public float foodRatio;

    private void OnEnable()
    {
        count = 0;
        pValue = 0;
    }
    public void Judge()
    {
        if (DisasterManager.canOccur)
        {
            count++;
            pValue = Math.Min(cValue * count, 1.0);
            if (Random.Range(0f, 1f) <= pValue)
            {
                count = 0;
                DisasterManager.nextDisaster = this;
                DisasterManager.canOccur = false;
            }
        }
    }
    public virtual void Use(GameObject area)
    {

    }
    public void Depopulation(GameObject area)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            int pChange = Convert.ToInt16(population * area.GetComponent<AreaScript>().areaDetail.Effectiveness[name]);
            int oPopulation = area.GetComponent<AreaScript>().areaDetail.population;
            area.GetComponent<AreaScript>().areaDetail.population -= pChange;
            if (area.GetComponent<AreaScript>().areaDetail.population < 1)
                area.GetComponent<AreaScript>().areaDetail.population = 1;
            int pRealChange = area.GetComponent<AreaScript>().areaDetail.population-oPopulation;
            //VisualizeEvent(area.transform, "population", pRealChange);
        }
    }
    public void DeFood(GameObject area)
    {
        float fChange = area.GetComponent<AreaScript>().areaDetail.food * foodRatio * area.GetComponent<AreaScript>().areaDetail.Effectiveness[name];
        area.GetComponent<AreaScript>().areaDetail.food -= fChange;
        //VisualizeEvent(area.transform, "food", fChange);
    }
    public void DeCoin(GameObject area)
    {
        float cChange = area.GetComponent<AreaScript>().areaDetail.coin * coinRatio * area.GetComponent<AreaScript>().areaDetail.Effectiveness[name];
        area.GetComponent<AreaScript>().areaDetail.coin -= cChange;
        //VisualizeEvent(area.transform,"coin",cChange);
    }
}
