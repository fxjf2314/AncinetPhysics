using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fog", menuName = "Card/Disaster/Fog")]
public class Fog : Disaster
{
    public override void Use(GameObject area)
    {
        depopulation(area);
    }
}
