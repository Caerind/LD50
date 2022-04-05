using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerController playerController = null;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerController.moveInput = Vector2.zero;
        }
        else
        {
            playerController.moveInput = context.ReadValue<Vector2>();
        }
    }

    public void MainButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.actionPressed = true;
        }
        else
        {
            playerController.actionPressed = false;
        }
    }
}
