using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))] // Ensures that a Collider2D component is attached to the GameObject
public class TriggerEvent : MonoBehaviour
{
    [Header("Tag Settings")]
    public string tagName = "Player"; // Tag name to identify the player

    [Header("Trigger Event Settings")]
    public UnityEvent onTriggerEnter; // Event to invoke when player enters the trigger
    public UnityEvent onTriggerExit; // Event to invoke when player exits the trigger
    private bool isInTrigger = false; // Tracks if the player is in the trigger area
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

    void Update()
    {
        if (isInTrigger)
        {
            onTriggerEnter.Invoke(); // Invoke the button pressed event
        }
        else
        {
            onTriggerExit.Invoke(); // Invoke the button pressed event
        }
    }
}
