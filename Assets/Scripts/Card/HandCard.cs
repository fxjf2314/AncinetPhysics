using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandCard : MonoBehaviour
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

    public AreaScript targetArea;
    public AreaScript[] applicationArea=new AreaScript[5];
    public AreaScript[] allarea=new AreaScript[5];

    private int totalCardNum;

    [SerializeField]
    private List<Card> cards;//从选牌阶段获取卡组

    [SerializeField]
    private Card[] handCards;//手牌

    [SerializeField]
    private HandCardSlot[] slots;

    private HashSet<int> indexes = new HashSet<int>();
    
    private void Start()
    {
        cards = HandCardGroup.Instance.handCards;
        
        foreach(Card card in cards)
        {
            if (card != null)
            {
                totalCardNum++;
            }
        }
        
        int count = 0;
        if(totalCardNum >= 4)
        {
            while (indexes.Count < 4)
            {
                int index = UnityEngine.Random.Range(0, totalCardNum);
                if (cards[index] != null)
                {
                    indexes.Add(index);
                }

            }
        }
        else
        {
            while (indexes.Count < (int)totalCardNum)
            {
                int index = UnityEngine.Random.Range(0, totalCardNum);
                if (cards[index] != null)
                {
                    indexes.Add(index);
                }
            }
        }
        
        foreach(int index in indexes)
        {

            handCards[count] = cards[index];
            count++;
        }
        count = 0;
        foreach (HandCardSlot slot in slots)
        {
            if (slot.MyCard == null)
            {
                slot.MyCard = handCards[count];
                count++;
            }
        }
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M))
        {
            handCards[0] = cards[0];
            foreach(HandCardSlot slot in slots)
            {
                if (slot.MyCard == null)
                {
                    slot.MyCard = handCards[0];
                    break;
                }
            }
        }

    }

    public void NextCard(HandCardSlot slot)
    {
        Debug.Log("2222");

        slot.MyCard = handCards[3];
        int totalCount = indexes.Count;
        if(indexes.Count != totalCardNum)
        {
            while (indexes.Count < (totalCount + 1))
            {
                int index = UnityEngine.Random.Range(0, totalCardNum);
                if (cards[index] != null)
                {
                    indexes.Add(index);
                }
                
            }
            int[] array = indexes.ToArray();
            handCards[3] = cards[array[indexes.Count - 1]];
            slots[3].MyCard = cards[array[indexes.Count - 1]];
        }
        else
        {
            handCards[3] = null;
            slots[3].MyCard = null;
            Color color = slots[3].handCardBack.color;
            color.a = 0;
            slots[3].handCardBack.color = color;

            slots[3].handCardIcon.color = color;
            slots[3].handcardTitle.text = "";
        }
        
    }
}
