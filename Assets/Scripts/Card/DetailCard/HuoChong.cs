using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HuoChong", menuName = "Card/HuoChong", order = 18)]
public class HuoChong : Card
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
