using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recover", menuName = "StageAffect/Recover")]
public class Recover : StageAffect
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
                Debug.Log(card.effectiveness["food"]);
                card.effectiveness["food"] = card.effectiveness["food"] * 2;
                Debug.Log(card);
                Debug.Log(card.effectiveness["food"]);
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
                Debug.Log(card.effectiveness["food"]);
                card.effectiveness["food"] = 1;
                Debug.Log(card);
                Debug.Log(card.effectiveness["food"]);
            }
        }
    }
}
