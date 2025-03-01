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
        Debug.Log(area.name + "��" + surroundingArea.name + "����");
        area.GetComponent<AreaScript>().areaDetail.population += 1;
        Debug.Log(area.name + "�˿ڼ�һ");
        surroundingArea.GetComponent<AreaScript>().areaDetail.population += 1;
        Debug.Log(surroundingArea.name + "�˿ڼ�һ");
        randomEventTips.text += area.name + "��" + surroundingArea.name + "����";
    }
}
