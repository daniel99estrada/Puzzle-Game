using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using GG.Infrastructure.Utils.Swipe;

public class InputHandler : MonoBehaviour
{   
    public SwipeListener swipeListener;

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
        swipeListener.OnSwipe.AddListener(OnSwipe);

        // playerInput = new PlayerInput();
        // playerInput.Input.Move.performed += Move;
        // playerInput.Enable();
    }

    private void OnSwipe(string swipe)
    {   
        if (isMoving) return;

        Vector2 movementInput = Vector2.zero;
        
        switch (swipe)
        {
            case "UpRight":
                movementInput = new Vector2(1f, 0f);
                break;
            case "UpLeft":
                movementInput = new Vector2(0f, 1f);
                break;
            case "DownRight":
                movementInput = new Vector2(0f, -1f);
                break;
            case "DownLeft":
                movementInput = new Vector2(-1f, 0f);
                break;
            default:
                break;
        }

        OnNewInput?.Invoke(movementInput);   

    }

    private void OnDisable()
    {   
        swipeListener.OnSwipe.RemoveListener(OnSwipe);

        // if (playerInput != null)
        //     playerInput.Disable();
    }

    // private void Move(InputAction.CallbackContext context)
    // {
    //     Vector2 movementInput = context.ReadValue<Vector2>();
    //     if (isMoving) return;

    //     OnNewInput?.Invoke(movementInput);        
    // }
}
