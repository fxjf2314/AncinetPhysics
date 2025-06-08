using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToHome : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, "是否返回标题");
        });
    }
}
