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
    public static bool isInspecting = false;

    [Header("Inspect Data")]
    public TMP_Text inspectText;
    private bool isInteractable;
    public string objectKey;

    [Header("Password Settings")]
    [SerializeField] private TranslationManager translationManager;
    public string password;
    //public string message;
    public UnityEvent onSuccessEvent;

    [SerializeField] private Journal journal;



    void Update()
    {
        if (ClientManager.objects[objectKey] != "null")
        {
            isInteractable = true;
            gameObject.tag = "Interactable Object";
        }
        else
        {
            isInteractable = false;
            gameObject.tag = "Untagged";
        }

        if (journal != null && journal.IsOpen)
        {
            return;
        }

        if (translationManager != null && translationManager.gameObject.activeSelf)
        {
            // If the password input manager is active, check to see if user pressed escape to close the input
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (journal != null && journal.IsOpen)
                    return;

                translationManager.Close();
                inspectPanel.SetActive(false);
                isInspecting = false;

                if (playerMovement != null)
                    playerMovement.SetCanMove(true);
            }
            return;
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && isInTrigger && isInteractable) // Check if 'E' key was pressed this frame and player is in trigger
        {
            EnemyTimer.Instance.TriggerFirstObjectDialogue();
            EnemyTimer.Instance.TryActivateTimer();

            if (!isInspecting) // inspect the image = display the enlarged sprite
            { 
                inspectImage.sprite = enlargedSprite; // assign the enlarged sprite to the correct image
                // Console.WriteLine("Object Index: " + objectIndex); 
                // Console.WriteLine("Object Descriptor: " + ParsingAI.Instance.objectDescriptors[objectIndex]);
                inspectText.text = CipherGeneration.Encrypt(ClientManager.objects[objectKey]); // assign the object title text
                // Console.WriteLine("Encrypted Object Descriptor: " + inspectText.text);
                inspectPanel.SetActive(true); // show the inspect panel
                isInspecting = true;
                
                if (playerMovement != null)
                {
                    playerMovement.SetCanMove(false); // disable player movement while inspecting
                }
                
                password = inspectText.text; // set password to the encrypted object descriptor
                //message = "Enter " + ParsingAI.Instance.objectDescriptors[objectIndex]; // temp message with password for testing
                translationManager.Open(password, onSuccessEvent); // open the object input manager, allowing the user to guess the secret language

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
