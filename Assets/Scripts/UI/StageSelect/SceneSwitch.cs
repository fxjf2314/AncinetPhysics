using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class SceneSwitch : MonoBehaviour
{
    private static SceneSwitch instance;
    public static SceneSwitch MyInstance => instance;
    public string targetSceneName; // 目标场景的名称
    private LoopList LoopList;

    private void Start()
    {
        LoopList = transform.GetComponent<LoopList>();
    }

    
    public void LoadScene()
    {
        foreach (Transform item in transform)
        {
            LoopListItem loopListItem = item.GetComponentInChildren<LoopListItem>();
            if(loopListItem.index == 0)
            {
                string nextScene = item.GetComponentInChildren<TextMeshProUGUI>().text;
                targetSceneName = nextScene;
                GetDataCard(nextScene);
            }
        }
        
    }

    private void GetDataCard(string nextScene)
    {
        foreach(Data data in LoopList.data)
        {
            if(data.name == nextScene)
            {
                ConfirmedCardsManager.MyInstance.StageRound = data.StageRound;
                ConfirmedCardsManager.MyInstance.ConfirmedCards = data.cards;
            }
        }

        
    }
}
