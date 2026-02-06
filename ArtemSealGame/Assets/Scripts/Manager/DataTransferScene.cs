using UnityEngine;
using System.Collections.Generic;
using System;
using Zenject;
using BattleField;

public class DataTransferScene : MonoBehaviour
{
    public Dictionary<string, object> data = new Dictionary<string, object>();
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    [Inject]
    private void Construct(BattleSceneSetting battleSceneSetting)
    {
    }
    public object Get<T>(string key)
    {
        if(data.TryGetValue(key, out object obj))
        {
            return obj;
        }
        return default;
    }
    public void Set<T>(string key, T value)
    {
        data[key] = value;
    }

    public void Remove(string key)
    {
        data.Remove(key);
    }
}
