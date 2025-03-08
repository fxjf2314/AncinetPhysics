using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HangHai", menuName = "Card/HangHai", order = 17)]
public class HangHai : Card
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
                        if (HandCard.MyInstance.applicationArea[i].areaDetail.food > HandCard.MyInstance.applicationArea[i].areaDetail.coin)
                        {
                            HandCard.MyInstance.applicationArea[i].FoodControl(250);
                        }
                        if (HandCard.MyInstance.applicationArea[i].areaDetail.food < HandCard.MyInstance.applicationArea[i].areaDetail.coin)
                        {
                            HandCard.MyInstance.applicationArea[i].CoinControl(250);
                        }
                    }
                }
            }
        }
        
        
    }
}
