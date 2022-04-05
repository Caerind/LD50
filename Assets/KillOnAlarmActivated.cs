using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnAlarmActivated : MonoBehaviour
{
    private void Update()
    {
        if (GlobalManager.Instance.IsAlarmActivated())
        {
            Destroy(gameObject);
        }
    }
}
