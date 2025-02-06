using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disaster : Card
{
    [SerializeField]
    private Image effect;//存放对应灾害的特效

    public Image GetCardPrefab()
    {
        return effect;//获取预制件
    }
}
