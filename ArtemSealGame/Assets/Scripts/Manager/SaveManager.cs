using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager
{
    private static string savePath = "Assets/Data/JsonData/save.json";

    public  void Save(SaveData data)
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(savePath, json);
        Debug.Log($"Data saved: {savePath}");
    }

    public SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
        Debug.LogWarning("Data is not founded");
        return new SaveData();
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Data deleted");
        }
    }
}
