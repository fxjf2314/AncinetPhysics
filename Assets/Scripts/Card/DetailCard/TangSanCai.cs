using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TangSanCai", menuName = "Card/TangSanCai", order = 14)]
public class TangSanCai : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {
            area.PopulationControl(1);//����һ������
            area.CoinControl(100);
        }
           
    }
}
