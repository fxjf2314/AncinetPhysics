using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsByEffect : MonoBehaviour
{
    public SerializableDictionary<string, Card> cards;

    private static CardsByEffect instance;
    public static CardsByEffect Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardsByEffect>();
            }
            return instance;
        }
    }
}
