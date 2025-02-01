using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandCard : Card
{
    [SerializeField]
    private GameObject cardPrefab;//存放卡牌所对应的预制件

    public GameObject GetCardPrefab()
    {
        return cardPrefab;//获取预制件
    }
}
