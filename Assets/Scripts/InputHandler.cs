using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> OnNewInput;

    public bool isMoving;

    private PlayerInput playerInput;

    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Input.Move.performed += Move;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        if (isMoving) return;

        OnNewInput?.Invoke(movementInput);        
    }
}
