using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HunTian", menuName = "Card/HunTian", order = 9)]
public class HunTian : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
            area.FoodControl(120);
        //受灾害提供正面效果
    }
}
