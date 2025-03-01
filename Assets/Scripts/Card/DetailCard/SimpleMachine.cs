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
                    }
                }
            }

        }
        
        //受战争影响降低。
    }
}
