using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingSave : MonoBehaviour,ISaveAndLoadGame
{
    public static GameSettingSave Instance => instance;
    static GameSettingSave instance;

    public GameSettingData setting;
    public bool isExistSave;

    private void Awake()
    {
        instance = this;
        setting = SaveTool.Load<GameSettingData>(SaveTool.GameSetting_Name);
        if (setting == null )
        {
            isExistSave = false;
            Debug.Log("û�д浵");
            setting = new GameSettingData();
        }
        else
        {
            isExistSave = true;
        }
    }


    public void Load<T>(T gameData)
    {
        
    }

    public void Save<T>(ref T gameData)
    {
        SaveTool.Save(SaveTool.GameSetting_Name, setting);
    }
}

[System.Serializable]
public class GameSettingData
{
    public string currentSave;
}