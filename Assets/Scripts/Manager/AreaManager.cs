using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaManager : MonoBehaviour
{
    [SerializeField]
    AreaScript[] areas;
    

    private void Start()
    {
        foreach (AreaScript area in areas)
        {
            area.areaDetail.population = 1;
            area.areaDetail.food = Random.Range(-30, 51);
            area.areaDetail.coin = Random.Range(0, 70);
        }
        EventVisualization.Instance.InitVisual();
    }
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
                    if (drageffect.Instance.state == drageffect.State.normal)
                    {
                        HandCard.MyInstance.targetArea = hit.transform.GetComponent<AreaScript>();
                    }             
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
