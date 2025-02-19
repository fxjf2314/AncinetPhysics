using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FengHuo", menuName = "Card/FengHuo", order = 8)]
public class FengHuo : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //受战争效果降低
    }
}
