using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    [Header("Password Settings")]
    public string password;
    public string message;
    public PasswordManager passwordManager;

    [Header("On Success")]
    public UnityEvent onUnlock;

    [Header("UI Customization")]
    public GameObject customLayout;   // Which layout to use (optional)
    public Sprite lockSprite;         // Optional lock sprite override

    private bool unlocked = false;

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
    }
}