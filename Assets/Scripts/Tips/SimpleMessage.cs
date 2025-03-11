using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleMessage : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private float data;
    
    [SerializeField]
    GameObject tip;
    
    [SerializeField]
    Image frameColor;
    
    [SerializeField]
    TextMeshProUGUI detailMessage;

    [SerializeField]
    string describe;

    public Color color;

    [SerializeField]
    TextMeshProUGUI profitDetail;

    string profitText;

    CanvasGroup tipCanvasGroup;

    private void Start()
    {
        profitDetail = tip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tipCanvasGroup = tip.GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(transform.CompareTag("ResourceIcon"))    
        {
            initProfitText();
            tip.GetComponent<RectTransform>().pivot = new Vector2(0.2f, 1.2f);
            profitDetail.transform.gameObject.SetActive(true);
            ShowToolTip(transform.position);
        }
        else
        {
            tip.GetComponent<RectTransform>().pivot = new Vector2(0.2f, 1.8f);
            profitDetail.transform.gameObject.SetActive(false);
            ShowToolTip(transform.position);
        }
        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    public void ShowToolTip(Vector3 position)
    {
        tipCanvasGroup.alpha = 1;
        tip.transform.position = position;

        detailMessage.text = describe;
        //frameColor.color = color;
    }

    public void HideToolTip()
    {

        tipCanvasGroup.alpha = 0;
    }

    private void initProfitText()
    {
        profitText = string.Empty;
        //ProfitVisualization.Instance.GetAreaData();
        switch (transform.name)
        {
            case "Population":
                data = UIManager.MyInstance.totalPopulation - UIManager.MyInstance.lastPopulation;
                if(data < 0)
                {
                    profitText += string.Format($"{data}：来自区域");
                }
                else
                {
                    profitText += string.Format($"+{data}：来自区域");
                }
                foreach (AreaScript area in AreaManager.MyInstance.areas)
                {
                    if (area.areaDetail.oPopulation < 0)
                    {
                        profitText += string.Format($"\n·\t{area.areaDetail.oPopulation}：来自{area.areaDetail.areaName}");
                    }
                    else
                    {
                        profitText += string.Format($"\n·\t+{area.areaDetail.oPopulation}：来自{area.areaDetail.areaName}");
                    }

                }
                break;
            case "Coin":
                data = UIManager.MyInstance.totalCoin - UIManager.MyInstance.lastCoin;
                if (data < 0)
                {
                    profitText += string.Format($"{data}：来自区域");
                }
                else
                {
                    profitText += string.Format($"+{data}：来自区域");
                }
                foreach (AreaScript area in AreaManager.MyInstance.areas)
                {
                    if(area.areaDetail.oCoin < 0)
                    {
                        profitText += string.Format($"\n·\t{area.areaDetail.oCoin}：来自{area.areaDetail.areaName}");
                    }
                    else
                    {
                        profitText += string.Format($"\n·\t+{area.areaDetail.oCoin}：来自{area.areaDetail.areaName}");
                    }

                }
                
                break;
            case "Food":
                data = UIManager.MyInstance.totalFood - UIManager.MyInstance.lastFood;
                if (data < 0)
                {
                    profitText += string.Format($"{data}：来自区域");
                }
                else
                {
                    profitText += string.Format($"+{data}：来自区域");
                }
                foreach (AreaScript area in AreaManager.MyInstance.areas)
                {
                    if (area.areaDetail.oFood < 0)
                    {
                        profitText += string.Format($"\n·\t{area.areaDetail.oFood}：来自{area.areaDetail.areaName}");
                    }
                    else
                    {
                        profitText += string.Format($"\n·\t+{area.areaDetail.oFood}：来自{area.areaDetail.areaName}");
                    }

                }
                break;
            default: break;
        }

        
        
        /*foreach(AreaScript area in AreaManager.MyInstance.areas)
        {
            area.areaDetail.
            
        }*/
        profitDetail.text = profitText;
    }
}
