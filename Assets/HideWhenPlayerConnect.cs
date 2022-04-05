using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenPlayerConnect : MonoBehaviour
{
    public int playerCount = 1;

    private void Update()
    {
        if (PlayerManager.Instance.GetPlayerCount() == playerCount)
        {
            gameObject.SetActive(false);
        }
    }
}
