using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandCard : Card
{
    [SerializeField]
    private GameObject cardPrefab;//��ſ�������Ӧ��Ԥ�Ƽ�

    public GameObject GetCardPrefab()
    {
        return cardPrefab;//��ȡԤ�Ƽ�
    }
}
