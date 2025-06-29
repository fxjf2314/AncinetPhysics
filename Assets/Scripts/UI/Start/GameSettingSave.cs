using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingSave : MonoBehaviour,ISaveAndLoadSetting
{
    public static GameSettingSave Instance => instance;
    static GameSettingSave instance;

    public GameSettingData setting;
    public bool isExistSave;

    private void Awake()
    {
        instance = this;
        UpdateCurrentSave();
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCurrentSave()
    {
        setting = SaveTool.Load<GameSettingData>(SaveTool.GameSetting_Name);
        if (setting == null || setting.currentSave == SaveTool.NoSaveFile)
        {
            isExistSave = false;
            Debug.Log("û�д浵");
            setting = new GameSettingData();
        }
        else
        {
            if (setting.currentSave != null)
            {
                isExistSave = true;
            }
        }
    }

    public void SaveSetting(ref GameSettingData gameSettingData)
    {
        gameSettingData = setting;
    }

    public void LoadSetting(GameSettingData gameSettingData)
    {
        setting = gameSettingData;
    }

    //public void UpdateGameSettingData(string saveName)
    //{
    //    setting.currentSave = saveName;
    //    DataPersistence.Instance.SaveGameSetting(SaveTool.GameSetting_Name);
    //}
}

[System.Serializable]
public class GameSettingData
{
    public string currentSave;
}