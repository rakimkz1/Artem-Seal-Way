using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
[Serializable]
public class SaveData
{
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public void Set<T>(string key, T value)
    {
        // Сохраняем любой объект напрямую в JSON
        string jsonValue = JsonConvert.SerializeObject(value);
        data[key] = jsonValue;
    }

    public T Get<T>(string key, out bool isContain, T defaultValue = default)
    {
        if (data.TryGetValue(key, out string jsonValue))
        {
            isContain = true;
            return JsonConvert.DeserializeObject<T>(jsonValue);
        }
        isContain = false;
        return defaultValue;
    }
}
