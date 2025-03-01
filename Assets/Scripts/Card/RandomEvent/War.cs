using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "War", menuName = "Card/RandomEvent/War")]
public class War : RandomEvent
{
    public int population;
    [Header("人口多的一方多出一个population的值所增加获胜概率的量")]
    public float probabilityChange;
    [Header("双方失去人口的概率")]
    public float probability;
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        Debug.Log(area.name+"对" + surroundingArea.name+"发起战争");
        float winProbability=0.5f;
        int thisPopulation=area.GetComponent<AreaScript>().areaDetail.population;
        int thatPopulation= surroundingArea.GetComponent<AreaScript>().areaDetail.population;
        int n = thisPopulation - thatPopulation;
        winProbability = Mathf.Clamp(winProbability + n * probabilityChange,0.0f,1.0f);
        if (Random.Range(0f, 1f) <= winProbability)
        {
            Debug.Log(area.name + "战胜" + surroundingArea.name+ winProbability);
        }
        else
        {
            Debug.Log(surroundingArea.name + "战胜" + area.name + (1.0f-winProbability));
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
                area.GetComponent<AreaScript>().areaDetail.population -= 1;
                Debug.Log(area.name + "人口减一");
            }
        }
    }
}
