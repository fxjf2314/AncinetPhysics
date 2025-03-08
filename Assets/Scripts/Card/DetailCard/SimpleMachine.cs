using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleMachine", menuName = "Card/SimpleMachine", order = 11)]
public class SimpleMachine : Card
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
                        HandCard.MyInstance.applicationArea[i].CoinControl(180);
                        if (area.areaDetail.Effectiveness["War"] > 0)
                            area.areaDetail.Effectiveness["War"] -= area.areaDetail.Effectiveness["War"] < 0.2f ? area.areaDetail.Effectiveness["War"] : 0.2f;
                    }
                }
            }

        }
        
        //受战争影响降低0.2。
    }
}
