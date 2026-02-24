using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hiding Spot" || other.tag == "Interactable Object")
        {
            ButtonPopUp.triggered = true;
            ButtonPopUp.collidedObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hiding Spot" || other.tag == "Interactable Object")
        {
            ButtonPopUp.triggered = false;
        }
    }
}
