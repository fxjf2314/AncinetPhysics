using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZiMu", menuName = "Card/ZiMu", order = 20)]
public class ZiMu : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //��ս��Ӱ�콵��
        //ս���ṩ��������
    }
}
