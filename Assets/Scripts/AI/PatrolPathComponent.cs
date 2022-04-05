using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPathComponent : MonoBehaviour
{
    [SerializeField] private GameObject aiPrefab;
    private bool alarmed;

    private void Start()
    {
        GameObject ai = Instantiate(aiPrefab);
        ai.transform.position = transform.GetChild(0).position;

        AIComponent aiComp = ai.GetComponent<AIComponent>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            aiComp.AddPathNode(transform.GetChild(i).position);
        }
    }

    private void Update()
    {
        if (!alarmed && GlobalManager.Instance.IsAlarmActivated())
        {
            alarmed = true;
            GameObject ai = Instantiate(aiPrefab);
            ai.transform.position = transform.GetChild(0).position;
            ai.transform.position = transform.GetChild(2).position;
        }
    }
}
