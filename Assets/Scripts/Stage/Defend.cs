using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defend", menuName = "StageAffect/Defend")]
public class Defend : StageAffect
{
    [SerializeField]
    List<Disaster> disasterList;
    public override void Effect()
    {
        Debug.Log(1111);
        ConfirmedCardsManager confirmedCardsManager = ConfirmedCardsManager.MyInstance;
        foreach (Card card in confirmedCardsManager.ConfirmedCards)
        {
            if (card != null)
            {
                Debug.Log(card);
                Debug.Log(card.effectiveness["disaster"]);
                card.effectiveness["disaster"] = card.effectiveness["disaster"]*2;
                Debug.Log(card);
                Debug.Log(card.effectiveness["disaster"]);
            }
        }
        foreach (Disaster disaster in disasterList)
        {
            if (disaster != null)
            {
                disaster.cValue = disaster.ocValue * 2;
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
                Debug.Log(card.effectiveness["disaster"]);
                card.effectiveness["disaster"] =1;
                Debug.Log(card);
                Debug.Log(card.effectiveness["disaster"]);
            }
        }
        foreach (Disaster disaster in disasterList)
        {
            if (disaster != null)
            {
                disaster.cValue = disaster.ocValue;
            }
        }
    }
}
