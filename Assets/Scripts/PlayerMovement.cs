using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public Grid Grid;
    bool _isMoving;
    bool isMoving = false;
    private float _rollSpeed = 10;
    public int speed = 300;

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
        var anchor = transform.position + (Vector3.down + dir) * (1.1f/2) ;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));

        // StartCoroutine(Roll(Vector3.back));
    }

    // IEnumerator Roll(Vector3 dir)
    // {
    //     isMoving = true;
    //     float remainingAngle = 90;
    //     Vector3 rotationCenter = (transform.position + dir / 2 + Vector3.down / 2);
    //     Vector3 rotationAxis= Vector3.Cross(Vector3.up, dir);

    //     while(remainingAngle > 0){
    //         float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
    //         transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
    //         remainingAngle -= rotationAngle;
    //         yield return null;
    //     }   
    //     isMoving = false;  
    // }
    
    private IEnumerator Roll(Vector3 anchor, Vector3 axis) {
        _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++) {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        _isMoving = false;
    }
}
