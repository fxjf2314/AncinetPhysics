using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSeed : MonoBehaviour
{
    private static GameSeed instance;
    public static GameSeed MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSeed>();
            }
            return instance;
        }
    }

    public bool isCardSeedInit = false;

    public List<int> cardSeed;

    private HashSet<int> cardSeeds = new HashSet<int>();

    public void InitSeed()
    {
        //Debug.Log(HandCard.MyInstance.cards.Count);
        cardSeed = new List<int>(HandCard.MyInstance.cards.Count);
        while(cardSeeds.Count < HandCard.MyInstance.cards.Count)
        {
            int index = UnityEngine.Random.Range(0, HandCard.MyInstance.cards.Count);
            cardSeeds.Add(index);

        }
        cardSeed = cardSeeds.ToList();
        isCardSeedInit = true;
    }
}
