using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YinShua", menuName = "Card/YinShua", order = 15)]
public class YinShua : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
            area.CoinControl(100);
        //有造纸术时进一步增多
    }
}
