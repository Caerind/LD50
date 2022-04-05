using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnableOnAlarmActivated : MonoBehaviour
{
    private Light2D[] lights;
    private bool a = false;

    private void Start()
    {
        lights = GetComponentsInChildren<Light2D>();
        foreach (Light2D l in lights)
        {
            l.enabled = false;
        }
    }

    private void Update()
    {
        if (!a)
        {
            if (GlobalManager.Instance.IsAlarmActivated())
            {
                a = true;

                foreach (Light2D l in lights)
                {
                    l.enabled = true;
                }
            }
        }
    }
}
