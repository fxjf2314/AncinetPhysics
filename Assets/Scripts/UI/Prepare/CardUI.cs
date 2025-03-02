using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    
    bool isInHand;//是否已经加入手牌
    [SerializeField]
    Card cardData;//卡牌配置文件


    void Start()
    {
        if(SceneManager.GetActiveScene().name != "wwwww")
        {
            InitCard();
        }
    }

    void InitCard()
    {
        Image icon = transform.Find("Icon").GetComponent<Image>();
        icon.sprite = cardData.GetSprite();
        TextMeshProUGUI title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        //title.text = cardData.GetDescription().title;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionManger.Instance.OpenSimpleDes(cardData);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionManger.Instance.CloseSimpleDes();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (DescriptionManger.Instance.isCanClick)
            {
                //Debug.Log("?");
                DescriptionManger.Instance.OpenDetailDes(cardData);
            }
            else if (!isInHand)
            {
                if (HandCardGroup.Instance.handCards.Count < 8)
                {
                    //添加到手牌                 
                    HandCardGroupUI.Instance.AddCardUIToHand(gameObject, cardData);
                    isInHand = true;
                    AddPanelOnCard();
                }
                else
                {
                    Debug.Log("手牌已满");
                }
            }
        }
    }
    
    void AddPanelOnCard()
    {
        Image icon = transform.Find("Icon").GetComponent<Image>();
        Color color = icon.color;
        color.a = 0.5f;
        icon.color = color;
    }

    public void RemoveCardFromHandCrad()
    {
        isInHand = false;
        Image icon = transform.Find("Icon").GetComponent<Image>();
        Color color = icon.color;
        color.a = 1f;
        icon.color = color;
    }

    public void ChangeCardData(Card card)
    {
        cardData = card;
    }
}
