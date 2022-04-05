using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GlobalManager : Singleton<GlobalManager>
{
    private GameObject hoveredObject;
    private GameObject selectedObject;

    public int currentLevel = 1;

    [SerializeField] private Image timer;
    [SerializeField] private float timeToEscapeDefault = 30.0f;
    private float timeToEscape;

    private bool alarmActivated = false;
    private float timerEscape;

    private void Awake()
    {
        alarmActivated = false;
        timerEscape = timeToEscapeDefault;
        timeToEscape = timeToEscapeDefault;
        timer.gameObject.SetActive(false);
        timer.fillAmount = Mathf.Clamp01(timerEscape / timeToEscape);
    }

    public bool IsAlarmActivated()
    {
        return alarmActivated;
    }

    public void ActivateAlarm()
    {
        alarmActivated = true;
        timerEscape = timeToEscape;
        timer.gameObject.SetActive(true);
        timer.fillAmount = Mathf.Clamp01(timerEscape / timeToEscape);
        AudioManager.PlaySound("alarm");
    }

    public void ResetLevel()
    {
        alarmActivated = false;
        timerEscape = timeToEscapeDefault;
        timeToEscape = timeToEscapeDefault;
        timer.gameObject.SetActive(false);
        timer.fillAmount = Mathf.Clamp01(timerEscape / timeToEscape);
        AudioManager.StopSound("alarm");
    }

    public void SetTimeToEscapeOverride(float t)
    {
        timeToEscape = t;
    }

    private void Update()
    {
        if (alarmActivated)
        {
            timerEscape -= Time.deltaTime;
            timer.fillAmount = Mathf.Clamp01(timerEscape / timeToEscape);
            if (timerEscape <= 0.0f)
            {
                ResetLevel();
                PlayerManager.Instance.ResetPlayersSpawned();
                StateManager.Instance.SwitchToScene(0);
                AudioManager.PlaySound("loose");
            }
        }
    }

    private void OnGUI()
    {
        /*
        // Controls
        {
            List<string> texts = new List<string>();
            texts.Add("A: Place node");
            texts.Add("W: Place water");

            AutoGUI.Display(Screen.width - 180 - 10, 10, 180, "Controls", texts);
        }
        */

        // Current tool
        /*
        if (GameManagerComponent.IsPlacingNode() || GameManagerComponent.IsPlacingLink() || GameManagerComponent.IsPlacingWater() || GameManagerComponent.HasSelectedObject())
        {
            List<string> texts = new List<string>();
            if (GameManagerComponent.IsPlacingNode())
            {
                texts.Add("Placing node");
            }
            else if (GameManagerComponent.IsPlacingLink())
            {
                texts.Add("Placing link");
            }
            else if (GameManagerComponent.IsPlacingWater())
            {
                texts.Add("Placing water");
            }
            else if (GameManagerComponent.HasSelectedObject())
            {
                LinkComponent link = GameManagerComponent.GetSelectedObject().GetComponent<LinkComponent>();
                if (link != null)
                {
                    texts.Add("Link: " + link.GetID());
                    texts.Add("Water: " + link.water);
                }

                NodeComponent node = GameManagerComponent.GetSelectedObject().GetComponent<NodeComponent>();
                if (node != null)
                {
                    texts.Add("Node: " + node.GetID());
                    texts.Add("Water: " + node.water);
                }
            }

            AutoGUI.Display(10, 10, 180, "Tool", texts);
        }
        */

        /*
        // Infos
        {
            List<string> texts = new List<string>();

            //texts.Add("Mouse: " + GameManagerComponent.GetMousePositionTerrain().ToString());
            texts.Add("Selected: " + ((HasSelectedObject()) ? GetSelectedObject().name : "null"));
            texts.Add("Hovered: " + ((HasHoveredObject()) ? GetHoveredObject().name : "null"));

            AutoGUI.Display(Screen.width - 180 - 10, Screen.height / 2, 180, "Infos", texts);
        }
        */
    }

    private void SelectObjects()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject != null)
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.transform != null && hit.transform.gameObject != null)
            {
                GameObject go = hit.transform.gameObject;

                if (CanObjectBeHovered(go))
                {
                    hoveredObject = go;
                }

                if (CanObjectBeSelected(go))
                {
                    UnselectCurrentObject();
                    SelectObject(go);
                }
            }
        }
    }

    public bool CanObjectBeHovered(GameObject gameObject)
    {
        return false;
    }

    public bool HasHoveredObject()
    {
        return hoveredObject != null;
    }

    public GameObject GetHoveredObject()
    {
        return hoveredObject;
    }

    public bool CanObjectBeSelected(GameObject gameObject)
    {
        return false;
    }

    public bool HasSelectedObject()
    {
        return selectedObject != null;
    }

    GameObject GetSelectedObject()
    {
        return selectedObject;
    }

    public void SelectObject(GameObject gameObject)
    {
        selectedObject = gameObject;
    }

    public void UnselectCurrentObject()
    {
        selectedObject = null;
    }
}
