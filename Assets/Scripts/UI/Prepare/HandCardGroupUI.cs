using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HandCardGroupUI : MonoBehaviour
{
    public static HandCardGroupUI Instance => instance;
    static HandCardGroupUI instance;

    [SerializeField]
    GameObject slotPrefab;
    List<GameObject> mSlots = new List<GameObject>();
    public List<GameObject> slots => mSlots;
    GameObject firstSlot;
    GameObject currentSlot;

    private void Start()
    {
        instance = this;
        firstSlot = transform.Find("CardSlot").gameObject;
        mSlots.Add(firstSlot);
        InitSlot();
    }

    void InitSlot()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject obj = Instantiate(slotPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            mSlots.Add(obj);
        }
    }

    public void AddCardUIToHand(GameObject card,Card cardData)
    {
        HandCardGroup.Instance.AddCardInGroup(cardData);  
        currentSlot = mSlots[HandCardGroup.Instance.handCards.Count - 1];
        HandCardUI handCardUI = currentSlot.GetComponent<HandCardUI>();
        handCardUI.AddCard(card.GetComponent<CardUI>(),cardData);
    }

    public void RemoveCardUIFromHand(Card cardData, GameObject slot)
    {
        mSlots.Remove(slot);
        HandCardGroup.Instance.RemoveCardFromGroup(cardData);
        GameObject obj = Instantiate(slotPrefab, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        mSlots.Add(obj);
        Image icon = obj.transform.Find("Icon").GetComponent<Image>();
        
    }

    
}
