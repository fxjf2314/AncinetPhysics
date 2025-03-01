using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DustStorm", menuName = "Card/Disaster/DustStorm")]
public class DustStorm : Disaster
{
    [Tooltip("失去人口的概率")]
    public double probability;
    public override void Use(GameObject area)
    {
        depopulation(area, probability);
    }
}
