using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.Controls;

public class TranslationManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passwordDisplay; // Display message to the user
    //public TextMeshProUGUI messageDisplay; // Display message to the user
    public Journal journal;

    [Header("Inspect Panel")]
    public GameObject inspectPanel;

    [Header("Player Reference")]
    public PlayerMovement playerMovement; // Reference to the PlayerMovement component
    private UnityEvent onSuccess; // Event to invoke on successful password entry
    private string correctPassword;
    private string currentInput = ""; // Input field for password entry
    public DialogueManager dialogueManager;

    void Update()
    {
        if (PauseMenu.isPaused || dialogueManager.isInDialogue)
        {
            return;
        }
        if (!gameObject.activeSelf) return; // If the password manager is not active, do nothing

        if (journal != null && journal.IsOpen)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame) // Check if 'Escape' key was pressed this frame
        {
            Close(); // hide password entry UI
            return;
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                UpdateDisplay();
            }
            return;
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ValidatePassword(currentInput);
            return;
        }

    }

    void OnEnable()
    {
        playerMovement.SetCanMove(false); // Disable player movement when entering password

        currentInput = ""; // Clear any previous input
        UpdateDisplay();
        Keyboard.current.onTextInput += OnTextInput; // Subscribe to text input events
    }

    void OnDisable()
    {
        playerMovement.SetCanMove(true); // Re-enable player movement when done
        Keyboard.current.onTextInput -= OnTextInput; // Unsubscribe from text input events
    }

    private void OnTextInput(char c)
    {
        if (PauseMenu.isPaused || dialogueManager.isInDialogue)
        {
            return;
        }
        if (!gameObject.activeSelf) return; // If the password manager is not active, do nothing

        if (journal != null && journal.IsOpen)
            return;

        if (!char.IsLetterOrDigit(c)) return; // Only allow letters and digits

        if (currentInput.Length < correctPassword.Length) // Limit input length to the length of the correct password
        {
            currentInput += char.ToUpperInvariant(c); // Append the character to the current input, converting to uppercase for case-insensitivity
            UpdateDisplay();
        }
    }

    public void Open(string password, UnityEvent onSuccess)
    {
        correctPassword = password.ToUpperInvariant(); // Set the correct password for validation
        currentInput = "";
        gameObject.SetActive(true); // Show password entry UI

        passwordDisplay.text = BuildDisplay(); // Prompt the user to enter the password
        //messageDisplay.text = message; // Display the provided message to the user
        this.onSuccess = onSuccess; // Store success event to evoke
    }

    public void Close()
    {
        gameObject.SetActive(false); //hide password entry UI
    }

    private void ValidatePassword(string input) // validate password after enter is pressed
    {
        if (input.Length == correctPassword.Length)    
        {
            // update journal with translation as long as input is the correct length
            onSuccess.Invoke(); // Invoke the success event
            inspectPanel.SetActive(false); // hide the inspect panel if it's open
            journal.UpdateJournalText(input.ToUpperInvariant(), correctPassword);
            EnlargeObject.isInspecting = false;
            Close();
        }
        else
        {
            // input too short
            currentInput = ""; // Clear the current input on failure
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        passwordDisplay.text = "" + BuildDisplay(); // Update the display with the current input
    }

    private string BuildDisplay()
    {
        string display = "";
        for (int i = 0; i < correctPassword.Length; i++)
        {
            if (i < currentInput.Length)
            {
                display += currentInput[i] + " "; // Show the entered character
            }
            else
            {
                display += "_ "; // Show an underscore for unentered characters
            }
        }
        return display.TrimEnd(); // Remove the trailing space;
    }
}
