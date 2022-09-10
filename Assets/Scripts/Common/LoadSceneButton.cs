using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public void OnLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
