using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<Card> handCards;
    public AreasJson[] areas;
    public int[] hashset;

    public int round;
    public int people;
    public int harvest;
    public int output;


    public GameData()
    {
        handCards = new List<Card>();
        areas = new AreasJson[5];
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i] = new AreasJson();
        }
    }

}

[System.Serializable]
public class AreasJson
{
    public AreaDetail areaDetail;
    public List<Card> cards;
}
