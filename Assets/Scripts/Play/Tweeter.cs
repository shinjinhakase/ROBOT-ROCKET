using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// twitterへツイートをするメソッド
public class Tweeter : MonoBehaviour
{
    [SerializeField] private List<string> hashtags = new List<string>() { "RobotRocket" };
    private string buildHashtags => string.Join(",", hashtags);

    // URLを作成する
    private string buildUrl(string text, string tag)
    {
        string esctext = UnityWebRequest.EscapeURL(text);
        string esctag = UnityWebRequest.EscapeURL(tag);
        return "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;
    }

    // ツイート画面を開く
    public void OpenTwitter(string text)
    {
        string url = buildUrl(text, buildHashtags);

        // Twitter投稿画面の起動
        Application.OpenURL(url);
    }
}
