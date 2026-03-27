using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class LeverManager : MonoBehaviour
{
    public ClientManager clientManager;
    public bool isInteractable;
    public string objectKey;
    public string password;
    public TextMeshProUGUI message;
    public GameObject leverPanel;
    public GameObject blueprint;
    public ButtonEvent buttonEvent;
    public PlayerMovement playerMovement;
    public EnemyTimer enemyTimer;
    public EncryptText encryptedMessageText;
    public string encryptedPassword;

    [Header("Events")]

    public bool leverComplete;

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

        if (Keyboard.current.escapeKey.wasPressedThisFrame && leverPanel.activeSelf)
        {
            leverPanel.SetActive(false);
            blueprint.SetActive(false);
            playerMovement.SetCanMove(true);
        }
    }

    public void PressButton(GameObject buttonObj)
    {
        if (!isInteractable) return;

        string pw = password.Trim().ToLower();

        CockpitLever lever = buttonObj.GetComponent<CockpitLever>();

        if (lever == null)
        {
            Debug.LogWarning("Button missing LeverButton component!");
            return;
        }

        bool isCorrect = pw == lever.value1.Trim().ToLower() || pw == lever.value2.Trim().ToLower();

        leverPanel.SetActive(false);
        blueprint.SetActive(false);
        playerMovement.SetCanMove(true);

        if (isCorrect)
        {
            //Debug.Log("Correct Lever");

            var cockpit = FindFirstObjectByType<CockpitManager>();
            bool valid = cockpit.OnLeverPressed(true);

            if (valid)
            {
                isInteractable = false;
                gameObject.tag = "Untagged";

                if (buttonEvent != null)
                    buttonEvent.enabled = false;
            }
            else
            {
                enemyTimer.timeLeft = 10.0f;
                enemyTimer.StartTimer();
            }
        }
        
        else
        {
            //Debug.Log("Incorrect Lever");
            enemyTimer.timeLeft = 10.0f;
            enemyTimer.StartTimer();
            FindFirstObjectByType<CockpitManager>().OnLeverPressed(false);
            isInteractable = true;
            //onFailure?.Invoke();
        }
    }
}