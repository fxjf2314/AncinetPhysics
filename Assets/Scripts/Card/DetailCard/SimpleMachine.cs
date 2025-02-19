using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleMachine", menuName = "Card/SimpleMachine", order = 11)]
public class SimpleMachine : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
            area.CoinControl(180);
        //受战争影响降低。
    }
}
