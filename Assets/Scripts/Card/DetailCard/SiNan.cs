using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SiNan", menuName = "Card/SiNan", order = 3)]
public class SiNan : Card
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
                        HandCard.MyInstance.applicationArea[i].CoinControl(150);
                    }
                }
            }
        }
       
        //ÊÜÉ³³¾±©£¬´óÎíÌìÆøÓ°Ïì½µµÍ£¨È±Ê§£©
    }
}
