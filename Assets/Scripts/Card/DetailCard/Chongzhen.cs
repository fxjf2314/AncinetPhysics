using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chongzhen", menuName = "Card/Chongzhen", order = 2)]
public class Chongzhen : Card
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
                        HandCard.MyInstance.applicationArea[i].PopulationControl(1);
                        HandCard.MyInstance.applicationArea[i].FoodControl(100);
                    }
                }
            }
        }
        
        
    }
}
