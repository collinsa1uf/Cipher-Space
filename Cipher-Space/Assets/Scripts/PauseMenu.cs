using System.Threading;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public DialogueManager dialogueManager;
    public EnemyTimer timer;
    public PlayerMovement playerMovement;
    public PasswordManager passwordManager;
    public TranslationManager translationManager;
    private bool wasTimerRunning;

    public void PauseGame()
    {
        isPaused = true;
        dialogueManager.PauseDialogue();

        wasTimerRunning = timer.timerRunning; // store state
        timer.StopTimer();

        playerMovement.SetCanMove(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        dialogueManager.ResumeDialogue();

        if (wasTimerRunning)
            timer.StartTimer();

        if (!passwordManager.gameObject.activeSelf && !translationManager.gameObject.activeSelf){
            playerMovement.SetCanMove(true);
        }
    }
}
