using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// twitter�փc�C�[�g�����郁�\�b�h
public class Tweeter : MonoBehaviour
{
    [SerializeField] private List<string> hashtags = new List<string>() { "RobotRocket" };
    private string buildHashtags => string.Join(",", hashtags);

    // URL���쐬����
    private string buildUrl(string text, string tag)
    {
        string esctext = UnityWebRequest.EscapeURL(text);
        string esctag = UnityWebRequest.EscapeURL(tag);
        return "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;
    }

    // �c�C�[�g��ʂ��J��
    public void OpenTwitter(string text)
    {
        string url = buildUrl(text, buildHashtags);

        // Twitter���e��ʂ̋N��
        Application.OpenURL(url);
    }
}
