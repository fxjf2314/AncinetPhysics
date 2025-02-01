using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("InteractiveObj"))
                {
                    TextMeshProUGUI areaTitle = hit.collider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                    //enabled = true;
                }
            }
        }
    }
}
