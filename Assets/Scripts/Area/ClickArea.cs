using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickArea : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro areaTitle;
    [SerializeField]
    private TextMeshPro areaPopu;
    [SerializeField]
    private GameObject areaPopuIcon;

    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    

    private void OnMouseEnter()
    {
        outline.enabled = true;
        areaTitle.enabled = true;
        areaPopu.enabled = true;
        areaPopuIcon.SetActive(true);
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
        areaTitle.enabled = false;
        areaPopu.enabled = false;
        areaPopuIcon.SetActive(false);
    }
}
