using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TangSanCai", menuName = "Card/TangSanCai", order = 14)]
public class TangSanCai : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
            if (area != null)
            {
            if (area.cards.Count < 3)
            {
                for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                {
                    if (HandCard.MyInstance.applicationArea[i] != null)
                    {
                        HandCard.MyInstance.applicationArea[i].PopulationControl(1);//其中一个区域
                        HandCard.MyInstance.applicationArea[i].CoinControl(100);
                    }
                }
            }
        }
    }
}
