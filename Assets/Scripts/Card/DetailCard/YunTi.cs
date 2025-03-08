using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YunTi", menuName = "Card/YunTi", order = 7)]
public class YunTi : Card
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



                        int index=area.transform.parent.GetComponent<RandomEventOccurs>().randomEvents[2].GetAreaIndex(area.gameObject);
                        area.transform.parent.GetComponent<RandomEventOccurs>().randomEvents[2].areasProbability[index].Probability += 0.1;
                        //战争概率增大

                    }
                }
            }
        }
    }
}
