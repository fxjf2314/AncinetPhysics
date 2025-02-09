using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HangHai", menuName = "Card/HangHai", order = 17)]
public class HangHai : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //增长最高的进行增长
    }
}
