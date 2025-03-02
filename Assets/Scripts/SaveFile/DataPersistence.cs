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
    //�洢ʵ���˽ӿڵ���
    List<ISaveAndLoadGame> dataPersistenceObjs;
    List<ISaveAndLoadSetting> settingPeresistenceObjs;

    private void Awake()
    {
        if (!isExist)
        {
            Instance = this;
            //��֤���ݹ�������ÿ������������
            DontDestroyOnLoad(gameObject);
            //�����ظ�ʵ����
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
        //    Debug.LogError("��ǰ�����ж�����ݴ洢������");
        //}
    }

    public void NewGame()
    {
        gameData = new GameData();
        //�������غ�Ѱ�Ҽ̳���IDataPersistence�Ľű�
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //����ÿ�����LoadData����
            dataPersistenceObj.Load(gameData);
        }
    }

    public void SaveGame(string FileName)
    {
        //�������غ�Ѱ�Ҽ̳���IDataPersistence�Ľű�
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        gameSettingData.currentSave = FileName;
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //����ÿ�����SaveData����
            dataPersistenceObj.Save(ref gameData);
        }
        SaveTool.Save(FileName, gameData);
        SaveTool.Save(SaveTool.GameSetting_Name, gameSettingData);
    }

    public void LoadGame(string FileName)
    {
        //�������غ�Ѱ�Ҽ̳���IDataPersistence�Ľű�
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        gameData = SaveTool.Load<GameData>(FileName);
        if (gameData == null)
        {
            //Debug.Log("û���ҵ��浵���Զ���ʼ����Ϸ");
            //NewGame();
        }

        Debug.Log(dataPersistenceObjs.Count);
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //Debug.Log("345");
            //����ÿ�����LoadData����
            dataPersistenceObj.Load(gameData);
        }
    }

    public void SaveGameSetting(string FileName)
    {
        settingPeresistenceObjs = FindAllSettingPersistenceObjs();
        //GameSettingSave.Instance.setting.currentSave = FileName;
        foreach (ISaveAndLoadSetting settingPersistenceObj in dataPersistenceObjs)
        {
            //����ÿ�����SaveData����
            settingPersistenceObj.SaveSetting(ref gameSettingData);
        }
        SaveTool.Save(FileName, gameSettingData);
    }

    public void LoadGameSetting(string FileName)
    {
        //�������غ�Ѱ�Ҽ̳���IDataPersistence�Ľű�
        settingPeresistenceObjs = FindAllSettingPersistenceObjs();
        gameSettingData = SaveTool.Load<GameSettingData>(FileName);
        if (gameData == null)
        {
            //Debug.Log("û���ҵ��浵���Զ���ʼ����Ϸ");
            //NewGame();
        }

        //Debug.Log(settingPeresistenceObjs.Count);
        foreach (ISaveAndLoadSetting settingPersistenceObj in dataPersistenceObjs)
        {
            //Debug.Log("345");
            //����ÿ�����LoadData����
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
