using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiDong", menuName = "Card/DiDong", order = 10)]
public class DiDong : Card
{
    public override void Use(AreaScript area)
    {
        base.Use(area);
        if (area.cards.Count < 3)
        {

        }
        //�ܵ���Ӱ��ʱ���˿ڲ�����
        //�ܵ���Ӱ��ʱ���ճɽ��͸��١�
        //�ܵ���Ӱ��ʱ���������͸��١�
    }
}
