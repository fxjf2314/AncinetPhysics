using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    
    bool isInHand;//�Ƿ��Ѿ���������
    [SerializeField]
    GameObject panelPrefab;//��Ƭ�ɲ�Ԥ�Ƽ�
    GameObject panel;//��Ƭ�ɲ�
    [SerializeField]
    Card cardData;//���������ļ�


    void Start()
    {
        InitCard();
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
                DescriptionManger.Instance.OpenDetailDes(cardData);
            }
            else if (!isInHand)
            {
                if (HandCardGroup.Instance.handCards.Count < 8)
                {
                    //��ӵ�����                 
                    HandCardGroupUI.Instance.AddCardUIToHand(gameObject, cardData);
                    isInHand = true;
                    AddPanelOnCard();
                }
                else
                {
                    Debug.Log("��������");
                }
            }
        }
    }
    
    void AddPanelOnCard()
    {
        panel = Instantiate(panelPrefab, transform.position, Quaternion.identity);
        panel.transform.SetParent(transform);
        var rect = GetComponent<RectTransform>();
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.rect.width, rect.rect.height);
        panel.GetComponent<RectTransform>().localPosition = Vector3.zero;
        //panel.GetComponent<RectTransform>()
    }

    public void RemoveCardFromHandCrad()
    {
        if(panel != null)
        {
            Destroy(panel);
            isInHand = false;
        }
    }

}
