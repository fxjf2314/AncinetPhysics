using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : ScriptableObject,IDescribable,IUseable//���ƵĻ��࣬���ڿ��ƺ��ֺ��ĳ�ʼ��
{
    [SerializeField]
    private Sprite icon;//ͼ��
    
    [SerializeField]
    private string title;//����
    
    [SerializeField]
    private string description;//Ч������

    [SerializeField]
    private GameObject cardPrefab;

    public Sprite MyIcon { get => icon; }

    public virtual string GetDescription()
    {
        return description;//��ȡ����
    }

    public Sprite GetSprite()
    {
        return icon;//��ȡͼ��
    }

    public string GetTitle()
    {
        return title;//��ȡ����
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
