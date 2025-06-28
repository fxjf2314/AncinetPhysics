using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;


public static class SaveTool
{
    public const string Game_Name = "天工";
    public const string NoSaveFile = "NoSave";
    public const string File_Name_01 = "Save01";
    public const string File_Name_02 = "Save02";
    public const string File_Name_03 = "Save03";
    public const string GameSetting_Name = "Setting";

    private static string mSavePath;
    private const int maxFileCount = 3;
    //存档
    public static void Save<T>(string FileName, T data)
    {
        ChangeSavePath(FileName);
        //将存储数据转变为json格式
        string json = JsonUtility.ToJson(data,true);
        try
        {
            string directory = Path.GetDirectoryName(mSavePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //在mSavePath路径处(路径不存在就自动创建路径)创建并将数据写入json文件，如果文件存在就自动覆盖存档
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
    //读档
    public static T Load<T>(string FileName)
    {
        ChangeSavePath(FileName);
        try
        {
            //读取路径的json文件并转为T泛型类型
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
            return default;
        }
    }
    //删除存档
    public static void Delete<T>(string FileName)
    {
        ChangeSavePath(FileName);
        try
        {
            //删除对应路径存档（文件不存在不会有错误提示）
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

    //在删除文件后自动更改存档文件名，传入删除的存档文件号
    public static void ChangeFileName(int index)
    {
        for (int i = index; i < maxFileCount; i++)
        {
            // 定义原始文件路径和新文件名
            string originalFilePath = Application.persistentDataPath + "/" + Game_Name + "/" + "Save0" + (i+1) + ".json"; ; // 替换为你的JSON文件路径
            string newFileName = "Save0" + (i) + ".json"; // 新文件名
            string newFilePath = Path.Combine(Path.GetDirectoryName(originalFilePath), newFileName);

            // 检查原始文件是否存在
            if (File.Exists(originalFilePath))
            {
                // 更改文件名
                File.Move(originalFilePath, newFilePath);
                Console.WriteLine($"文件已重命名为：{newFilePath}");
            }
            else
            {
                Console.WriteLine("原始文件不存在，请检查路径！");
            }
        }
        GameSettingData setting = Load<GameSettingData>(GameSetting_Name);
        if (File.Exists(Application.persistentDataPath + "/" + Game_Name + "/" + File_Name_01 + ".json"))
        {
            setting.currentSave = File_Name_01;
        }
        else
        {
            setting.currentSave = NoSaveFile;
        }
    }

}

public interface ISaveAndLoadGame
{
    void Save(ref GameData gameData);
    void Load(GameData gameData);
}