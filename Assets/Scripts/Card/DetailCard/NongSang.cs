using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NongSang", menuName = "Card/NongSang", order = 5)]
public class NongSang : Card
{
    public override void Use(AreaScript area)
    {
        Debug.Log("111");
        base.Use(area);
            if (area != null)
            {
            if (area.cards.Count < 3)
            {
                for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
                {
                    if (HandCard.MyInstance.applicationArea[i] != null)
                    {
                        HandCard.MyInstance.applicationArea[i].FoodControl(300);
                    }
                }
            }
        }
        
    }
}
