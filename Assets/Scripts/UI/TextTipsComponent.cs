using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTipsComponent : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private float minTime = 1.0f;
    [SerializeField] private float randomAdditionMaxTime = 1.0f;
    private float timer;

    private int tipIndex = 0;
    [SerializeField] private List<string> tips;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Reset();
    }

    private void Reset()
    {
        timer = minTime + Random.Range(0.0f, randomAdditionMaxTime);
        tipIndex = Random.Range(0, tips.Count - 1);
        text.text = tips[tipIndex];
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            Reset();
        }
    }
}
