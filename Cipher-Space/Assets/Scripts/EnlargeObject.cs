using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class EnlargeObject : MonoBehaviour
{
    [Header("Tag Settings")]
    public string tagName = "Player";

    [Header("Button Event Settings")]
    private bool isInTrigger = false;

    public GameObject inspectPanel;
    public UnityEngine.UI.Image inspectImage;
    public Sprite enlargedSprite; // changes per object user inspects
    private bool isInspecting = false;

    
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isInTrigger) // Check if 'E' key was pressed this frame and player is in trigger
        {
           if(!isInspecting) // inspect the image = display the enlarged sprite
            { 
                inspectImage.sprite = enlargedSprite; // assign the enlarged sprite to the correct image
                inspectPanel.SetActive(true); // show the inspect panel
                isInspecting = true;
            }
            else // close the enlaged image
            {
                inspectPanel.SetActive(false); // hide the inspect panel
                isInspecting = false;
            }
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
