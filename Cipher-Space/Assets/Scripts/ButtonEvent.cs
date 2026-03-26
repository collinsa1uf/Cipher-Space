using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))] // Ensures that a Collider2D component is attached to the GameObject
public class ButtonEvent : MonoBehaviour
{

    [Header("Tag Settings")]
    public string tagName = "Player"; // Tag name to identify the player

    [Header("Button Event Settings")]
    public UnityEvent onButtonPressed; // Event to invoke when button is pressed
    private bool isInTrigger = false; // Tracks if the player is in the trigger area

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isInTrigger && !PauseMenu.isPaused) // Check if 'E' key was pressed this frame and player is in trigger
        {
            onButtonPressed.Invoke(); // Invoke the button pressed event
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(tagName)) // Check if the colliding object has the specified tag
        {
            isInTrigger = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(tagName)) // Check if the exiting object has the specified tag
        {
            isInTrigger = false;
        }
    }
}
