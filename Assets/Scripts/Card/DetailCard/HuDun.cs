using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HuDun", menuName = "Card/HuDun", order = 19)]
public class HuDun : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //受战争影响大幅降低
    }
}
