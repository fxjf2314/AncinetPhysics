using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandCardGroup : MonoBehaviour, ISaveAndLoadGame
{
    public static HandCardGroup Instance => instance;
    private static HandCardGroup instance;

    public List<Card> handCards => mHandCards;
    List<Card> mHandCards;

    int cardLimit = 8;

    

    private void Start()
    {
        instance = this;
        mHandCards = new List<Card>();
    }

    

    public void AddCardInGroup(Card card)
    {
        if(mHandCards.Count < cardLimit)
        {
            mHandCards.Add(card);
            
        }
    }

    public void RemoveCardFromGroup(Card card)
    {
        mHandCards.Remove(card);
    }

    public void Save<T>(ref T gameData)
    {
        (gameData as GameData).handCards = mHandCards;
    }

    public void Load<T>(T gameData)
    {
        mHandCards= (gameData as GameData).handCards;
        foreach(var card in mHandCards)
        {
            Debug.Log(card.name + card.GetDescription().title + card.GetDescription().area + card.GetDescription().effect);
        }
    }
}
