using UnityEngine;
using UnityEditor;

public class QuitComponent : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#endif // UNIT_EDITOR

        Application.Quit();
    }
}
