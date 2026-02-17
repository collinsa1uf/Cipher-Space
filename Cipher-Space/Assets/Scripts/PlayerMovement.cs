using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))] // Ensures that a Rigidbody2D component is attached to the GameObject
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5.0f; // Allows adjustment of movement speed in the Inspector
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movementVector; // Stores movement input vector
    private bool isMoving = false; // Tracks if the player is currently moving
    private bool canMove = true; // Flag to enable/disable movement

    [Header("Animator Settings")]
    public Animator animator; // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D reference
    }

    
    void Update()
    {
        movementVector = Vector2.zero; // Reset movement vector
        isMoving = false; // Reset moving state

        if (Keyboard.current.wKey.isPressed) // Check if 'W' key is pressed
        {
            movementVector.y += 1;
            isMoving = true;
        }
        if (Keyboard.current.sKey.isPressed) // Check if 'S' key is pressed
        {
            movementVector.y -= 1;
            isMoving = true;
        }
        if (Keyboard.current.aKey.isPressed) // Check if 'A' key is pressed
        {
            movementVector.x -= 1;
            isMoving = true;
        }
        if (Keyboard.current.dKey.isPressed) // Check if 'D' key is pressed
        {
            movementVector.x += 1;
            isMoving = true;
        }
        movementVector.Normalize(); // Normalize the movement vector to prevent faster diagonal movement
        UpdateAnimator(); // Update animator parameters based on movement
    }

    public void SetCanMove(bool value) // Method to enable or disable player movement
    {
        canMove = value;
        if (!canMove)
        {
            movementVector = Vector2.zero; // Stop movement immediately when disabled
            rb.linearVelocity = Vector2.zero; // Stop the player immediately when movement is disabled
        }
    }

    void FixedUpdate() // Uses physics update instead of framerate update
    {
        if (canMove)
        {
            rb.linearVelocity = movementVector * movementSpeed; // Move the player based on input
        }
    }

    void UpdateAnimator()
    {
        if (canMove){
            animator.SetBool("isMoving", isMoving); // Update animator parameter based on movement state
            animator.SetFloat("moveX", movementVector.x); // Update animator parameter for horizontal movement
            animator.SetFloat("moveY", movementVector.y); // Update animator parameter for vertical movement
            
            if (isMoving){
                    animator.SetFloat("lastMoveX", movementVector.x); // Update last move X value
                    animator.SetFloat("lastMoveY", movementVector.y); // Update last move Y value
            }
        }

        else
        {
            animator.SetBool("isMoving", false); // Ensure the player is not moving in the animator when movement is disabled
        }
    }
}
