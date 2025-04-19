using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    Rigidbody2D rb;
    RectTransform rectTransform;
    [SerializeField][Header("是否禁用方向拖拽")]
    bool forbiddenX,forbiddenY;
    [SerializeField]
    RectTransform dragArea;
    float time;
    public Slider cardSlider;
    float sliderValue;
    Vector2 origionPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
        origionPos = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        time = 0;
        rb.velocity = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = new Vector2(eventData.delta.x, eventData.delta.y);
        if(forbiddenX)delta.x = 0;
        if(forbiddenY)delta.y = 0;
        LimitPos(rectTransform.anchoredPosition + delta, dragArea);       
        time += Time.deltaTime;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 velocity = (Vector2)(eventData.position - eventData.pressPosition) / time * 0.1f;
        velocity.y = 0;
        rb.velocity = velocity;
        time = 0;
    }

    public void UpdateSlider()
    {
        cardSlider.onValueChanged.RemoveAllListeners();
        cardSlider.value = sliderValue;
        cardSlider.onValueChanged.AddListener((float value) =>
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Vector2 newPos = new Vector2((origionPos.x - value * (dragArea.rect.xMax - dragArea.rect.xMin)), origionPos.y);
            rectTransform.anchoredPosition = newPos;
        });
    }

    void LimitPos(Vector2 newPos, RectTransform limitArea)
    {
        newPos = new Vector2(Mathf.Clamp(newPos.x, dragArea.rect.xMin, dragArea.rect.xMax), Mathf.Clamp(newPos.y, dragArea.rect.yMin, dragArea.rect.yMax));
        rectTransform.anchoredPosition = newPos;
        sliderValue = -(rectTransform.anchoredPosition.x - origionPos.x) / (dragArea.rect.xMax - dragArea.rect.xMin);
        cardSlider.value = sliderValue;
    }

    void MouseScroll()
    {
        // 监听鼠标滚轮事件
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0)
        {
            // 根据滚轮方向调整UI位置
            Vector2 scrollDelta = new Vector2(scroll * 50000f, 0); // 滚轮滚动速度可以根据需求调整
            rb.velocity = scrollDelta;
        }
    }

    private void Update()
    {
        MouseScroll();
        LimitPos(rectTransform.anchoredPosition, dragArea);

    }
}