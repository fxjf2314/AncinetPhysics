using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YunTi", menuName = "Card/YunTi", order = 7)]
public class YunTi : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if(area.cards.Count < 3)
        {
            //战争概率增大
        }

    }
}
