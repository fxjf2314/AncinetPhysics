using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [TextArea(3, 5)]
    public string detailDes;

}


public class Card : ScriptableObject,IDescribable,IUseable//���ƵĻ��࣬���ڿ��ƺ��ֺ��ĳ�ʼ��
{
    [SerializeField]
    private Sprite icon;//ͼ��

    [SerializeField]
    private int id;//���


    [SerializeField]
    private Description cardDes;

    [SerializeField]
    private GameObject cardPrefab;

    public Sprite MyIcon { get => icon; }
    public int MyId { get => id; set => id = value; }

    public Sprite GetSprite()
    {
        return icon;//��ȡͼ��
    }

    public int GetId()
    {
        return id;
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
