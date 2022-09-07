using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void OnLoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
