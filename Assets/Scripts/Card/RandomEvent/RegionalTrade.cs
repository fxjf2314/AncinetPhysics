using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionalTrade", menuName = "Card/RandomEvent/RegionalTrade")]
public class RegionalTrade : RandomEvent
{
    public int coinChange;
    public int foodChange;
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        Debug.Log(area.name + "��" + surroundingArea.name + "ͨ��");
        area.GetComponent<AreaScript>().areaDetail.coin += coinChange;
        area.GetComponent<AreaScript>().areaDetail.food+= foodChange;
        Debug.Log(area.name + "�����ճ�����");
        surroundingArea.GetComponent<AreaScript>().areaDetail.coin += coinChange;
        surroundingArea.GetComponent<AreaScript>().areaDetail.food += foodChange;
        Debug.Log(surroundingArea.name + "�����ճ�����");
    }
}
