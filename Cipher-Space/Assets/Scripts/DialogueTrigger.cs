using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueManager dialogueManager;
    public TextAsset textFile;
    public string playerTag = "Player";
    public bool singleUse = false;
    public PasswordManager passwordManager;

    private Queue<string> dialogueLines = new();
    private PlayerMovement playerMovement;
    private bool hasBeenUsed = false;

    public void StartDialogue()
    {
        if (hasBeenUsed && singleUse) return;

        Collider2D col = Physics2D.OverlapBox(transform.position, GetComponent<Collider2D>().bounds.size, 0);
        if (col != null && col.CompareTag(playerTag))
        {
            playerMovement = col.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.SetCanMove(false); // freeze player

            ReadTextFile();

            dialogueManager.CurrentTrigger = this;
            dialogueManager.onDialogueEnded.AddListener(OnDialogueEnded);
            dialogueManager.BeginDialogue(dialogueLines);

            hasBeenUsed = true;
        }
    }

    private void ReadTextFile()
    {
        dialogueLines.Clear();
        string[] lines = textFile.text.Split('\n');
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (!string.IsNullOrEmpty(trimmed))
                dialogueLines.Enqueue(trimmed);
        }
        dialogueLines.Enqueue("End");
    }

    private void OnDialogueEnded()
    {
        if (playerMovement != null && !passwordManager.gameObject.activeSelf)
            playerMovement.SetCanMove(true);

        dialogueManager.onDialogueEnded.RemoveListener(OnDialogueEnded);
    }
}
