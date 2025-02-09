using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZiMu", menuName = "Card/ZiMu", order = 20)]
public class ZiMu : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //受战争影响降低
        //战争提供正面收益
    }
}
