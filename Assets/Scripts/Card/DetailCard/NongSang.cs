using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NongSang", menuName = "Card/NongSang", order = 5)]
public class NongSang : Card
{
    public override void Use(AreaScript area)
    {
        Debug.Log("111");
        base.Use(area);
        if (area.cards.Count < 3)
        {
            area.FoodControl(300);
        }
    }
}
