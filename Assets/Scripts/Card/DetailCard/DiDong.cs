using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiDong", menuName = "Card/DiDong", order = 10)]
public class DiDong : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //受地震影响时，人口不减少
        //受地震影响时，收成降低更少。
        //受地震影响时，产出降低更少。
    }
}
