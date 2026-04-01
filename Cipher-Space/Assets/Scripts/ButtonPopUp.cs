using UnityEngine;

public class ButtonPopUp : MonoBehaviour
{
    [HideInInspector] public static GameObject collidedHidingObject = null;
    [HideInInspector] public static GameObject collidedInteractableObject = null;
    [HideInInspector] public static bool triggeredHiding = false;
    [HideInInspector] public static bool triggeredInteractable = false;
    private GameObject hidingButton;
    private GameObject interactButton;

    void Start()
    {
        hidingButton = gameObject.transform.GetChild(0).gameObject;
        interactButton = gameObject.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if ((triggeredHiding || triggeredInteractable) && (collidedHidingObject != null || collidedInteractableObject != null))
        {
            ShowButtonIcon();
        }
        else if (!(triggeredHiding || triggeredInteractable) && (collidedHidingObject != null || collidedInteractableObject != null))
        {
            HideButtonIcon();
        }
    }

    private void ShowButtonIcon()
    {
        if (triggeredHiding && collidedHidingObject != null)
        {
            Vector3 objectPos = collidedHidingObject.transform.position;
            hidingButton.transform.position = new Vector3(objectPos.x, objectPos.y + 20f, objectPos.z); // Moves button pop-up to object collided with
            hidingButton.SetActive(true);
        }
        if (triggeredInteractable && collidedInteractableObject != null)
        {
            Vector3 objectPos = collidedInteractableObject.transform.position;
            interactButton.transform.position = new Vector3(objectPos.x, objectPos.y + 20f, objectPos.z); // Moves button pop-up to object collided with
            interactButton.SetActive(true);
        }
    }

    private void HideButtonIcon()
    {
        if (!triggeredHiding && collidedHidingObject != null)
        {
            hidingButton.SetActive(false);
        }
        if (!triggeredInteractable && collidedInteractableObject != null)
        {
            interactButton.SetActive(false);
        }
    }
}
