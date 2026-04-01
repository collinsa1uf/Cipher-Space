using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hiding Spot")
        {
            ButtonPopUp.triggeredHiding = true;
            ButtonPopUp.collidedHidingObject = other.gameObject;
        }
        else if (other.tag == "Interactable Object")
        {
            ButtonPopUp.triggeredInteractable = true;
            ButtonPopUp.collidedInteractableObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hiding Spot")
        {
            ButtonPopUp.triggeredHiding = false;
        }
        else if (other.tag == "Interactable Object")
        {
            ButtonPopUp.triggeredInteractable = false;
        }
    }
}
