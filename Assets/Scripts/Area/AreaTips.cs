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
        }
    }

    

    public void FadeIn(Collider area)
    {
        string AddOrRemove = string.Empty;
        if(area.transform.GetComponent<AreaScript>() != null)
        {
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
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(FadeCanvasGroupRoutine(tipsCanvasGroup, tipsCanvasGroup.alpha, 0, duration, startPosition));
        }
    }

    public void PlacedAchivement(AreaScript area)
    {
        foreach(Card card in area.cards)
        {
            if(card != null)
            {
                foreach(Image achivement in areaAchivements)
                {
                    if(achivement.sprite == null)
                    {
                        achivement.sprite = card.GetSprite();
                    }

                }
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
            // ʹ��Mathf.Lerp��ָ��ʱ����ƽ������alphaֵ
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            tipsRectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, counter / duration);
            // ���alphaֵ�Ѿ��ӽ�Ŀ��ֵ����ǰ����ѭ��
            if (Mathf.Approximately(cg.alpha, end))
            {
                break;
            }

            yield return null; // �ȴ���һ֡
        }
        // ȷ��alphaֵ��ȷ����ΪĿ��ֵ
        cg.alpha = end;
        // ���Э�����ã��ͷ���Դ
        currentCoroutine = null;
    }
}
