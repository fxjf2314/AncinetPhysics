using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HuoYao", menuName = "Card/HuoYao", order = 13)]
public class HuoYao : Card
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
                        HandCard.MyInstance.applicationArea[i].PopulationControl(1);
                        int index = area.transform.parent.GetComponent<RandomEventOccurs>().randomEvents[2].GetAreaIndex(area.gameObject);
                        area.transform.parent.GetComponent<RandomEventOccurs>().randomEvents[2].areasProbability[index].Probability += 0.1;
                        if (area.areaDetail.Effectiveness["War"] > 0)
                            area.areaDetail.Effectiveness["War"] -= area.areaDetail.Effectiveness["War"] < 0.2f ? area.areaDetail.Effectiveness["War"] : 0.2f;
                    }
                }
            }

        }
        
        //发动战争概率增大
        //受战争影响减小
    }
}
