using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitch : MonoBehaviour
{
    public static SceneSwitch Instance { get; private set; }

    [Header("Scene Settings")]
    //[SerializeField] private string defaultSceneName = "MainMenu";

    private LoopList levelSelectionList;



    private string targetSceneName;
    
    public string TargetSceneName => targetSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelSelectionList = GetComponent<LoopList>();
        if (levelSelectionList == null)
        {
            Debug.LogError("LoopList component not found on SceneSwitch object!");
        }
    }

    

    public bool TryGetSelectedLevel(out Data selectedLevel)
    {
        selectedLevel = new Data();

        

        foreach (Transform item in transform)
        {
            var loopListItem = item.GetComponentInChildren<LoopListItem>();
            if (loopListItem != null && loopListItem.index == 0)
            {
                string levelName = item.GetComponentInChildren<TextMeshProUGUI>().text;
                targetSceneName = levelName;

                foreach (Data data in levelSelectionList.data)
                {
                    if (data.name == levelName)
                    {
                        selectedLevel = data;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void PrepareLevelData(Data levelData)
    {
        if (ConfirmedCardsManager.MyInstance != null)
        {
            ConfirmedCardsManager.MyInstance.StageRound = levelData.StageRound;
            ConfirmedCardsManager.MyInstance.ConfirmedCards = levelData.cards;
            ConfirmedCardsManager.MyInstance.confirmStageType = levelData.stageType;
            ConfirmedCardsManager.MyInstance.confirmLevelNum = levelData.stageLevel;
        }
        else
        {
            Debug.LogError("ConfirmedCardsManager instance not found!");
        }
    }

    private void ShowLevelLockedMessage(Data levelData)
    {
        //Debug.Log($"Level {levelData.name} is locked! {levelData.unlockHint}");
        // 可以在这里调用UI系统显示提示
    }

    public void LoadScene()
    {
        
        
        SceneManager.LoadScene("wwwww");
        
        
    }
}
