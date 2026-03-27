using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public ClientManager clientManager;
    public bool isInteractable;
    public string objectKey;
    public string password;
    public TextMeshProUGUI message;
    public GameObject buttonPanel;
    public GameObject blueprint;
    public PlayerMovement playerMovement;
    public EnemyTimer enemyTimer;
    public EncryptText encryptedMessageText;
    public string encryptedPassword;

    [Header("Events")]
    public UnityEvent onSuccess;
    public UnityEvent onFailure;

    public bool buttonComplete;

    void Update()
    {
        if (ClientManager.objects[objectKey] != "null")
        {
            isInteractable = true;
            gameObject.tag = "Interactable Object";
            password = ClientManager.objects[objectKey];
            encryptedPassword = encryptedMessageText.EncryptInput(password.ToUpper());
        }
        else
        {
            isInteractable = false;
            gameObject.tag = "Untagged";
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && buttonPanel.activeSelf)
        {
            buttonPanel.SetActive(false);
            blueprint.SetActive(false);
            playerMovement.SetCanMove(true);
        }
    }

    public void PressButton(GameObject buttonObj)
    {
        if (!isInteractable) return;

        string pw = password.Trim().ToLower();

        CockpitButton button = buttonObj.GetComponent<CockpitButton>();

        if (button == null)
        {
            Debug.LogWarning("Button missing CockpitButton component!");
            return;
        }

        bool isCorrect = pw == button.value1.Trim().ToLower() || pw == button.value2.Trim().ToLower();

        buttonPanel.SetActive(false);
        blueprint.SetActive(false);
        playerMovement.SetCanMove(true);

        if (isCorrect)
        {
            Debug.Log("Correct button");
            isInteractable = false;
            gameObject.tag = "Untagged";
            onSuccess?.Invoke();
        }
        else
        {
            Debug.Log("Incorrect button");
            enemyTimer.timeLeft = 10.0f;
            enemyTimer.StartTimer();
            onFailure?.Invoke();
        }
    }
}