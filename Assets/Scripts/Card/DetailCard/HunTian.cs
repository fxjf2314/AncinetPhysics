using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HunTian", menuName = "Card/HunTian", order = 9)]
public class HunTian : Card
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
                        HandCard.MyInstance.applicationArea[i].FoodControl(120);
                        var keys = new List<string>(area.areaDetail.Effectiveness.Keys);
                        foreach (var key in keys)
                        {
                            if (key == "War")
                                continue;
                            area.areaDetail.Effectiveness[key] = -1;
                        }
                    }
                }
            }

        }
        
        //受灾害提供正面效果
    }
}
