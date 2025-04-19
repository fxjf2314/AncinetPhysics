using UnityEngine;

public class Canvasadjust : MonoBehaviour
{
    public float widthRatio = 16f / 9f; // 设计分辨率的宽高比
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AdaptScreen();
    }

    void AdaptScreen()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        if (screenRatio > widthRatio)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * screenRatio, rectTransform.sizeDelta.y);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / screenRatio);
        }
    }
}