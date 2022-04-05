using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float timeToEscapeOverride = 30.0f;

    private void Awake()
    {
        GlobalManager.Instance.SetTimeToEscapeOverride(timeToEscapeOverride);
    }
}
