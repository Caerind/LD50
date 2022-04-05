using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSceneLoader : MonoBehaviour
{
    [SerializeField] private string bootSceneName = "Boot";

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(bootSceneName).IsValid())
        {
            SceneManager.LoadSceneAsync(bootSceneName, LoadSceneMode.Additive);
        }
    }
}
