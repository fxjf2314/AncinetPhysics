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
                        if (area.areaDetail.Effectiveness["DustStorm"] > 0)
                            area.areaDetail.Effectiveness["DustStorm"] -= 0.1f;
                        if (area.areaDetail.Effectiveness["Fog"] > 0)
                            area.areaDetail.Effectiveness["Fog"] -= 0.1f;
                    }
                }
            }
        }
       
        //受沙尘暴，大雾天气影响降低（缺失）
    }
}
