using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
                if (hit.collider.CompareTag("Area") && !EventSystem.current.IsPointerOverGameObject())
                {
                    HandCard.MyInstance.targetArea = hit.transform.GetComponent<AreaScript>();                    
                    //AreaTips.MyInstance.FadeOut();
                    AreaTips.MyInstance.FadeIn(hit.collider);
                    //Debug.Log("111");
                }
                else 
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        AreaTips.MyInstance.FadeOut();
                    }
                    
                }
            }
        }
    }

    
}
