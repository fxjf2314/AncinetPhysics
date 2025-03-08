using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Description
{
    [TextArea(3,5)]
    public string title;
    [TextArea(3, 5)]
    public string area;
    [TextArea(3, 5)]
    public string effect;
}


public class Card : ScriptableObject,IDescribable,IUseable//卡牌的基类，用于卡牌和灾害的初始化
{
    [SerializeField]
    private Sprite icon;//图标

    [SerializeField]
    protected int id;//编号


    [SerializeField]
    private Description cardDes;

    [SerializeField]
    private GameObject cardPrefab;


    public Sprite MyIcon { get => icon; }

    public Sprite GetSprite()
    {
        return icon;//获取图标
    }


    public Description GetDescription()
    {
        return cardDes;
    }

    public GameObject GetPrefab()
    {
        return cardPrefab;
    }

    public virtual void Use(AreaScript area)
    {
        if (area != null)
        {
            if (area.cards.Count < 3)
            {

                area.cards.Add(this);
            }
        }
    }
}
