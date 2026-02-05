using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))] // Ensures that a Rigidbody2D component is attached to the GameObject
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5.0f; // Allows adjustment of movement speed in the Inspector
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movementVector; // Stores movement input vector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D reference
    }

    
    void Update()
    {
        movementVector = Vector2.zero; // Reset movement vector

        if (Keyboard.current.wKey.isPressed) // Check if 'W' key is pressed
        {
            movementVector.y += 1;
        }
        if (Keyboard.current.sKey.isPressed) // Check if 'S' key is pressed
        {
            movementVector.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed) // Check if 'A' key is pressed
        {
            movementVector.x -= 1;
        }
        if (Keyboard.current.dKey.isPressed) // Check if 'D' key is pressed
        {
            movementVector.x += 1;
        }
        movementVector.Normalize(); // Normalize the movement vector to prevent faster diagonal movement
    }

    void FixedUpdate() // Uses physics update instead of framerate update
    {
        rb.linearVelocity = movementVector * movementSpeed; // Move the player based on input
    }
}
