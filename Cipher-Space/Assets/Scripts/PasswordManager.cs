using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PasswordManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI messageDisplay; // Display message to the user
    public TMP_InputField passwordInputField; // Input field for password entry

    [Header("Player Reference")]
    public PlayerMovement playerMovement; // Reference to the PlayerMovement component
    private UnityEvent onSuccess; // Event to invoke on successful password entry
    private string correctPassword;
    
    void Start()
    {
        passwordInputField.onValidateInput += ValidateCharacter; // Set up character validation
        passwordInputField.lineType = TMP_InputField.LineType.SingleLine;
        passwordInputField.onSubmit.AddListener(ValidatePassword); // listen for enter key
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) // Check if 'Escape' key was pressed this frame
        {
            Close(); // hide password entry UI
        }
    }

    void OnEnable()
    {
        playerMovement.SetCanMove(false); // Disable player movement when entering password
        passwordInputField.text = ""; // Clear any previous input
        passwordInputField.ActivateInputField(); // Activate the input field
        passwordInputField.Select(); // Select the input field
    }

    void OnDisable()
    {
        playerMovement.SetCanMove(true); // Re-enable player movement when done
    }

    public void Open(string password, string message, UnityEvent onSuccess)
    {
        correctPassword = password.ToUpperInvariant(); // Set the correct password for validation
        gameObject.SetActive(true); // Show password entry UI

        messageDisplay.text = message; // Prompt the user to enter the password
        this.onSuccess = onSuccess; // Store success event to evoke
    }

    public void Close()
    {
        gameObject.SetActive(false); //hide password entry UI
    }

    private char ValidateCharacter(string text, int charIndex, char addedChar) // validate each character as it's entered
    {
        if (!char.IsLetterOrDigit(addedChar)) // Allow only letters or digits
        {
            return '\0'; // Reject non-letter or non-digit characters (backspace)
        }
        else
        {
            return char.ToUpperInvariant(addedChar); // Accept the character
        }
    }
    private void ValidatePassword(string input) // validate password after enter is pressed
    {
        if (input.ToUpperInvariant() == correctPassword.ToUpperInvariant())
        {
            messageDisplay.text = "Password Correct!";
            onSuccess.Invoke(); // Invoke the success event
            Close();
        }
        else
        {
            messageDisplay.text = "Incorrect Password!";
            passwordInputField.text = ""; //clear password after enter
            passwordInputField.ActivateInputField(); //refocus input field
        }
    }
}
