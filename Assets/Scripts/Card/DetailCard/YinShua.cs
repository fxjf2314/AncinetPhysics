using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YinShua", menuName = "Card/YinShua", order = 15)]
public class YinShua : Card
{
    public override void Use(AreaScript area)
    {
        

        if (area != null)
        {
            if (area.cards.Count < 3)
            {
                for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                {
                    if (HandCard.MyInstance.applicationArea[i] != null)
                    {
                        HandCard.MyInstance.applicationArea[i].CoinControl(Convert.ToInt32(150 * effectiveness["coin"]));
                        if(area.areaDetail.isCoin == true)
                        {
                            HandCard.MyInstance.applicationArea[i].CoinControl(Convert.ToInt32(280 * effectiveness["coin"]));
                        }
                        else
                        {
                            area.areaDetail.isCoin = true;
                        }
                    }
                }
                base.Use(area);
            }

        }
        

    }
}
