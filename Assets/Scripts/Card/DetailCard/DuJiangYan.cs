using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuJiangYan", menuName = "Card/DuJiangYan", order = 1)]
public class DuJiangYan : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
            if (area != null)
            {
            if (area.cards.Count < 3)
            {
                for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                {
                    if (HandCard.MyInstance.applicationArea[i] != null)
                    {
                        HandCard.MyInstance.applicationArea[i].FoodControl(200);
                        area.areaDetail.Effectiveness["Flood"] = 0;
                    }
                }
            }
        }
        
        //²»ÔÙÊÜºéÀÔÔÖº¦£¨È±Ê§£©
    }
}
