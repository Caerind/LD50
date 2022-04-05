using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditionalSceneLoader : MonoBehaviour
{
    [SerializeField] private List<string> additionalScenes = new List<string>();
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        foreach (string sceneName in additionalScenes)
        {
            if (!SceneManager.GetSceneByName(sceneName).IsValid())
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }

        if (currentSceneName == "GameManager")
        {
            string levelName = "Level-" + StateManager.Instance.GetLevelIndex();
            SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (current.name == "GameManager")
        {
            string levelName = "Level-" + StateManager.Instance.GetLevelIndex();
            SceneManager.UnloadSceneAsync(levelName);
        }

        if (current.name == currentSceneName)
        {
            foreach (string sceneName in additionalScenes)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}
