using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionalMarriage", menuName = "Card/RandomEvent/RegionalMarriage")]
public class RegionalMarriage : RandomEvent
{
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        area.GetComponent<AreaScript>().areaDetail.population += 1;
        surroundingArea.GetComponent<AreaScript>().areaDetail.population += 1;
        randomEventTips.text += GetAreaName(area) + "与" + GetAreaName(surroundingArea) + "联姻"+"\n";
        randomEventTips.text += GetAreaName(area) +"和"+ GetAreaName(surroundingArea) + "人口各增加一点" + "\n";
    }
}
