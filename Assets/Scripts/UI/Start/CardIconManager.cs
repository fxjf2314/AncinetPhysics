using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardIconManager : MonoBehaviour
{
    [SerializeField]
    List<Card> cards;
    int i = 0;

    [Header("ͼƬ1")]
    public Image image1;
    [Header("ͼƬ2")]
    public Image image2;
    [Header("�л�ʱ��")]
    public float switchTime = 1.0f;
    [Header("�Զ��л����")]
    public float autoSwitchInterval = 3.0f;
    bool isAutoSwitch = true;

    private bool isSwitching = false;
    private bool isImage1Active = true;

    void Start()
    {
        // ��ʼ��ͼƬ͸����
        image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, 1.0f);
        image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, 0.0f);

        if (isAutoSwitch)
        {
            // �����Զ��л�Э��
            StartCoroutine(AutoSwitch());
        }
    }


    // �л�Э��
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

            // ���Բ�ֵ���㵱ǰ͸����
            float currentAlpha1 = Mathf.Lerp(startAlpha1, targetAlpha1, t);
            float currentAlpha2 = Mathf.Lerp(startAlpha2, targetAlpha2, t);

            // ����ͼƬ͸����
            image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, currentAlpha1);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, currentAlpha2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ������͸������ȷ
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

    // �Զ��л�Э��
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
