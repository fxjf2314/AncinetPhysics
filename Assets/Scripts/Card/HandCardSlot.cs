using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardSlot : MonoBehaviour//,IPointerClickHandler
{
    public Card MyCard;

    
    public Sprite achivementBack;
    public Image handCardBack;
    public Image handCardIcon;
    public TextMeshProUGUI handcardTitle;
    public CanvasClickHandler application;

    //public Card MyCard { get => card; set => card = value; }

    private void Start()
    {
        HandCard.MyInstance.targetArea = null;
        for (int i = 0; i < HandCard.MyInstance.applicationArea.Length; i++)
        {
            HandCard.MyInstance.applicationArea[i] = null;
        }
        MyCard = null;
        Color color = handCardBack.color;
        color.a = 0;
        handCardBack.color = color;

        handCardIcon.color = color;
        handcardTitle.text = "";
    }

    private void Update()
    {
        if(MyCard !=  null)
        {
            Color color = handCardBack.color;
            color.a = 1;
            handcardTitle.text = MyCard.GetDescription().title;
            handCardBack.sprite = achivementBack;
            handCardIcon.sprite = MyCard.GetSprite();
            handCardBack.color = color;
            handCardIcon.color = color;
        }

        if (application != null)
        {
            if (application.ifapplication)
            {
                application.ifapplication = false;
                if (MyCard != null && HandCard.MyInstance.targetArea != null && HandCard.MyInstance.targetArea.cards.Count < 3)
                {
                    ButtonsManager.MyInstance.isPlaceCard = true;
                    ButtonsManager.MyInstance.stepButtons[2].transform.gameObject.SetActive(true);
                    
                    UseCard(HandCard.MyInstance.targetArea);
                    //HandCard.MyInstance.NextCard(this);
                    
                    GameSeed.MyInstance.cardSeed.Remove(MyCard.GetId());
                    MyCard = null;
                    Color color = handCardBack.color;
                    color.a = 0;
                    handCardBack.color = color;

                    handCardIcon.color = color;
                    handcardTitle.text = "";

                    HandCard.MyInstance.NextCard(this);
                    
                    AreaTips.MyInstance.FadeIn(HandCard.MyInstance.targetArea.GetComponent<Collider>());
                }
                else if(HandCard.MyInstance.targetArea.cards.Count >= 3)
                {
                    #region 按图索骥--复原模型
                    if (application.isdragone)
                    {
                        switch (application.cardtext.text)
                        {
                            case "《崇祯历书》":
                                application.model[0].position = new Vector3(124, -500, -82);
                                break;
                            case "地动仪":
                                application.model[1].position = new Vector3(124, -500, -82);
                                break;
                            case "都江堰":
                                application.model[2].position = new Vector3(124, -500, -82);
                                break;
                            case "烽火":
                                application.model[3].position = new Vector3(124, -500, -82);
                                break;
                            case "航海术":
                                application.model[4].position = new Vector3(124, -500, -82);
                                break;
                            case "虎蹲炮":
                                application.model[5].position = new Vector3(124, -500, -82);
                                break;
                            case "浑天仪":
                                application.model[6].position = new Vector3(124, -500, -82);
                                break;
                            case "火铳":
                                application.model[7].position = new Vector3(124, -500, -82);
                                break;
                            case "火药":
                                application.model[8].position = new Vector3(124, -500, -82);
                                break;
                            case "秦朝军事力学":
                                application.model[9].position = new Vector3(124, -500, -82);
                                break;
                            case "《墨经》《考工记》":
                                application.model[10].position = new Vector3(124, -500, -82);
                                break;
                            case "《木经》":
                                application.model[11].position = new Vector3(124, -500, -82);
                                break;
                            case "《农桑辑要》":
                                application.model[12].position = new Vector3(124, -500, -82);
                                break;
                            case "简单机械组":
                                application.model[13].position = new Vector3(124, -500, -82);
                                break;
                            case "司南":
                                application.model[14].position = new Vector3(124, -500, -82);
                                break;
                            case "唐三彩":
                                application.model[15].position = new Vector3(124, -500, -82);
                                break;
                            case "活字印刷术":
                                application.model[16].position = new Vector3(124, -500, -82);
                                break;
                            case "云梯":
                                application.model[17].position = new Vector3(124, -500, -82);
                                break;
                            case "造纸术":
                                application.model[18].position = new Vector3(124, -500, -82);
                                break;
                            case "子母炮":
                                application.model[19].position = new Vector3(124, -500, -82);
                                break;
                        }
                        application.isdragone = false;
                    }
                    #endregion
                }
            }
        }
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        if(application.ifapplication)
        {
            application.ifapplication = false;
            if (card != null && HandCard.MyInstance.targetArea != null && HandCard.MyInstance.targetArea.cards.Count < 3 )
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
    }*/

    public void UseCard(AreaScript area)
    {
        if(MyCard is IUseable)
        {
            (MyCard as IUseable).Use(area);
            
        }
    }

    
}
