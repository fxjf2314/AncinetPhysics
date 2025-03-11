using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardIconManager : MonoBehaviour
{
    [SerializeField]
    List<Card> cards;
    int i = 0;

    [Header("图片1")]
    public Image image1;
    [Header("图片2")]
    public Image image2;
    [Header("切换时间")]
    public float switchTime = 1.0f;
    [Header("自动切换间隔")]
    public float autoSwitchInterval = 3.0f;
    bool isAutoSwitch = true;

    private bool isSwitching = false;
    private bool isImage1Active = true;

    void Start()
    {
        // 初始化图片透明度
        image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, 1.0f);
        image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, 0.0f);

        if (isAutoSwitch)
        {
            // 开启自动切换协程
            StartCoroutine(AutoSwitch());
        }
    }


    // 切换协程
    private IEnumerator SwitchCoroutine()
    {
        isSwitching = true;

        float elapsedTime = 0.0f;
        float startAlpha1 = isImage1Active ? 1.0f : 0.0f;
        float startAlpha2 = isImage1Active ? 0.0f : 1.0f;
        float targetAlpha1 = isImage1Active ? 0.0f : 1.0f;
        float targetAlpha2 = isImage1Active ? 1.0f : 0.0f;

        while (elapsedTime < switchTime)
        {
            float t = elapsedTime / switchTime;

            // 线性插值计算当前透明度
            float currentAlpha1 = Mathf.Lerp(startAlpha1, targetAlpha1, t);
            float currentAlpha2 = Mathf.Lerp(startAlpha2, targetAlpha2, t);

            // 更新图片透明度
            image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, currentAlpha1);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, currentAlpha2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终透明度正确
        image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, targetAlpha1);
        image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, targetAlpha2);

        if (targetAlpha1 == 0.0f)
        {
            image1.sprite = cards[i].GetSprite();
        }
        if (targetAlpha2 == 0.0f)
        {
            image2.sprite = cards[i].GetSprite();
        }
        if(++i >= 20)
        {
            i = 0;
        }
        isImage1Active = !isImage1Active;
        isSwitching = false;
    }

    // 自动切换协程
    private IEnumerator AutoSwitch()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSwitchInterval);
            if (!isSwitching)
            {
                StartCoroutine(SwitchCoroutine());
            }
        }
    }
}
