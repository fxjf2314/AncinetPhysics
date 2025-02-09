using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HuoYao", menuName = "Card/HuoYao", order = 13)]
public class HuoYao : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
            area.PopulationControl(1);
        //����ս����������
        //��ս��Ӱ���С
    }
}
