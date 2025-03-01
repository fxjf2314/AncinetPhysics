using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveTool
{
    public const string Game_Name = "�칤";
    public const string File_Name_01 = "Save01";
    public const string File_Name_02 = "Save02";
    public const string File_Name_03 = "Save03";
    public const string GameSetting_Name = "Setting";

    private static string mSavePath;

    //�浵
    public static void Save<T>(string FileName, T data)
    {
        ChangeSavePath(FileName);
        //���洢����ת��Ϊjson��ʽ
        string json = JsonUtility.ToJson(data, true);
        try
        {
            string directory = Path.GetDirectoryName(mSavePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //��mSavePath·����(·�������ھ��Զ�����·��)������������д��json�ļ�������ļ����ھ��Զ����Ǵ浵
            File.WriteAllText(mSavePath, json);
#if UNITY_EDITOR
            Debug.Log("Save Data to" + mSavePath);
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError(exception);
            Debug.LogError("Failed to save file");
#endif
        }
    }
    //����
    public static T Load<T>(string FileName)
    {
        ChangeSavePath(FileName);
        try
        {
            //��ȡ·����json�ļ���תΪT��������
            string json = File.ReadAllText(mSavePath);
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogWarning(exception);
            Debug.LogWarning("Failed to load file");
            return default;
#endif
        }
    }
    //ɾ���浵
    public static void Delete<T>(string FileName)
    {
        ChangeSavePath(FileName);
        try
        {
            //ɾ����Ӧ·���浵���ļ������ڲ����д�����ʾ��
            File.Delete(mSavePath);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError(exception);
            Debug.LogError("Failed to delete file");
#endif
        }
    }

    static void ChangeSavePath(string FileName)
    {
        mSavePath = Application.persistentDataPath + "/" + Game_Name + "/" + FileName + ".json";
    }

}

public interface ISaveAndLoadGame
{
    void Save(ref GameData gameData);
    void Load(GameData gameData);
}