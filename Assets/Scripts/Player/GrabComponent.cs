using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabComponent : MonoBehaviour
{
    private PlayerController playerController = null;

    private void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StrongboxComponent strongbox = collision.gameObject.GetComponent<StrongboxComponent>();
        if (strongbox != null)
        {
            playerController.SetStrongbox(strongbox);
            return;
        }

        BagComponent bag = collision.gameObject.GetComponent<BagComponent>();
        if (bag != null)
        {
            playerController.SetBag(bag);
            return;
        }

        GameButtonComponent button = collision.gameObject.GetComponent<GameButtonComponent>();
        if (button != null && button.CanBeSelected())
        {
            playerController.SetButton(button);
            return;
        }


        if (collision.gameObject.GetComponent<ZoneTrigger>() != null)
        {
            if (StateManager.Instance.GetLevelIndex() == 0)
            {
                AudioManager.StopSound("win");
                AudioManager.StopSound("loose");

                GlobalManager.Instance.ResetLevel();
                PlayerManager.Instance.ResetPlayersSpawned();
                StateManager.Instance.SwitchToScene(GlobalManager.Instance.currentLevel);
            }
            else
            {
                PlayerController c = GetComponentInParent<PlayerController>();
                c.isInZone = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<StrongboxComponent>() != null)
        {
            playerController.SetStrongbox(null);
            return;
        }
        if (collision.gameObject.GetComponent<BagComponent>() != null)
        {
            playerController.SetBag(null);
            return;
        }
        if (collision.gameObject.GetComponent<GameButtonComponent>() != null)
        {
            playerController.SetButton(null);
            return;
        }

        if (collision.gameObject.GetComponent<ZoneTrigger>() != null)
        {
            if (StateManager.Instance.GetLevelIndex() != 0)
            {
                PlayerController c = GetComponentInParent<PlayerController>();
                c.isInZone = false;
            }
        }
    }
}
