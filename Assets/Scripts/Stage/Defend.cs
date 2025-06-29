using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defend", menuName = "StageEffect/Defend")]
public class Defend : StageEffect
{
    [SerializeField]
    List<Disaster> disasterList;
    public override void Effect()
    {
        ConfirmedCardsManager confirmedCardsManager = ConfirmedCardsManager.MyInstance;
        List<Card> cards = confirmedCardsManager.ConfirmedCards;
        foreach (Card card in cards)
        {
            if (card != null)
            {
                card.effectiveness["Disaster"] *=2 ;
            }
        }
        foreach (Disaster disaster in disasterList)
        {
            if (disaster != null)
            {
                disaster.cValue = disaster.o_cValue*=2;
            }
        }
    }
}
