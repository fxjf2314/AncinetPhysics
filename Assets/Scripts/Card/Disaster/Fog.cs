using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fog", menuName = "Card/Disaster/Fog")]
public class Fog : Disaster
{
    [Tooltip("ʧȥ�˿ڵĸ���")]
    public double probability;
    public override void Use(GameObject area)
    {
        Debug.Log(area.name + "����" + this.name);
        depopulation(area, probability);
    }
}
