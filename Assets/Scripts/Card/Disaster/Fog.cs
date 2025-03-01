using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fog", menuName = "Card/Disaster/Fog")]
public class Fog : Disaster
{
    [Tooltip("失去人口的概率")]
    public double probability;
    public override void Use(GameObject area)
    {
        Debug.Log(area.name + "发生" + this.name);
        depopulation(area, probability);
    }
}
