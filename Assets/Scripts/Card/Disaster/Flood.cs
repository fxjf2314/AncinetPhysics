using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flood", menuName = "Card/Disaster/Flood")]
public class Flood : Disaster
{
    [Tooltip("˫��ʧȥ�˿ڵĸ���")]
    public double probability;
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        depopulation(area, probability);
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        depopulation(surroundingArea, probability);
    }
}
