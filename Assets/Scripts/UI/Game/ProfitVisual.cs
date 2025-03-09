using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfitVisual : MonoBehaviour
{
    public float moveDistance = 1.0f;

    // ����ʱ�䣨�룩
    public float duration = 1.0f;

    // �Ƿ�����Э��
    public bool startCoroutineOnStart = true;

    private void Start()
    {
        if (startCoroutineOnStart)
        {
            StartCoroutine(MoveAndFadeCoroutine());
        }
    }

    // ���ô˷��������ֶ�����Э��
    public void StartMoveAndFade()
    {
        StartCoroutine(MoveAndFadeCoroutine());
    }

    private IEnumerator MoveAndFadeCoroutine()
    {
        // ��ȡ��ʼλ��
        Vector3 startPosition = transform.position;
        // ��ȡ��ʼ͸����
        float startAlpha = GetComponent<TextMeshProUGUI>().color.a;

        // ����ÿ֡��ʱ�����
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // ���㵱ǰ���ȣ�0��1֮�䣩
            float t = elapsedTime / duration;

            // ���Բ�ֵ���㵱ǰλ��
            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + moveDistance * t,
                startPosition.z
            );

            // ���Բ�ֵ���㵱ǰ͸����
            float currentAlpha = Mathf.Lerp(startAlpha, 0.0f, t);

            // ����͸����
            Color color = GetComponent<TextMeshProUGUI>().color;
            color.a = currentAlpha;
            GetComponent<TextMeshProUGUI>().color = color;

            // �ۼ�ʱ��
            elapsedTime += Time.deltaTime;

            // ��ͣ��ֱ����һ֡
            yield return null;
        }

        // ȷ������״̬��ȷ
        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + moveDistance,
            startPosition.z
        );
        GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0); // ��ȫ����
    }
}
