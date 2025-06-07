using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandCard : MonoBehaviour,ISaveAndLoadGame
{
    private static HandCard instance;

    public static HandCard MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandCard>();
            }
            return instance;
        }
    }

    public bool isLevelMode;

    public AreaScript targetArea;
    public AreaScript[] applicationArea=new AreaScript[5];
    public AreaScript[] allarea=new AreaScript[5];

    private int totalCardNum;

   

    public List<Card> cards;//从选牌阶段获取卡组

    [SerializeField]
    private Card[] handCards;//手牌

    [SerializeField]
    private HandCardSlot[] slots;

    private HashSet<int> indexes = new HashSet<int>();

    

    private void Awake()
    {
        if (HandCardGroup.Instance != null)
        {
            cards = HandCardGroup.Instance.handCards;
            isLevelMode = false;
        }
        else
        {
            cards = ConfirmedCardsManager.MyInstance.ConfirmedCards;
            isLevelMode = true;
        }
        foreach(Card card in cards)
        {
            if (card != null)
            {
                card.MyId = totalCardNum;
                totalCardNum++;
            }
        }

        if(!GameSeed.MyInstance.isCardSeedInit)
            GameSeed.MyInstance.InitSeed();
        
        
        int count = 0;
       
        
        foreach(int index in GameSeed.MyInstance.cardSeed)
        {
            if (count < 4)
            {
                handCards[count] = cards[index];
                count++;
            }
            else
                break;
            
        }
        count = 0;
        foreach (HandCardSlot slot in slots)
        {
            if (slot.MyCard == null)
            {
                slot.MyCard = handCards[count];

                //Debug.Log(handCards[count].GetId());
                
                
                if (slot.MyCard!=null)
                {
                    CardUI cardUI = slot.gameObject.AddComponent<CardUI>();
                    cardUI.ChangeCardData(slot.MyCard);
                }
                count++;
            }
        }
    }

    

    public void NextCard(HandCardSlot slot)
    {
        
        slot.MyCard = handCards[3];
        CardUI cardUI = slot.GetComponent<CardUI>();
        cardUI.ChangeCardData(slot.MyCard);
        int totalCount = indexes.Count;
        if(GameSeed.MyInstance.cardSeed.Count > 3)
        {
            
            if (cards[GameSeed.MyInstance.cardSeed[3]] != null || GameSeed.MyInstance.cardSeed.Count > 3)
            { 
                handCards[3] = cards[GameSeed.MyInstance.cardSeed[3]];
                slots[3].MyCard = cards[GameSeed.MyInstance.cardSeed[3]];
                CardUI card = slots[3].GetComponent<CardUI>();
                card.ChangeCardData(slots[3].MyCard);
            }
        }
        else
        {
            handCards[3] = null;
            slots[3].MyCard = null;
            CardUI card = slots[3].GetComponent<CardUI>();
            Destroy(card);
            Color color = slots[3].handCardBack.color;
            color.a = 0;
            slots[3].handCardBack.color = color;

            slots[3].handCardIcon.color = color;
            slots[3].handcardTitle.text = "";
        }



        

    }

    public void Save(ref GameData gameData)
    {
        gameData.handCards = cards;
        gameData.cardSeed = GameSeed.MyInstance.cardSeed;
        gameData.isCardSeedInit = GameSeed.MyInstance.isCardSeedInit;
        gameData.houseSeed = GameSeed.MyInstance.houseSeed;
        gameData.isHouseSeedInit = GameSeed.MyInstance.isHouseSeedInit;
    }

    public void Load(GameData gameData)
    {
        cards = gameData.handCards;
        GameSeed.MyInstance.cardSeed = gameData.cardSeed;
        GameSeed.MyInstance.isCardSeedInit = gameData.isCardSeedInit;
        GameSeed.MyInstance.houseSeed = gameData.houseSeed;
        GameSeed.MyInstance.isHouseSeedInit= gameData.isHouseSeedInit;
    }
}
