using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class tutorialLock : MonoBehaviour
{
    [Header("Password Settings")]
    public string password = "1234";
    public string message = "1234";
    public PasswordManager passwordManager;

    [Header("On Success")]
    public UnityEvent onUnlock;

    [Header("UI Customization")]
    public GameObject customLayout;   // Which layout to use (optional)
    public Sprite lockSprite;         // Optional lock sprite override

    private bool unlocked = false;
    private bool isInteractable = true;
    public bool isJail;
    public string objectKey;

    void Update()
    {
        
    }

    public void Interact()
    {
        if (passwordManager.gameObject.activeSelf) return;
        if (unlocked) return;

        UnityEvent successEvent = new UnityEvent();
        successEvent.AddListener(() =>
        {
            Unlock();
            onUnlock?.Invoke();
        });

        PasswordUIConfig config = new PasswordUIConfig
        {
            customLayout = customLayout,
            lockImage = lockSprite
        };

        passwordManager.Open(password, message, successEvent, config);
    }

    private void Unlock()
    {
        unlocked = true;
        enabled = false;
        isInteractable = false;
        gameObject.tag = "Untagged";
    }
}