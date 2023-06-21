using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public Grid Grid;
    bool _isMoving;
    public float _rollSpeed = 10;

    public void Start ()
    {
        InputHandler.Instance.OnNewInput += Move;
    }

    public void DisableMovement()
    {
        InputHandler.Instance.OnNewInput -= Move;
    }

    private void Move(Vector2 movementInput)
    {   
        Grid.MovePlayer(movementInput);
    }

    public void Assemble(Vector3 dir) 
    {   
        var anchor = transform.position + (Vector3.down + dir) * (Grid.offsetX/2) ;
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
