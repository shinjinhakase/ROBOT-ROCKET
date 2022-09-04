using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesProgressSaveManager
{
    private string keyStr = "StagesProgressSaveData";

    public void Save(StagesProgressSaveData saveData)
    {
        // セーブデータ形式変換
        string jsonSaveDataStr = JsonUtility.ToJson(saveData);

        // 保存
        PlayerPrefs.SetString(keyStr, jsonSaveDataStr);
        PlayerPrefs.Save();
    }

    public StagesProgressSaveData Load()
    {
        string jsonSaveDataStr = PlayerPrefs.GetString(keyStr);
        Debug.Log($"jsonSaveDataStr -> {jsonSaveDataStr}");

        StagesProgressSaveData saveData 
            = JsonUtility.FromJson<StagesProgressSaveData>(jsonSaveDataStr);
        return saveData;
    }
}
