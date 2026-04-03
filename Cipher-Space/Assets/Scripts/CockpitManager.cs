using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.Events;

public class CockpitManager : MonoBehaviour
{
    public ButtonManager buttonManager;
    public int buttonIndex;
    public LeverManager leverManager;
    public int leverIndex;
    //public GameObject blueprintPanel;

    public EnemyTimer enemyTimer;
    public TextMeshProUGUI firstInstruction;
    public TextMeshProUGUI secondInstruction;
    public TextMeshProUGUI passwordField;
    private string buttonExtension = " BUTTON";
    private string leverExtension = " LEVER";
    public TextMeshProUGUI leverLabel;
    public TextMeshProUGUI buttonLabel;
    public string passwordText;

    private int currentStep = 0;
    private bool buttonDone = false;
    private bool leverDone = false;

    public UnityEvent onSuccess;

    public UnityEvent onFailure;

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
                buttonLabel.text = CipherGeneration.Encrypt("1");
                leverLabel.text = CipherGeneration.Encrypt("2");
            }
            else
            {
                firstInstruction.text = leverManager.encryptedPassword + CipherGeneration.Encrypt(leverExtension);
                secondInstruction.text = buttonManager.encryptedPassword + CipherGeneration.Encrypt(buttonExtension);
                buttonLabel.text = CipherGeneration.Encrypt("2");
                leverLabel.text = CipherGeneration.Encrypt("1");
            }

            passwordText = ClientManager.objects["blueprintCode"];
            passwordField.text = CipherGeneration.Encrypt(passwordText.ToUpper());
        }
    }

    public bool OnButtonPressed (bool success)
    {
        if (!success)
        {
            Fail();
            return false;
        }

        bool shouldBeFirst = (buttonIndex == 1);

        if (currentStep == 0)
        {
            if (!shouldBeFirst)
            {
                Fail();
                return false;
            }
            buttonDone = true;
            currentStep = 1;
            return true;
        }

        else if (currentStep == 1)
        {
            if (shouldBeFirst)
            {
                Fail();
                return false;
            }
            buttonDone = true;
            CheckComplete();
            return true;
        }
        return false;
    }

    public bool OnLeverPressed (bool success)
    {
        if (!success)
        {
            Fail();
            return false;
        }

        bool shouldBeFirst = (leverIndex == 1);

        if (currentStep == 0)
        {
            if (!shouldBeFirst)
            {
                Fail();
                return false;
            }
            leverDone = true;
            currentStep = 1;
            return true;
        }

        else if (currentStep == 1)
        {
            if (shouldBeFirst)
            {
                Fail();
                return false;
            }
            leverDone = true;
            CheckComplete();
            return true;
        }
        return false;
    }

    private void CheckComplete()
    {
        if (buttonDone && leverDone)
        {
            //Debug.Log("Correct order");
            onSuccess?.Invoke();
        }
    }

    private void Fail()
    {
        enemyTimer.countdownDuration = 10f;
        enemyTimer.RestartTimer();
        onFailure?.Invoke();
    }
}
