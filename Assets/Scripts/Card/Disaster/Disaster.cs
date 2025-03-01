using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Disaster : Card
{
    public Sprite[] images; // 存储所有图片
    public double cValue;
    [SerializeField]
    private double pValue;
    [SerializeField]
    private int count;

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
    public void depopulation(GameObject area, double probability)
    {
        if (Random.Range(0f, 1f) <= probability)
        {
            if (area.GetComponent<AreaScript>().areaDetail.population > 1)
            {
                area.GetComponent<AreaScript>().areaDetail.population -= 1;
                Debug.Log(area.name + "人口减一");
            }
        }
    }
}
