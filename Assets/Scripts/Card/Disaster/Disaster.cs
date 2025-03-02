using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Disaster : Card
{
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
    public double pEffectiveness;
    public double fEffectiveness;
    public double cEffectiveness;

    private void OnEnable()
    {
        count = 0;
        pValue = 0;
        pEffectiveness = 1;
        fEffectiveness = 1;
        cEffectiveness = 1;
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
    public void depopulation(GameObject area)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            AreaDetail areaDetail = area.GetComponent<AreaScript>().areaDetail;
            int pChange = Convert.ToInt16(population * pEffectiveness);
            areaDetail.population -= pChange;
            if (areaDetail.population < 1)
                areaDetail.population = 1;
        }
    }
    public void deFood(GameObject area)
    {
        AreaDetail areaDetail = area.GetComponent<AreaScript>().areaDetail;
        areaDetail.food -= areaDetail.food*foodRatio;
    }
    public void deCoin(GameObject area)
    {
        AreaDetail areaDetail = area.GetComponent<AreaScript>().areaDetail;
        areaDetail.coin -= areaDetail.coin * coinRatio;
    }
}
