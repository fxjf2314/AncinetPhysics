using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    Rigidbody2D rb;
    RectTransform rectTransform;
    [SerializeField][Header("是否禁用方向拖拽")]
    bool forbiddenX,forbiddenY;
    [SerializeField]
    RectTransform dragArea;
    float time;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
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

    void LimitPos(Vector2 newPos, RectTransform limitArea)
    {
        newPos = new Vector2(Mathf.Clamp(newPos.x, dragArea.rect.xMin, dragArea.rect.xMax), Mathf.Clamp(newPos.y, dragArea.rect.yMin, dragArea.rect.yMax));
        rectTransform.anchoredPosition = newPos;
    }

    private void Update()
    {
        LimitPos(rectTransform.anchoredPosition, dragArea);
    }
}