using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiDong", menuName = "Card/DiDong", order = 10)]
public class DiDong : Card
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

                    }
                }
            }
        }
        
        //�ܵ���Ӱ��ʱ���˿ڲ�����
        //�ܵ���Ӱ��ʱ���ճɽ��͸��١�
        //�ܵ���Ӱ��ʱ���������͸��١�
    }
}
