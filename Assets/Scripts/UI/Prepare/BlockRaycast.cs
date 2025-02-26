using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class BlockRaycast : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (var uiElement in FindObjectsOfType<Graphic>())
        {
            if (uiElement.gameObject != this)
            {
                uiElement.raycastTarget = false;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var uiElement in FindObjectsOfType<Graphic>())
        {
            if (uiElement.gameObject != this)
            {
                uiElement.raycastTarget = true;
            }
        }
    }
}
