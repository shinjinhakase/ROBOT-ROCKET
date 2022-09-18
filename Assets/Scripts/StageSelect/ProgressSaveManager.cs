using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSaveManager
{
    private string keyStr = "ProgressData";

    public void Save(ProgressData saveData)
    {
        // �Z�[�u�f�[�^�`���ϊ�
        string jsonSaveDataStr = JsonUtility.ToJson(saveData);

        // �ۑ�
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
