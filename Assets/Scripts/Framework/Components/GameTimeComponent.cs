using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeComponent : MonoBehaviour
{
    [SerializeField] private float timeScale = 1.0f;
    private float timeSinceBeginning = 0.0f;
    private bool playing = true;

    public float GetDeltaTime()
    {
        if (playing)
        {
            return timeScale * Time.deltaTime;
        }
        else
        {
            return 0.0f;
        }
    }

    public void Play()
    {
        playing = true;
    }

    public void Pause()
    {
        playing = false;
    }

    public bool IsPlaying()
    {
        return playing;
    }

    public bool IsPaused()
    {
        return playing;
    }

    public float GetTimeSinceBeginning()
    {
        return timeSinceBeginning;
    }

    public void ResetTimeSinceBeginning()
    {
        timeSinceBeginning = 0.0f;
    }

    public float GetTimeScale()
    {
        return timeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    private void Update()
    {
        timeSinceBeginning += GetDeltaTime();
    }
}
