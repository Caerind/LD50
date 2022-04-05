using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsComponent : MonoBehaviour
{
    private void Start()
    {
        Vector3[] spawnPoints = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            spawnPoints[i] = transform.GetChild(i).position;
        }
        PlayerManager.Instance.SetSpawnPoints(spawnPoints);
    }
}
