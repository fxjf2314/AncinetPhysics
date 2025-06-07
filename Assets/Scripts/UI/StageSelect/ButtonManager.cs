using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button backToHome;
    [SerializeField]
    private Button startGame;
    [SerializeField]
    private GameObject area;
    private void Start()
    {
        startGame.onClick.AddListener(() =>
        {
            area.transform.GetComponent<SceneSwitch>().LoadScene();
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToGame, "是否要进入" + area.transform.GetComponent<SceneSwitch>().targetSceneName + "关卡");
        });

        backToHome.onClick.AddListener(() =>
        {
            TipPanelManager.Instance.OpenPanel(BtnFunction.goToTitle, "是否返回标题");
        });
    }
}
