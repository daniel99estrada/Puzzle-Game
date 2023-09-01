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

    // public float rotationSpeed = 90f; // Degrees per second

    private bool isRotating = false;
    private float WinningRollSpeed = 360;
    public AnimationCurve speedCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.1f, 1f),
        new Keyframe(0.2f, 1f),
        new Keyframe(0.3f, 0.1f),
        new Keyframe(0.4f, 0.1f),
        new Keyframe(0.6f, 0.6f),
        new Keyframe(0.8f, 0.4f),
        new Keyframe(0.9f, 0.2f),
        new Keyframe(1.0f, 0.1f)
    );
IEnumerator SpinObjectCoroutine()
    {
        isRotating = true;

        float startRotation = transform.rotation.eulerAngles.y;
        float targetRotation = startRotation + 360f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / (WinningRollSpeed * speedCurve.Evaluate(t) / 360f);

            float currentRotation = Mathf.Lerp(startRotation, targetRotation, t);
            transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);

            yield return null;
        }

        isRotating = false;
    }
    public void WinningRoll() 
    {   
        StartCoroutine(SpinObjectCoroutine());
        StartCoroutine(MoveUp());
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

public AnimationCurve movementCurve = new AnimationCurve(
    new Keyframe(0f, 1f),
    new Keyframe(0.1f, 1f),
    new Keyframe(0.2f, 1f),
    new Keyframe(0.3f, 0.7f),
    new Keyframe(0.4f, 0.3f),
    new Keyframe(0.6f, 0.7f),
    new Keyframe(0.7f, 1f),
    new Keyframe(0.8f, 1f),
    new Keyframe(0.9f, 1f),
    new Keyframe(1.0f, 1f)
); 
    public float movementHeight = 2.0f; // Total height of the movement
    public float movementSpeed = 1.0f; // Speed of the movement

    private Vector3 startPosition;
    private bool isMovingUp = true; // Flag to track the direction of movement

    private IEnumerator MoveUp()
    {
        startPosition = transform.position;
        float journeyLength = movementHeight * 2; // Total distance traveled in the movement
        float startTime = Time.time;
        float fractionOfJourney = 0f;

        while (fractionOfJourney <= 1f)
        {
            float journeyTime = Time.time - startTime;
            fractionOfJourney = journeyTime / movementSpeed;

            if (fractionOfJourney > 1f)
            {
                // Ensure that the object reaches the end position
                fractionOfJourney = 1f;
            }

            float yOffset = movementCurve.Evaluate(fractionOfJourney) * movementHeight;

            // Update the position based on the direction of movement
            Vector3 newPosition = isMovingUp ? startPosition + new Vector3(0f, yOffset, 0f) : startPosition - new Vector3(0f, yOffset, 0f);
            transform.position = newPosition;

            yield return null;

            // Check if the object has reached the top position
            if (fractionOfJourney >= 1f && isMovingUp)
            {   
                startPosition = transform.position; 
                isMovingUp = false; // Change the direction to move down
                startTime = Time.time; // Reset the start time for the downward movement
            }
        }
    }
}
