using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameTimeComponent))]
public class SeasonComponent : MonoBehaviour
{
    public enum Season { Winter, Spring, Summer, Autumn }

    [SerializeField] private Season season = Season.Spring;
    [SerializeField] private float timePerSeason = 365.25f / 4.0f;
    private float seasonTimeAccumulator = 0.0f;

    private GameTimeComponent timeComponent = null;

    private void Awake()
    {
        timeComponent = GetComponent<GameTimeComponent>();
    }

    private void Update()
    {
        seasonTimeAccumulator += timeComponent.GetDeltaTime();
        if (seasonTimeAccumulator >= timePerSeason)
        {
            season = NextSeason(season);
            seasonTimeAccumulator -= timePerSeason;
        }
    }

    public Season GetSeason()
    {
        return season;
    }

    public void SetSeason(Season season)
    {
        this.season = season;
    }

    public static Season NextSeason(Season season)
    {
        return (Season)(((int)season + 1) % 4);
    }
}
