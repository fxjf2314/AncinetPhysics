using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<Card> handCards;
    public AreasJson[] areas;
    public Vector3[] modelPosition;
    public int[] hashset;
    public List<int> cardSeed;
    public bool isCardSeedInit;

    public int round;
    public int people;
    public int harvest;
    public int output;


    public GameData()
    {
        handCards = new List<Card>();
        areas = new AreasJson[5];
        modelPosition = new Vector3[20];
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i] = new AreasJson();
        }
        cardSeed = new List<int>();
    }

}

[System.Serializable]
public class AreasJson
{
    public int population;
    public float food;
    public float coin;
    public List<Card> cards;
}
