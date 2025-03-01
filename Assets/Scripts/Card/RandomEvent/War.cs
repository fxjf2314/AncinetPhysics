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
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        Debug.Log(area.name+"��" + surroundingArea.name+"����ս��");
        float winProbability=0.5f;
        int thisPopulation=area.GetComponent<AreaScript>().areaDetail.population;
        int thatPopulation= surroundingArea.GetComponent<AreaScript>().areaDetail.population;
        int n = thisPopulation - thatPopulation;
        winProbability = Mathf.Clamp(winProbability + n * probabilityChange,0.0f,1.0f);
        if (Random.Range(0f, 1f) <= winProbability)
        {
            Debug.Log(area.name + "սʤ" + surroundingArea.name+ winProbability);
        }
        else
        {
            Debug.Log(surroundingArea.name + "սʤ" + area.name + (1.0f-winProbability));
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
                Debug.Log(area.name + "�˿ڼ�һ");
            }
        }
    }
}
