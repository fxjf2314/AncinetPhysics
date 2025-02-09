using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuJiangYan", menuName = "Card/DuJiangYan", order = 1)]
public class DuJiangYan : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {
            area.FoodControl(200);
        }
        
        //²»ÔÙÊÜºéÀÔÔÖº¦£¨È±Ê§£©
    }
}
