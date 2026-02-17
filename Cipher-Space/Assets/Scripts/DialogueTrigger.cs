using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueManager dialogueManager;
    public TextAsset textFile;
    public string playerTag = "Player";
    public bool singleUse = false;

    private Queue<string> dialogueLines = new();
    private PlayerMovement playerMovement;
    private bool hasBeenUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenUsed && singleUse == true) return;

        if (collision.CompareTag(playerTag))
        {
            hasBeenUsed = true;

            playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.SetCanMove(false);

            ReadTextFile();
            dialogueManager.CurrentTrigger = this;
            dialogueManager.onDialogueEnded.AddListener(OnDialogueEnded); // callback
            dialogueManager.BeginDialogue(dialogueLines);
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
        if (playerMovement != null)
            playerMovement.SetCanMove(true);

        dialogueManager.onDialogueEnded.RemoveListener(OnDialogueEnded);
    }
}
