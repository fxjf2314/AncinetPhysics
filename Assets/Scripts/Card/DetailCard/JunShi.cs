using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JunShi", menuName = "Card/JunShi", order = 6)]
public class JunShi : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {
            //发生战争概率增大
            //受战争影响降低
        }

    }
}
