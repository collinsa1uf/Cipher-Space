using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CockpitManager : MonoBehaviour
{
    public ButtonManager buttonManager;
    public int buttonIndex;
    public LeverManager leverManager;
    public int leverIndex;
    public TextMeshProUGUI firstInstruction;
    public TextMeshProUGUI secondInstruction;
    public TextMeshProUGUI passwordField;
    private string buttonExtension = " BUTTON";
    private string leverExtension = " LEVER";
    public string passwordText;
    void Start()
    {
        buttonIndex = Random.Range(1, 3);
        if (buttonIndex == 1) leverIndex = 2;
        else leverIndex = 1;
    }
    void Update()
    {
        if (leverManager.isInteractable && buttonManager.isInteractable)
        {
            if (buttonIndex == 1)
            {
                firstInstruction.text = buttonManager.encryptedPassword + CipherGeneration.Encrypt(buttonExtension);
                secondInstruction.text = leverManager.encryptedPassword + CipherGeneration.Encrypt(leverExtension);
            }
            else
            {
                firstInstruction.text = leverManager.encryptedPassword + CipherGeneration.Encrypt(leverExtension);
                secondInstruction.text = buttonManager.encryptedPassword + CipherGeneration.Encrypt(buttonExtension);
            }

            passwordText = ClientManager.objects["blueprintCode"];
            passwordField.text = CipherGeneration.Encrypt(passwordText.ToUpper());
        }
    }
}
