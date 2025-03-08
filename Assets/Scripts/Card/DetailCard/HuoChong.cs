using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HuoChong", menuName = "Card/HuoChong", order = 18)]
public class HuoChong : Card
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
                        area.areaDetail.Effectiveness["War"] = -1;
                    }
                }
            }
        }
        //��ս��Ӱ�콵��
        //ս���ṩ��������
    }
}
