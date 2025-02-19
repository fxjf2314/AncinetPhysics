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
        if (area.cards.Count < 3)
        {
            area.CoinControl(150);
        }
        
        //ÊÜÉ³³¾±©£¬´óÎíÌìÆøÓ°Ïì½µµÍ£¨È±Ê§£©
    }
}
