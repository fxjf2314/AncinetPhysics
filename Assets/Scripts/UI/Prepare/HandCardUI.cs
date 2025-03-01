using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardUI : MonoBehaviour,IPointerClickHandler
{
    //[SerializeField]
    //Card slotData;
    Card originCardData;
    CardUI originCard;
    bool isContainCard;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveCard();
        }
    }

    public void AddCard(CardUI card,Card cardData)
    {
        isContainCard = true;
        originCardData = cardData;
        originCard = card;
        Image icon = transform.Find("Icon").GetComponent<Image>();
        icon.sprite = cardData.GetSprite();
        Color color = icon.color;
        color.a = 1f;
        icon.color = color;
        //TextMeshProUGUI title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        //title.text = cardData.GetDescription().title;
    }

    public void RemoveCard()
    {
        if (isContainCard)
        {
            //ÒÆ³ýÃÉ²¼
            originCard.RemoveCardFromHandCrad();
            HandCardGroupUI.Instance.RemoveCardUIFromHand(originCardData, gameObject);
            Destroy(gameObject);
        }
    }


}
