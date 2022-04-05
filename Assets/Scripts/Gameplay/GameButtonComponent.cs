using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonComponent : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors;

    [SerializeField] private bool lightEnabled = true;
    [SerializeField] private new GameObject light;

    private void Awake()
    {
        if (doors.Count > 0)
        {
            light.SetActive(lightEnabled);
        }
        else
        {
            light.SetActive(false);
        }
    }

    public void Press()
    {
        if (doors.Count > 0)
        {
            AudioManager.PlaySound("buttonActive");
        }
        else
        {
            AudioManager.PlaySound("buttonInactive");
        }


        foreach (GameObject go in doors)
        {
            if (go != null)
            {
                AudioManager.PlaySound("doorOpen");
                go.GetComponent<DoorComponent>().OnButtonPressed();
            }
        }
        doors.Clear();

        light.SetActive(false);
    }

    public bool CanBeSelected()
    {
        return true;
    }
}
