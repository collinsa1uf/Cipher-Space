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

    public void PauseGame()
    {
        isPaused = true;
        dialogueManager.PauseDialogue();
        timer.StopTimer();
        playerMovement.SetCanMove(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        dialogueManager.ResumeDialogue();
        timer.StartTimer();

        if (!passwordManager.gameObject.activeSelf && !translationManager.gameObject.activeSelf){
            playerMovement.SetCanMove(true);
        }
    }
}
