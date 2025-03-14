using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaTips : MonoBehaviour
{
    private static AreaTips instance;

    public static AreaTips MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AreaTips>();
            }
            return instance;
        }
    }

    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private CanvasGroup tipsCanvasGroup;

    [SerializeField]
    private RectTransform tipsRectTransform;
    private Vector2 finalPosition = new Vector2(-293, -123);
    private Vector2 startPosition;

    private Coroutine currentCoroutine;

    private AreaScript areaScript;
    [SerializeField]
    private TextMeshProUGUI areaName;
    [SerializeField]
    private TextMeshProUGUI areaPopu;
    [SerializeField]
    private TextMeshProUGUI areaCoin;
    [SerializeField]
    private TextMeshProUGUI areaFood;
    [SerializeField]
    private Image areaIcon;
    [SerializeField]
    private Image[] areaAchivements;

    private void Start()
    {
        tipsCanvasGroup = GetComponent<CanvasGroup>();
        tipsRectTransform = GetComponent<RectTransform>();
        tipsCanvasGroup.alpha = 0;
        startPosition = tipsRectTransform.anchoredPosition;
        foreach(Image image in areaAchivements)
        {
            image.sprite = null;
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }
    }


    

    public void FadeIn(Collider area)
    {
        string AddOrRemove = string.Empty;
        Logo.MyInstance.FadeOut();
        if(area.transform.GetComponent<AreaScript>() != null)
        {
            tipsCanvasGroup.blocksRaycasts = true;
            tipsCanvasGroup.interactable = true;
            areaScript = area.transform.GetComponent<AreaScript>();
            PlacedAchivement(areaScript);
            areaName.text = areaScript.GetName();
            areaPopu.text = areaScript.GetPopulation();
            areaIcon.sprite = areaScript.GetIcon();
            if(areaScript.areaDetail.coin < 0)
            {
                areaCoin.color = Color.red;
                areaCoin.text = areaScript.GetCoin();
            }
            else
            {
                areaCoin.color = Color.green;
                AddOrRemove += string.Format("+");
                areaCoin.text = AddOrRemove + areaScript.GetCoin();
                AddOrRemove = string.Empty;
            }
            
            if (areaScript.areaDetail.food < 0)
            {
                areaFood.color = Color.red;
                areaFood.text = areaScript.GetFood();
            }
            else
            {
                areaFood.color = Color.green;
                AddOrRemove += string.Format("+");
                areaFood.text = AddOrRemove + areaScript.GetFood();
                
            }
        }
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(FadeCanvasGroupRoutine(tipsCanvasGroup, tipsCanvasGroup.alpha, 1, duration, finalPosition));
        }
    }
    public void FadeOut()
    {
        Logo.MyInstance.FadeIn();
        tipsCanvasGroup.blocksRaycasts = false;
        tipsCanvasGroup.interactable = false;
        
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(FadeCanvasGroupRoutine(tipsCanvasGroup, tipsCanvasGroup.alpha, 0, duration, startPosition));
        }
    }

    public void PlacedAchivement(AreaScript area)
    {
        for(int i = 0;i < area.cards.Count ;i++)
        {
            areaAchivements[i].sprite = area.cards[i].GetSprite();
            Color color = areaAchivements[i].color;
            color.a = 1;
            areaAchivements[i].color = color;
        }
        if (area.cards.Count < areaAchivements.Length)
        {
            for (int i = area.cards.Count; i < areaAchivements.Length; i++)
            {
                areaAchivements[i].sprite = null;
                Color color = areaAchivements[i].color;
                color.a = 0; // 设置透明
                areaAchivements[i].color = color;
            }
        }
        if (area.cards.Count == 0)
        {
            foreach (Image image in areaAchivements)
            {
                image.sprite = null;
                Color color = image.color;
                color.a = 0;
                image.color = color;
            }
        }
    }

    private IEnumerator FadeCanvasGroupRoutine(CanvasGroup cg, float start, float end, float duration, Vector2 targetPosition)
    {

        float counter = 0f;
        Vector2 currentPosition = tipsRectTransform.anchoredPosition;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            // 使用Mathf.Lerp在指定时间内平滑过渡alpha值
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            tipsRectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, counter / duration);
            // 如果alpha值已经接近目标值，提前结束循环
            if (Mathf.Approximately(cg.alpha, end))
            {
                break;
            }

            yield return null; // 等待下一帧
        }
        // 确保alpha值精确设置为目标值
        cg.alpha = end;
        // 清除协程引用，释放资源
        currentCoroutine = null;
    }
}
