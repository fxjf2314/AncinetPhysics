using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthQuake", menuName = "Card/Disaster/EarthQuake")]
public class EarthQuake : Disaster
{
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        Depopulation(area);
        DeCoin(area);
        DeFood(area);
        foreach (GameObject surroundingArea in surroundingAreas)
        {
            Depopulation(surroundingArea);
            DeFood(surroundingArea);
            DeCoin(surroundingArea);
        }
    }
}
