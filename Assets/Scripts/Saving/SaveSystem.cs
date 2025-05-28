using System;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    public const string GameDataFileName = "gameSaveData.json";

    public static void SaveData<T>(T data, string filePath)
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }

    public static T LoadData<T>(string filePath) where T : new()
    {
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);

            try
            {
                return JsonUtility.FromJson<T>(jsonData);
            }
            catch(Exception)
            {
                return new T();
            }
        }
        else return new T();
    }

    public static string DataFilePath(string fileName)
    {
        if(!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        return Path.Combine(Application.persistentDataPath, fileName);
    }
}