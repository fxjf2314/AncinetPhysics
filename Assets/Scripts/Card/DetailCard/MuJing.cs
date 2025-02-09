using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MuJing", menuName = "Card/MuJing", order = 16)]
public class MuJing : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
            area.PopulationControl(1);//ÏàÁÚ
        
    }
}
