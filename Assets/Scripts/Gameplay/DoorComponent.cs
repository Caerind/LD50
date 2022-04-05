using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    public float timer = 3.0f;
    public GameObject sprite;
    public new GameObject light;

    private void Awake()
    {
        sprite.SetActive(true);
        light.SetActive(false);
    }

    public void OnButtonPressed()
    {
        sprite.SetActive(false);
        light.SetActive(true);

        Destroy(gameObject, timer);
    }
}
