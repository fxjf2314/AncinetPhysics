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
    //�洢ʵ���˽ӿڵ���
    List<ISaveAndLoadGame> dataPersistenceObjs;

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
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //����ÿ�����SaveData����
            dataPersistenceObj.Save(ref gameData);
        }
        SaveTool.Save(FileName, gameData);
    }

    public void LoadGame(string FileName)
    {
        //�������غ�Ѱ�Ҽ̳���IDataPersistence�Ľű�
        dataPersistenceObjs = FindAllDataPersistenceObjs();
        gameData = SaveTool.Load<GameData>(FileName);
        if (gameData == null)
        {
            Debug.Log("û���ҵ��浵���Զ���ʼ����Ϸ");
            NewGame();
        }

        Debug.Log(dataPersistenceObjs.Count);
        foreach (ISaveAndLoadGame dataPersistenceObj in dataPersistenceObjs)
        {
            //Debug.Log("345");
            //����ÿ�����LoadData����
            dataPersistenceObj.Load(gameData);
        }
    }

    List<ISaveAndLoadGame> FindAllDataPersistenceObjs()
    {
        IEnumerable<ISaveAndLoadGame> dataPersistenceObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveAndLoadGame>();
        return new List<ISaveAndLoadGame>(dataPersistenceObjs);
    }
}
