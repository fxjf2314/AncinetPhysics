using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disaster : Card
{
    [SerializeField]
    private Image effect;//��Ŷ�Ӧ�ֺ�����Ч

    public Image GetCardPrefab()
    {
        return effect;//��ȡԤ�Ƽ�
    }
}
