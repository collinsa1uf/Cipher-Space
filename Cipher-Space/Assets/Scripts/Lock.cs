using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    [Header("Password Settings")]
    public string password; // The password required to unlock
    public string message; // Message to display when prompting for password
    public PasswordManager passwordManager; // Reference to the PasswordManager component

    [Header("On Success")]
    public UnityEvent onUnlock; // Event to invoke when the correct password is entered

    [Header("On Failure")]
    public UnityEvent onFailure; // Event to invoke when an incorrect password is entered]
    
    private bool unlocked = false;
    public void Interact()
    {
        if (unlocked) return; // If already unlocked, do nothing

        UnityEvent successEvent = new UnityEvent();
        successEvent.AddListener(() => {
            Unlock();
            onUnlock.Invoke();
        });

        passwordManager.Open(password, message, successEvent); // Open the password manager with the specified password and events
    }

    private void Unlock()
    {
        unlocked = true;
        enabled = false; // Disable this script to prevent further interaction
    }
    
}
