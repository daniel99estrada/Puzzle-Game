using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public float speed;

    private PlayerInput playerInput;

    public static PlayerMovement Instance { get; private set; }
    
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
        playerInput.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {   
        Vector2 movementInput = context.ReadValue<Vector2>();

        Grid.Instance.MovePlayer(movementInput);
    }
}
