using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private int levelIndex = 1;

    public void SwitchLevel()
    {
        StateManager.Instance.SwitchToScene(levelIndex);
    }
}
