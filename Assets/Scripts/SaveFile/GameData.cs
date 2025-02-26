using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<Card> handCards;

    public GameData()
    {
        handCards = new List<Card>();
    }



}
