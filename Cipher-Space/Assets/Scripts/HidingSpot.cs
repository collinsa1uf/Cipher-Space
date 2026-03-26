using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))] // Ensures that a Collider2D component is attached to the GameObject
public class HidingSpot : MonoBehaviour
{
    [Header("Tag Settings")]
    public string tagName = "Player"; // Tag name to identify the player
    private bool isInTrigger = false; // Tracks if the player is in the trigger area

    private PlayerHiding playerHiding; // Reference to the PlayerHiding component

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame && isInTrigger && playerHiding != null && !PauseMenu.isPaused) // Check if 'E' key was pressed this frame and player is in trigger
        {
            PlayerHiding playerHiding = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHiding>(); // Get the PlayerHiding component from the player GameObject
            if (playerHiding != null) // Check if the PlayerHiding component exists
            {
                playerHiding.ToggleHiding(); // Toggle the hiding state of the player
            }
        }

        UpdateSprite(); // Updates specific sprites to indicate if player is hiding there or not. Ex: Open -> Closed state
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(tagName)) // Check if the colliding object has the specified tag
        {
            isInTrigger = true;
            playerHiding = collider.GetComponent<PlayerHiding>(); // Get the PlayerHiding component from the colliding object
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(tagName)) // Check if the exiting object has the specified tag
        {
            isInTrigger = false;
            playerHiding = null; // Clear the reference to the PlayerHiding component when the player exits the trigger
        }
    }

    private void UpdateSprite()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (playerHiding != null)
        {
            // Closed states
            if (playerHiding.getIsHiding())
            {
                if (sr.sprite.name == "Locker-Open")
                {
                    Sprite newSprite = Resources.Load<Sprite>("Locker-Closed");
                    sr.sprite = newSprite;
                }
                else if (sr.sprite.name == "Cabinet-Open")
                {
                    Sprite newSprite = Resources.Load<Sprite>("Cabinet-Closed");
                    sr.sprite = newSprite;
                }
            }
            // Open states
            else
            {
                if (sr.sprite.name == "Locker-Closed")
                {
                    Sprite newSprite = Resources.Load<Sprite>("Locker-Open");
                    sr.sprite = newSprite;
                }
                else if (sr.sprite.name == "Cabinet-Closed")
                {
                    Sprite newSprite = Resources.Load<Sprite>("Cabinet-Open");
                    sr.sprite = newSprite;
                }
            }
        }
    }
}