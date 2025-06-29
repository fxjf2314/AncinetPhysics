using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Undone", menuName = "StageAffect/Undone")]
public class Undone : StageAffect
{
    public override void Effect()
    {
        Debug.Log(1111);
        ConfirmedCardsManager confirmedCardsManager = ConfirmedCardsManager.MyInstance;
        foreach (Card card in confirmedCardsManager.ConfirmedCards)
        {
            if (card != null)
            {
                Debug.Log(card);
                Debug.Log(card.effectiveness["coin"]);
                card.effectiveness["coin"] = card.effectiveness["coin"] * 2;
                Debug.Log(card);
                Debug.Log(card.effectiveness["coin"]);
            }
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
                Debug.Log(card.effectiveness["coin"]);
                card.effectiveness["coin"] = 1;
                Debug.Log(card);
                Debug.Log(card.effectiveness["coin"]);
            }
        }
    }
}
