using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : Singleton<StateManager>
{
    [SerializeField] private string bootSceneName = "Boot";
    private Scene bootScene;

    private Scene currentScene;
    private int levelIndex = 0;

    private void Awake()
    {
        bootScene = SceneManager.GetSceneByName(bootSceneName);
        if (!bootScene.IsValid())
        {
            SceneManager.LoadScene(bootSceneName);

            bootScene = SceneManager.GetSceneByName(bootSceneName);
            if (!bootScene.IsValid())
            {
                Debug.LogError("Error with Boot scene!");
            }
        }

#if UNITY_EDITOR
        for (int i = 0; i < 20; ++i)
        {
            Scene scene = SceneManager.GetSceneByName("Level-" + i);
            if (scene.IsValid())
            {
                levelIndex = i;
                currentScene = scene;
                break;
            }
        }
#endif

        if (!currentScene.IsValid())
        {
            LoadScene(0);
        }
    }

    public void SwitchToScene(int levelIndex)
    {
        if (currentScene.IsValid())
        {
            var progress = SceneManager.UnloadSceneAsync(currentScene);
            progress.completed += (op) =>
            {
                LoadScene(levelIndex);
            };
        }
        else
        {
            LoadScene(levelIndex);
        }
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    private void LoadScene(int levelIndex)
    {
        string sceneName = "Level-" + levelIndex;
        var progress = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        progress.completed += (op) =>
        {
            this.levelIndex = levelIndex;
            currentScene = SceneManager.GetSceneByName(sceneName);
            if (currentScene.IsValid())
            {
                SceneManager.SetActiveScene(currentScene);
            }
        };
    }

    public Scene GetBootScene()
    {
        return bootScene;
    }
    public Scene GetCurrentScene()
    {
        return currentScene;
    }
}
