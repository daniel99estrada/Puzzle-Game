using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> OnNewInput;

    bool _isMoving;

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
        if (_isMoving) return;

        Vector2 movementInput = context.ReadValue<Vector2>();

        OnNewInput?.Invoke(movementInput);   

        StartCoroutine(DisableInput());     
    }

    private IEnumerator DisableInput()
    {
        // Disable movement
        _isMoving = true;

        // Wait for half a second
        yield return new WaitForSeconds(0.2f);

        // Enable movement
        _isMoving = false;
    }
}
