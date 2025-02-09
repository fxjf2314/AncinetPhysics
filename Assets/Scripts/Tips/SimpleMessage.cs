using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleMessage : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    GameObject tip;
    
    [SerializeField]
    Image frameColor;
    
    [SerializeField]
    TextMeshProUGUI detailMessage;

    [SerializeField]
    string describe;

    public Color color;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowToolTip(transform.position);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    public void ShowToolTip(Vector3 position)
    {
        tip.SetActive(true);
        tip.transform.position = position;

        detailMessage.text = describe;
        frameColor.color = color;
    }

    public void HideToolTip()
    {

        tip.SetActive(false);
    }
}
