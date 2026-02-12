using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.Controls;

public class PasswordManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passwordDisplay; // Display message to the user
    public TextMeshProUGUI messageDisplay; // Display message to the user

    [Header("Player Reference")]
    public PlayerMovement playerMovement; // Reference to the PlayerMovement component
    private UnityEvent onSuccess; // Event to invoke on successful password entry
    private string correctPassword;
    private string currentInput = ""; // Input field for password entry

    void Update()
    {
        if (!gameObject.activeSelf) return; // If the password manager is not active, do nothing
        if (Keyboard.current.escapeKey.wasPressedThisFrame) // Check if 'Escape' key was pressed this frame
        {
            Close(); // hide password entry UI
        }

        HandleTyping(); // Handle character input for password entry
    }

    void OnEnable()
    {
        playerMovement.SetCanMove(false); // Disable player movement when entering password
        //StartCoroutine(FocusInputField()); // Focus the input field on the next frame to ensure it works correctly
    }

    void OnDisable()
    {
        playerMovement.SetCanMove(true); // Re-enable player movement when done
    }

    public void Open(string password, string message, UnityEvent onSuccess)
    {
        correctPassword = password.ToUpperInvariant(); // Set the correct password for validation
        currentInput = "";
        gameObject.SetActive(true); // Show password entry UI

        passwordDisplay.text = BuildDisplay(); // Prompt the user to enter the password
        this.onSuccess = onSuccess; // Store success event to evoke
    }

    public void Close()
    {
        gameObject.SetActive(false); //hide password entry UI
    }

    private void ValidatePassword(string input) // validate password after enter is pressed
    {
        if (input.ToUpperInvariant() == correctPassword.ToUpperInvariant())
        {
            messageDisplay.text = "PASSWORD CORRECT!";
            onSuccess.Invoke(); // Invoke the success event
            Close();
        }
        else
        {
            messageDisplay.text = "PASSWORD INCORRECT!";
            currentInput = ""; // Clear the current input on failure
            UpdateDisplay();
        }
    }

    private void HandleTyping()
    {
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (!key.wasPressedThisFrame) continue; // Check if the key was pressed this frame

            if (key == Keyboard.current.backspaceKey)
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1); // Remove the last character from the input
                    UpdateDisplay();
                    return;
                }
            }

            if (key == Keyboard.current.enterKey)
            {
                ValidatePassword(currentInput); // Validate the password when 'Enter' is pressed
                return;
            }

            char c = GetCharFromKey(key);
            if (c == '\0') return;

            if (currentInput.Length < correctPassword.Length) // Limit input length to the length of the correct password
            {
                currentInput += c; // Append the character to the current input
                UpdateDisplay();
            }
        }
    }

    private char GetCharFromKey(KeyControl key) // Convert a KeyControl to its corresponding character, if it's a letter or digit
    {
        string keyName = key.displayName;

        if (string.IsNullOrEmpty(keyName) || keyName.Length != 1) // Only process single-character keys (letters and digits)
        {
            return '\0';
        }

        char c = keyName[0]; // Get the character from the key's display name

        if (!char.IsLetterOrDigit(c)) // Allow only letters or digits
        {
            return '\0';
        }
        return char.ToUpperInvariant(c); // Convert to uppercase for case-insensitive comparison
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