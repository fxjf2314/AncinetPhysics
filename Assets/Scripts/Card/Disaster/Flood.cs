using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flood", menuName = "Card/Disaster/Flood")]
public class Flood : Disaster
{   
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        Depopulation(area);
        DeFood(area);
        DeCoin(area);
        GameObject surroundingArea = surroundingAreas[Random.Range(0, surroundingAreas.Length)];
        Depopulation(surroundingArea);
        DeFood(surroundingArea);
        DeCoin(surroundingArea);
    }
}
