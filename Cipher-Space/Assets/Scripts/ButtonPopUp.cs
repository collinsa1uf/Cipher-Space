using UnityEngine;

public class ButtonPopUp : MonoBehaviour
{
    [HideInInspector] public static GameObject collidedObject = null;
    [HideInInspector] public static bool triggered = false;
    private GameObject hidingButton;
    private GameObject interactButton;

    void Start()
    {
        hidingButton = gameObject.transform.GetChild(0).gameObject;
        interactButton = gameObject.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (triggered && collidedObject != null)
        {
            ShowButtonIcon();
        }
        else if (!triggered && collidedObject != null)
        {
            HideButtonIcon();
        }
    }

    private void ShowButtonIcon()
    {
        Vector3 objectPos = collidedObject.transform.position;
        gameObject.transform.position = new Vector3(objectPos.x, objectPos.y + 20f, objectPos.z); // Moves button pop-up to object collided with

        if (collidedObject.tag == "Hiding Spot")
        {
            hidingButton.SetActive(true);
        }
        else if (collidedObject.tag == "Interactable Object")
        {
            interactButton.SetActive(true);
        }
    }

    private void HideButtonIcon()
    {
        if (collidedObject.tag == "Hiding Spot")
        {
            hidingButton.SetActive(false);
        }
        else if (collidedObject.tag == "Interactable Object")
        {
            interactButton.SetActive(false);
        }
    }
}
