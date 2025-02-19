using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chongzhen", menuName = "Card/Chongzhen", order = 2)]
public class Chongzhen : Card
{
    public override void Use(AreaScript area)
    {
        Debug.Log("111");
        base.Use(area);
        if (area.cards.Count < 3)
        {
            area.PopulationControl(1);
            area.FoodControl(100);
        }
        
    }
}
