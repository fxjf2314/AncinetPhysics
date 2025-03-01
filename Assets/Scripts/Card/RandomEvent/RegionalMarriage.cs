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
        Debug.Log(area.name + "与" + surroundingArea.name + "联姻");
        area.GetComponent<AreaScript>().areaDetail.population += 1;
        Debug.Log(area.name + "人口加一");
        surroundingArea.GetComponent<AreaScript>().areaDetail.population += 1;
        Debug.Log(surroundingArea.name + "人口加一");
        randomEventTips.text += area.name + "与" + surroundingArea.name + "联姻";
    }
}
