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
        save1.onClick.AddListener(() =>
        {
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_01);
            //GameSettingSave.Instance.UpdateGameSettingData(SaveTool.File_Name_01);
        });
        save2.onClick.AddListener(() =>
        {
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_02);
            //GameSettingSave.Instance.UpdateGameSettingData(SaveTool.File_Name_02);
        });
        save3.onClick.AddListener(() =>
        {
            DataPersistence.Instance.SaveGame(SaveTool.File_Name_03);
            //GameSettingSave.Instance.UpdateGameSettingData(SaveTool.File_Name_03);
        });
    }
}
