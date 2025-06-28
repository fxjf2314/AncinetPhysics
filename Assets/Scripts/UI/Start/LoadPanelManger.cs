using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelManger : MonoBehaviour
{
    [SerializeField]
    protected Button save1, save2, save3;
    [SerializeField]
    protected GameObject noSave1, noSave2, noSave3;
    [SerializeField]
    protected Button delete1, delete2, delete3;
    [SerializeField]
    protected Button renameBtn1, renameBtn2, renameBtn3;
    protected Dictionary<Button, GameObject> saves = new Dictionary<Button, GameObject>();
    protected SaveTipPanelManager saveTipPanelManager;

    protected void Awake()
    {
        //Debug.Log(1345534);
        saves.Add(save1, noSave1);
        saves.Add(save2, noSave2);
        saves.Add(save3, noSave3);
        InitSaveTipPanelManager();
        ButtonAddListener();
        UpdataPanel();
    }

    private void OnEnable()
    {
        UpdataPanel();
    }

    protected void InitSaveTipPanelManager()
    {
        saveTipPanelManager = transform.Find("SaveTipPanelManager").GetComponent<SaveTipPanelManager>();
        saveTipPanelManager.AfterDelete += UpdataPanel;
    }

    protected virtual void ButtonAddListener()
    {
        save1.onClick.AddListener(() =>
        {
            Debug.Log("跳转到存档1");
            Transition.Instance.LoadSceneWithTransition("wwwww", SaveTool.File_Name_01);
            //DataPersistence.Instance.LoadGame(SaveTool.File_Name_01);
        });
        delete1.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(false, 1);
        });
        renameBtn1.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(true, 1);
        });

        save2.onClick.AddListener(() =>
        {
            Debug.Log("跳转到存档2");
            Transition.Instance.LoadSceneWithTransition("wwwww", SaveTool.File_Name_02);
            //DataPersistence.Instance.LoadGame(SaveTool.File_Name_02);
        });
        delete2.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(false, 2);
        });
        renameBtn2.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(true, 2);
        });

        save3.onClick.AddListener(() =>
        {
            Debug.Log("跳转到存档3");
            Transition.Instance.LoadSceneWithTransition("wwwww", SaveTool.File_Name_03);
            //DataPersistence.Instance.LoadGame(SaveTool.File_Name_03);
        });
        delete3.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(false, 3);
        });
        renameBtn3.onClick.AddListener(() =>
        {
            saveTipPanelManager.OpenRenameOrDeletePanel(true, 3);
        });
    }

    protected void UpdateText(Transform saveGrid, GameData gameData)
    {
        TextMeshProUGUI name = TransformFind.TransformFindChild(saveGrid, "Name").GetComponent<TextMeshProUGUI>();
        name.text = gameData.saveFileName;
        TextMeshProUGUI round = TransformFind.TransformFindChild(saveGrid,"Round").GetComponent<TextMeshProUGUI>();
        round.text = "回合数：" + gameData.round;
        TextMeshProUGUI people = TransformFind.TransformFindChild(saveGrid, "People").GetComponent<TextMeshProUGUI>();
        people.text = "总人口：" + gameData.people;
        TextMeshProUGUI harvest = TransformFind.TransformFindChild(saveGrid, "Harvest").GetComponent<TextMeshProUGUI>();
        harvest.text = "总收成：" + gameData.harvest;
        TextMeshProUGUI output = TransformFind.TransformFindChild(saveGrid, "Output").GetComponent<TextMeshProUGUI>();
        output.text = "总产出：" + gameData.output;
        LayoutRebuilder.ForceRebuildLayoutImmediate(name.rectTransform);
    }

    protected void UpdateSaveShow(Button save, GameData gameData)
    {
        if(gameData == null)
        {
            saves[save].SetActive(true);
        }
        else
        {
            saves[save].SetActive(false);
            UpdateText(save.transform, gameData);
        }
    }

    protected void UpdataPanel()
    {
        UpdateSaveShow(save1, SaveTool.Load<GameData>(SaveTool.File_Name_01));
        UpdateSaveShow(save2, SaveTool.Load<GameData>(SaveTool.File_Name_02));
        UpdateSaveShow(save3, SaveTool.Load<GameData>(SaveTool.File_Name_03));
    }

}
