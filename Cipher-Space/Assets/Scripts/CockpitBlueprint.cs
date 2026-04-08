using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CockpitBlueprint : MonoBehaviour
{
    public ButtonManager buttonManager;
    public int buttonIndex;
    public LeverManager leverManager;
    public int leverIndex;
    public GameObject blueprintPanel;
    public PlayerMovement playerMovement;
    public CockpitManager cockpitManager;

    public TextMeshProUGUI firstInstruction;
    public TextMeshProUGUI secondInstruction;
    public TextMeshProUGUI passwordField;
    private string buttonExtension = "PRESS ";
    private string leverExtension = "PULL ";
    public TextMeshProUGUI leverLabel;
    public TextMeshProUGUI buttonLabel;
    public string passwordText;

    void Start()
    {
        buttonIndex = cockpitManager.buttonIndex;
        leverIndex = cockpitManager.leverIndex;
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && blueprintPanel.activeSelf)
        {
            blueprintPanel.SetActive(false);
            playerMovement.SetCanMove(true);
        }
        if (leverManager.isInteractable && buttonManager.isInteractable)
        {
            if (buttonIndex == 1)
            {
                firstInstruction.text = CipherGeneration.Encrypt(buttonExtension) + buttonManager.encryptedPassword;
                secondInstruction.text = CipherGeneration.Encrypt(leverExtension) + leverManager.encryptedPassword;
                buttonLabel.text = CipherGeneration.Encrypt("1");
                leverLabel.text = CipherGeneration.Encrypt("2");
            }
            else
            {
                firstInstruction.text = CipherGeneration.Encrypt(leverExtension) + leverManager.encryptedPassword;
                secondInstruction.text = CipherGeneration.Encrypt(buttonExtension) + buttonManager.encryptedPassword;
                buttonLabel.text = CipherGeneration.Encrypt("2");
                leverLabel.text = CipherGeneration.Encrypt("1");
            }

            passwordText = ClientManager.objects["blueprintCode"];
            passwordField.text = CipherGeneration.Encrypt(passwordText.ToUpper());
        }
    }
}
