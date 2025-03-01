using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<Card> handCards;
    public int round;
    public int people;
    public int harvest;
    public int output;

    public GameData()
    {
        handCards = new List<Card>();
    }



}
