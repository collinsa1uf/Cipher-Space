using UnityEngine;
using UnityEngine.UI;

public class EnlargePassword : MonoBehaviour
{
    [SerializeField] private PasswordManager passwordManager;
    [SerializeField] private Image passwordInspectImage;
    [SerializeField] private Sprite enlargedSprite;

    private bool wasActiveLastFrame = false;

    void Update()
    {
        if (passwordManager == null) return;

        bool isActiveNow = passwordManager.gameObject.activeSelf;

        if (isActiveNow && !wasActiveLastFrame)
        {
            passwordInspectImage.sprite = enlargedSprite;
            passwordInspectImage.gameObject.SetActive(true);
        }

        if (!isActiveNow && wasActiveLastFrame)
        {
            passwordInspectImage.gameObject.SetActive(false);
        }

        wasActiveLastFrame = isActiveNow;
    }
}