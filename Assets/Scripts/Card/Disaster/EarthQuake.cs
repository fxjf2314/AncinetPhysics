using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthQuake", menuName = "Card/Disaster/EarthQuake")]
public class EarthQuake : Disaster
{
    public override void Use(GameObject area)
    {
        GameObject[] surroundingAreas = area.GetComponent<AreaScript>().areaDetail.surroundingAreas;
        depopulation(area, 1f);
        foreach (GameObject surroundingArea in surroundingAreas)
        {
            depopulation(surroundingArea, 1f);
        }
    }
}
