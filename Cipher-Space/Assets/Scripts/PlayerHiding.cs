using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerHiding : MonoBehaviour
{
    private bool isHiding = false; // Tracks if the player is currently hiding
    private SpriteRenderer sprite; // Reference to the SpriteRenderer component for visual feedback
    private PlayerMovement playerMovement; // Reference to the PlayerMovement component to control player movement

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to the player
        playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement component attached to the player
    }

    public void ToggleHiding()
    {
        isHiding = !isHiding; // Toggle the hiding state
        
        sprite.enabled = !isHiding; // Enable or disable the SpriteRenderer based on the hiding state
        playerMovement.enabled = !isHiding; // Enable or disable player movement based on the hiding state 
    
    }

    public bool getIsHiding()
    {
        return isHiding; // Returns the current hiding state of the player
    }

    public void setIsHiding(bool hiding)
    {
        isHiding = hiding; // Sets the hiding state of the player
    }
}
