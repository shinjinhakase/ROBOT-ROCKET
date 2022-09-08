using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSaveManager
{
    private string keyStr = "ProgressData";

    public void Save(ProgressData saveData)
    {
        // セーブデータ形式変換
        string jsonSaveDataStr = JsonUtility.ToJson(saveData);

        // 保存
        PlayerPrefs.SetString(keyStr, jsonSaveDataStr);
        PlayerPrefs.Save();
    }

    public ProgressData Load()
    {
        string jsonSaveDataStr = PlayerPrefs.GetString(keyStr);
        Debug.Log($"jsonSaveDataStr -> {jsonSaveDataStr}");

        ProgressData saveData 
            = JsonUtility.FromJson<ProgressData>(jsonSaveDataStr);
        return saveData;
    }
}
