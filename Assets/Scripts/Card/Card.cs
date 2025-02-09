using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : ScriptableObject,IDescribable,IUseable//卡牌的基类，用于卡牌和灾害的初始化
{
    [SerializeField]
    private Sprite icon;//图标
    
    [SerializeField]
    private string title;//名字
    
    [SerializeField]
    private string description;//效果描述

    [SerializeField]
    private GameObject cardPrefab;

    public Sprite MyIcon { get => icon; }

    public virtual string GetDescription()
    {
        return description;//获取描述
    }

    public Sprite GetSprite()
    {
        return icon;//获取图标
    }

    public string GetTitle()
    {
        return title;//获取名字
    }

    public GameObject GetPrefab()
    {
        return cardPrefab;
    }

    public virtual void Use(AreaScript area)
    {
        if (area.cards.Count < 3)
        {
        
            area.cards.Add(this);
        }
        

    }
}
