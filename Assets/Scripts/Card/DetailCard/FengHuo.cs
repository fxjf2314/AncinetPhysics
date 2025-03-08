using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FengHuo", menuName = "Card/FengHuo", order = 8)]
public class FengHuo : Card
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
                        if(area.areaDetail.Effectiveness["War"]>0)
                            area.areaDetail.Effectiveness["War"] -= area.areaDetail.Effectiveness["War"] < 0.3f ? area.areaDetail.Effectiveness["War"] : 0.3f;
                        //受战争效果降低0.3
                    }
                }
            }
        }
    }
}
