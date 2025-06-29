using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "War", menuName = "StageAffect/War")]
public class StageWar : StageAffect
{
    public War war;
    private double[] o_areaProbabilities=new double[5];
    public override void Effect()
    {
        Debug.Log(1111);
        ConfirmedCardsManager confirmedCardsManager = ConfirmedCardsManager.MyInstance;
        foreach (Card card in confirmedCardsManager.ConfirmedCards)
        {
            if (card != null)
            {
                Debug.Log(card);
                Debug.Log(card.effectiveness["war"]);
                card.effectiveness["war"] = card.effectiveness["war"] * 2;
                Debug.Log(card);
                Debug.Log(card.effectiveness["war"]);
            }
        }
        for (int i = 0; i < war.areasProbability.Length; i++)
        {
            o_areaProbabilities[i] = war.areasProbability[i].Probability;
            war.areasProbability[i].Probability *= 2;
        }
    }
    public override void ResetEffect()
    {
        Debug.Log(1111);
        ConfirmedCardsManager confirmedCardsManager = ConfirmedCardsManager.MyInstance;
        List<Card> cards = confirmedCardsManager.ConfirmedCards;
        foreach (Card card in cards)
        {
            if (card != null)
            {
                Debug.Log(card);
                Debug.Log(card.effectiveness["war"]);
                card.effectiveness["war"] = 1;
                Debug.Log(card);
                Debug.Log(card.effectiveness["war"]);
            }
        }
        for (int i = 0; i < war.areasProbability.Length; i++)
        {
            war.areasProbability[i].Probability = o_areaProbabilities[i];
        }
    }
}
