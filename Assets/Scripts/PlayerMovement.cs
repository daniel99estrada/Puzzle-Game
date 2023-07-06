using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    public Grid Grid;
    private float _rollSpeed = 10;

    public void Start ()
    {   
        InputHandler.Instance.OnNewInput += Move;

        rb = GetComponent<Rigidbody>();
    }

    public void DisableMovement()
    {
        InputHandler.Instance.OnNewInput -= Move;
    }
    
    private void OnDestroy()
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
    }

    public void WinningRoll() 
    {   
        // AddForceAndRotate();

        // Vector3 dir = new Vector3(0,1,0);      
        // var anchor = transform.position + (Vector3.up + dir) * 2;
        // var axis = Vector3.Cross(Vector3.up, dir);
        // StartCoroutine(Roll(anchor, axis));
    }
    
    private IEnumerator Roll(Vector3 anchor, Vector3 axis) {
        
        InputHandler.Instance.isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++) {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        InputHandler.Instance.isMoving = false;
    }

    public float forceMagnitude = 1f;
    public float rotationSpeed = 300f;
    private Rigidbody rb;


    public void AddForceAndRotate()
    {
        // Add force in the up direction
        // rb.AddForce(Vector3.up * forceMagnitude, ForceMode.Impulse);

        // Rotate the game object 360 degrees
        StartCoroutine(RotateObject());
    }

    private IEnumerator RotateObject()
    {
        float remainingAngle = 9f;

        while (remainingAngle > 0)
        {
            float rotationAngle = rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationAngle, 0, 0);
            remainingAngle -= rotationAngle;
            yield return null;
        }
    }
}
