using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YinShua", menuName = "Card/YinShua", order = 15)]
public class YinShua : Card
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
                        HandCard.MyInstance.applicationArea[i].CoinControl(100);
                        if(area.areaDetail.isCoin == true)
                        {
                            HandCard.MyInstance.applicationArea[i].CoinControl(200);
                        }
                        else
                        {
                            area.areaDetail.isCoin = true;
                        }
                    }
                }
            }

        }
        
    }
}
