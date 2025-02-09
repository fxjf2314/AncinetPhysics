using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image icon;

    private void Start()
    {
        icon = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon.sprite != null)
        {
            icon.color = Color.gray;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (icon.sprite != null)
        {
            icon.color = Color.white;
        }

    }


    
}

