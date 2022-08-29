using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

// PlayerPref�܂��̓t�@�C���Ƀf�[�^��ۑ��ł���N���X
// ���̃N���X���p������ƁASave()���\�b�h���ĂԂ��Ƃ�public�܂���[SerializeField]�ɂ����t�B�[���h��JSON�ɕۑ��ł���
public abstract class SavableSingletonBase<T> where T : SavableSingletonBase<T>, new()
{
    private static T _instance;
    private bool _isLoaded;

    // �V���A���C�Y����JSON��PlyerPref�ɕۑ����邩�A�t�@�C���ɕۑ����邩�̐ݒ�
    protected virtual bool IsSaveToPlayerPref => true;

    public static T Instance
    {
        get
        {
            if (null != _instance) return _instance;

            string json;
            _instance = new T();
            // �C���X�^���X�������Ƀf�[�^���������[�h����
            if (_instance.IsSaveToPlayerPref)
            {
                json = PlayerPrefs.GetString(SaveKey);
            }
            else
            {
                json = File.Exists(SavePath) ? File.ReadAllText(SavePath) : "";
            }

            if(string.IsNullOrEmpty(json) || !LoadFromJson(json))
            {
                _instance._isLoaded = true;
            }

            return _instance;
        }
    }

    // �f�[�^��JSON�ɃV���A���C�Y
    protected virtual string SerializedData => JsonUtility.ToJson(this);

    private static string SavePath => $"{Application.persistentDataPath}/{SaveKey}";

    private static string SaveKey
    {
        get
        {
            // �N���X���̃n�b�V���l�𐶐����Ă���
            var provider = new SHA1CryptoServiceProvider();
            var hash = provider.ComputeHash(
                System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName ?? throw new InvalidOperationException()));
            return BitConverter.ToString(hash);
        }
    }

    // JSON�f�[�^����f�[�^�𕜌�����
    public static bool LoadFromJson(string json)
    {
        try
        {
            _instance = JsonUtility.FromJson<T>(json);
            _instance._isLoaded = true;
            return true;
        }
        catch(Exception e)
        {
            Debug.LogWarning(e.ToString());
            return false;
        }
    }

    // �f�[�^��ۑ�����B
    public void Save()
    {
        if (!_isLoaded) return;

        if (IsSaveToPlayerPref)
        {
            PlayerPrefs.SetString(SaveKey, SerializedData);
            PlayerPrefs.Save();
        }
        else
        {
            var path = SavePath;
            File.WriteAllText(path, SerializedData);
#if UNITY_IOS
            // iOS�Ńf�[�^��iCloud�Ƀo�b�N�A�b�v�����Ȃ��ݒ�
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
        }
    }

    // �f�[�^�����Z�b�g����
    public void Reset()
    {
        _instance = null;
    }

    // �f�[�^���폜����
    public void Delete()
    {
        if (IsSaveToPlayerPref)
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
        else
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }

        _instance = null;
    }
}
