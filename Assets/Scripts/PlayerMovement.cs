using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public float speed;
    bool _isMoving;

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
        if (playerInput != null)
            playerInput.Disable();
        playerInput.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {   
        Vector2 movementInput = context.ReadValue<Vector2>();
        if(_isMoving) return;
        Grid.Instance.MovePlayer(movementInput);
    }

    public float _rollSpeed = 5;

    public void Assemble(Vector3 dir) 
    {   
        var anchor = transform.position + (Vector3.down + dir) * (Grid.Instance.offsetX/2) ;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }
    
    private IEnumerator Roll(Vector3 anchor, Vector3 axis) {
        _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++) {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        _isMoving = false;
    }
}
