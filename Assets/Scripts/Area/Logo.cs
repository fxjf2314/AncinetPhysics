using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    private static Logo instance;

    public static Logo MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Logo>();
            }
            return instance;
        }
    }

    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private CanvasGroup logoCanvasGroup;

    [SerializeField]
    private RectTransform logoRectTransform;
    private Vector2 finalPosition = new Vector2(-293, -123);
    private Vector2 startPosition;

    private Coroutine currentCoroutine;

    private void Start()
    {
        logoCanvasGroup = GetComponent<CanvasGroup>();
        logoRectTransform = GetComponent<RectTransform>();
        logoCanvasGroup.alpha = 1;
        startPosition = logoRectTransform.anchoredPosition;
        
    }

    public void FadeIn()
    {
        
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(FadeCanvasGroupRoutine(logoCanvasGroup, logoCanvasGroup.alpha, 1, duration, finalPosition));
        }
    }

    public void FadeOut()
    {
        logoCanvasGroup.blocksRaycasts = false;
        logoCanvasGroup.interactable = false;

        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(FadeCanvasGroupRoutine(logoCanvasGroup, logoCanvasGroup.alpha, 0, duration, startPosition));
        }
    }

    private IEnumerator FadeCanvasGroupRoutine(CanvasGroup cg, float start, float end, float duration, Vector2 targetPosition)
    {

        float counter = 0f;
        Vector2 currentPosition = logoRectTransform.anchoredPosition;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            // 使用Mathf.Lerp在指定时间内平滑过渡alpha值
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            logoRectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, counter / duration);
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
