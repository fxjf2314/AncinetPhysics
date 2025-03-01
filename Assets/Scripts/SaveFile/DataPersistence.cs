using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static bool isExist = false;
    public static DataPersistence Instance { get; private set; }
    GameData gameData;
    public GameData GameData { get { return gameData; } }
    GameSettingData gameSettingData;
    public GameSettingData GameSettingData { get {return gameSettingData; } }
    //存储实现了接口的类
    List<ISaveAndLoadGame> dataPersistenceObjs;
    List<ISaveAndLoadSetting> settingPeresistenceObjs;

    private void Awake()
    {
        if (!isExist)
        {
            Instance = this;
            //保证数据管理器在每个场景都存在
            DontDestroyOnLoad(gameObject);
            //避免重复实例化
            isExist = true;
        }
        else
        {
            Destroy(gameObject);
        }
        gameData = new GameData();
        gameSettingData = new GameSettingData();
        //gameData = SaveTool.Load<GameData>(SaveTool.File_Name_01);
        //if(Instance != null)
        //{
        //    Debug.LogError("当前场景有多个数据存储管理器");
        //}
    }

    public void NewGame()
    {
        gameData = new GameData();
        //场景加载后寻找继承了IDataPersistence的脚本
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //调用每个类的LoadData方法
            dataPersistenceObj.Load(gameData);
        }
    }

    public void SaveGame(string FileName)
    {
        //场景加载后寻找继承了IDataPersistence的脚本
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        gameSettingData.currentSave = FileName;
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //调用每个类的SaveData方法
            dataPersistenceObj.Save(ref gameData);
        }
        SaveTool.Save(FileName, gameData);
        SaveTool.Save(SaveTool.GameSetting_Name, gameSettingData);
    }

    public void LoadGame(string FileName)
    {
        //场景加载后寻找继承了IDataPersistence的脚本
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        gameData = SaveTool.Load<GameData>(FileName);
        if (gameData == null)
        {
            //Debug.Log("没有找到存档，自动开始新游戏");
            //NewGame();
        }

        Debug.Log(dataPersistenceObjs.Count);
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //Debug.Log("345");
            //调用每个类的LoadData方法
            dataPersistenceObj.Load(gameData);
        }
    }

    public void SaveGameSetting(string FileName)
    {
        settingPeresistenceObjs = FindAllSettingPersistenceObjs();
        //GameSettingSave.Instance.setting.currentSave = FileName;
        foreach (ISaveAndLoadSetting settingPersistenceObj in dataPersistenceObjs)
        {
            //调用每个类的SaveData方法
            settingPersistenceObj.SaveSetting(ref gameSettingData);
        }
        SaveTool.Save(FileName, gameSettingData);
    }

    public void LoadGameSetting(string FileName)
    {
        //场景加载后寻找继承了IDataPersistence的脚本
        settingPeresistenceObjs = FindAllSettingPersistenceObjs();
        gameSettingData = SaveTool.Load<GameSettingData>(FileName);
        if (gameData == null)
        {
            //Debug.Log("没有找到存档，自动开始新游戏");
            //NewGame();
        }

        //Debug.Log(settingPeresistenceObjs.Count);
        foreach (ISaveAndLoadSetting settingPersistenceObj in dataPersistenceObjs)
        {
            //Debug.Log("345");
            //调用每个类的LoadData方法
            settingPersistenceObj.LoadSetting(gameSettingData);
        }
    }

    List<ISaveAndLoadSetting> FindAllSettingPersistenceObjs()
    {
        IEnumerable<ISaveAndLoadSetting> settingPersistenceObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveAndLoadSetting>();
        return new List<ISaveAndLoadSetting>(settingPersistenceObjs);
    }

    List<ISaveAndLoadGame> FindAllDataPersistenceObjs()
    {
        IEnumerable<ISaveAndLoadGame> dataPersistenceObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveAndLoadGame>();
        return new List<ISaveAndLoadGame>(dataPersistenceObjs);
    }
}
