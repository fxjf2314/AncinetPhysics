using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePanelManager : LoadPanelManger
{
    // Start is called before the first frame update
    new  void Awake()
    {
        base.Awake();
        ButtonAddListener();
    }

    protected override void ButtonAddListener()
    {
        //Debug.Log(111);
        save1.onClick.AddListener(() =>
        {
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_01);
            UpdataPanel();
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
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_02);
            UpdataPanel();
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
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_03);
            UpdataPanel();
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
}
