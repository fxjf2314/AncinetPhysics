using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionalMarriage", menuName = "Card/RandomEvent/RegionalMarriage")]
public class RegionalMarriage : RandomEvent
{
    public int population;
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        area.GetComponent<AreaScript>().areaDetail.population += population;
        surroundingArea.GetComponent<AreaScript>().areaDetail.population += population;
        randomEventTips.text += GetAreaName(area) + "��" + GetAreaName(surroundingArea) + "����"+"\n";
        randomEventTips.text += GetAreaName(area) +"��"+ GetAreaName(surroundingArea) + "�˿ڸ�����"+population+"��" + "\n";
    }
}
