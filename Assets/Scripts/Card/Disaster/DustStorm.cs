using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DustStorm", menuName = "Card/Disaster/DustStorm")]
public class DustStorm : Disaster
{
    public override void Use(GameObject area)
    {
        depopulation(area);
    }
}
