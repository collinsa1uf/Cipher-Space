using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.UI;
using JetBrains.Annotations;


[System.Serializable]
public class PasswordUIConfig
{
    public GameObject customLayout;   // Which layout to use
    public Sprite lockImage;          // Optional lock sprite override
}
public class PasswordManager : MonoBehaviour
{
    [Header("Layout Root")]
    public GameObject passwordPanelRoot;   // Parent of all layouts
    public GameObject defaultLayout;       // Fallback layout

    [Header("References")]
    public PlayerMovement playerMovement;
    public DialogueManager dialogueManager;
    public Journal journal;

    private PasswordLayout activeLayout;

    private UnityEvent onSuccess;
    private string correctPassword;
    private string currentInput = "";

    void Update()
    {
        if (!gameObject.activeSelf)
            return;

        if (dialogueManager != null && dialogueManager.isInDialogue)
            return;

        if (journal != null && journal.IsOpen)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
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
        if (playerMovement != null)
            playerMovement.SetCanMove(false);

        currentInput = "";
        UpdateDisplay();

        if (Keyboard.current != null)
            Keyboard.current.onTextInput += OnTextInput;
    }

    void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.SetCanMove(true);

        if (Keyboard.current != null)
            Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char c)
    {
        if (!gameObject.activeSelf)
            return;

        if (dialogueManager != null && dialogueManager.isInDialogue)
            return;

        if (journal != null && journal.IsOpen)
            return;

        if (!char.IsLetterOrDigit(c))
            return;

        if (currentInput.Length < correctPassword.Length)
        {
            currentInput += char.ToUpperInvariant(c);
            UpdateDisplay();
        }
    }

    public void Open(string password, string message, UnityEvent successEvent, PasswordUIConfig config = null)
    {
        correctPassword = password.ToUpperInvariant();
        currentInput = "";
        onSuccess = successEvent;

        SetLayout(config != null ? config.customLayout : null);

        if (activeLayout == null)
        {
            Debug.LogError("No active layout found!");
            return;
        }
        gameObject.SetActive(true);
        activeLayout.messageDisplay.text = message;
        activeLayout.messageDisplay.ForceMeshUpdate();

        if (config != null && config.lockImage != null)
            activeLayout.lockImageDisplay.sprite = config.lockImage;

        EncryptText[] encryptables = activeLayout.GetComponentsInChildren<EncryptText>(true);
        
        foreach (var e in encryptables)
        {
            if (e.GetComponent<TextMeshProUGUI>() == activeLayout.messageDisplay)
                e.SetNewOriginal(message);
            e.Encrypt();
        }

        gameObject.SetActive(true);
        UpdateDisplay();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    // private void ValidatePassword(string input)
    // {
    //     if (CipherGeneration.Encrypt(input.ToUpperInvariant()) ==
    //         correctPassword.ToUpperInvariant())
    //     {
    //         journal?.UpdateJournalText(input.ToUpperInvariant(), correctPassword);
    //         onSuccess?.Invoke();
    //         Close();
    //     }
    //     else
    //     {
    //         currentInput = "";
    //         UpdateDisplay();
    //     }
    // }
    private void ValidatePassword(string input)
    {
        if (input.ToUpperInvariant() == correctPassword.ToUpperInvariant())
        {
            journal?.UpdateJournalText(input.ToUpperInvariant(), CipherGeneration.Encrypt(correctPassword));
            onSuccess?.Invoke();
            Close();
        }
        else
        {
            currentInput = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (activeLayout == null) return;

        activeLayout.passwordDisplay.text = BuildDisplay();
    }

    private string BuildDisplay()
    {
        string display = "";

        for (int i = 0; i < correctPassword.Length; i++)
        {
            if (i < currentInput.Length)
                display += currentInput[i] + " ";
            else
                display += "_ ";
        }

        return display.TrimEnd();
    }

    private void SetLayout(GameObject layoutObject)
    {
        foreach (Transform child in passwordPanelRoot.transform)
            child.gameObject.SetActive(false);

        GameObject selected = layoutObject != null ? layoutObject : defaultLayout;
        selected.SetActive(true);

        activeLayout = selected.GetComponent<PasswordLayout>();
    }
}