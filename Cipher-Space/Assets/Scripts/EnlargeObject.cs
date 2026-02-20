using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnlargeObject : MonoBehaviour
{
    [Header("Tag Settings")]
    public string tagName = "Player";
    private PlayerMovement playerMovement;
    
    [Header("Button Event Settings")]
    private bool isInTrigger = false;

    [Header("Inspect Panel")]
    public GameObject inspectPanel;
    public UnityEngine.UI.Image inspectImage;

    [Header("Inspect Object")]
    public Sprite enlargedSprite; // changes per object user inspects
    private bool isInspecting = false;

    [Header("Inspect Data")]
    public int objectIndex;
    public TMP_Text inspectText;

    [Header("Password Settings")]
    [SerializeField] private TranslationManager translationManager;
    public string password;
    public string message;
    public UnityEvent onSuccessEvent;

    void Update()
    {
        if (translationManager != null && translationManager.gameObject.activeSelf)
        {
            // If the password input manager is active, check to see if user pressed escape to close the input
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                translationManager.Close();
                inspectPanel.SetActive(false);
                isInspecting = false;

                if (playerMovement != null)
                    playerMovement.SetCanMove(true);
            }
            return;
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && isInTrigger) // Check if 'E' key was pressed this frame and player is in trigger
        {
           if(!isInspecting) // inspect the image = display the enlarged sprite
            { 
                inspectImage.sprite = enlargedSprite; // assign the enlarged sprite to the correct image
                inspectText.text = CipherGeneration.Encrypt(ParsingAI.Instance.objectDescriptors[objectIndex]); // assign the object title text
                inspectPanel.SetActive(true); // show the inspect panel
                isInspecting = true;
                
                if (playerMovement != null)
                {
                    playerMovement.SetCanMove(false); // disable player movement while inspecting
                }
                
                password = inspectText.text; // set password to the encrypted object descriptor
                message = "Enter " + ParsingAI.Instance.objectDescriptors[objectIndex]; // temp message with password for testing
                translationManager.Open(password, message, onSuccessEvent); // open the object input manager, allowing the user to guess the secret language

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
            playerMovement = collider.GetComponent<PlayerMovement>(); // Get the PlayerMovement component from the colliding object
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
