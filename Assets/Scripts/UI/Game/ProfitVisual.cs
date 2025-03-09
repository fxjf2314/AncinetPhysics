using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfitVisual : MonoBehaviour
{
    public float moveDistance = 1.0f;

    // 持续时间（秒）
    public float duration = 1.0f;

    // 是否启用协程
    public bool startCoroutineOnStart = true;

    private void Start()
    {
        if (startCoroutineOnStart)
        {
            StartCoroutine(MoveAndFadeCoroutine());
        }
    }

    // 调用此方法可以手动启动协程
    public void StartMoveAndFade()
    {
        StartCoroutine(MoveAndFadeCoroutine());
    }

    private IEnumerator MoveAndFadeCoroutine()
    {
        // 获取初始位置
        Vector3 startPosition = transform.position;
        // 获取初始透明度
        float startAlpha = GetComponent<TextMeshProUGUI>().color.a;

        // 计算每帧的时间比例
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // 计算当前进度（0到1之间）
            float t = elapsedTime / duration;

            // 线性插值计算当前位置
            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + moveDistance * t,
                startPosition.z
            );

            // 线性插值计算当前透明度
            float currentAlpha = Mathf.Lerp(startAlpha, 0.0f, t);

            // 更新透明度
            Color color = GetComponent<TextMeshProUGUI>().color;
            color.a = currentAlpha;
            GetComponent<TextMeshProUGUI>().color = color;

            // 累加时间
            elapsedTime += Time.deltaTime;

            // 暂停，直到下一帧
            yield return null;
        }

        // 确保最终状态正确
        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + moveDistance,
            startPosition.z
        );
        GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0); // 完全隐藏
    }
}
