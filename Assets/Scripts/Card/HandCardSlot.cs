using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardSlot : MonoBehaviour,IPointerClickHandler
{
    private Card card;

    
    public Sprite achivementBack;
    public Image handCardBack;
    public Image handCardIcon;
    public TextMeshProUGUI handcardTitle;

    public Card MyCard { get => card; set => card = value; }

    private void Start()
    {
        HandCard.MyInstance.targetArea = null;
        card = null;
        Color color = handCardBack.color;
        color.a = 0;
        handCardBack.color = color;

        handCardIcon.color = color;
        handcardTitle.text = "";
    }

    private void Update()
    {
        if(card !=  null)
        {
            Color color = handCardBack.color;
            color.a = 1;
            handcardTitle.text = card.GetDescription().title;
            handCardBack.sprite = achivementBack;
            handCardIcon.sprite = card.GetSprite();
            handCardBack.color = color;
            handCardIcon.color = color;
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(card != null && HandCard.MyInstance.targetArea != null && HandCard.MyInstance.targetArea.cards.Count < 3 )
            {
                ButtonsManager.MyInstance.isPlaceCard = true;
                ButtonsManager.MyInstance.stepButtons[2].transform.gameObject.SetActive(true);
                UseCard(HandCard.MyInstance.targetArea);
                //HandCard.MyInstance.NextCard(this);
                card = null;
                Color color = handCardBack.color;
                color.a = 0;
                handCardBack.color = color;

                handCardIcon.color = color;
                handcardTitle.text = "";
                HandCard.MyInstance.NextCard(this);
                AreaTips.MyInstance.FadeIn(HandCard.MyInstance.targetArea.GetComponent<Collider>());
            }
           
        }
    }

    public void UseCard(AreaScript area)
    {
        if(card is IUseable)
        {
            (card as IUseable).Use(area);
            
        }
    }

    
}
