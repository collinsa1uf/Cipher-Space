using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueManager dialogueManager;
    public TextAsset textFile;
    public bool singleUse = false;

    private Queue<string> dialogueLines = new();
    private bool hasBeenUsed = false;

    public void StartDialogue()
    {
        if (hasBeenUsed && singleUse) return;

        ReadTextFile();

        dialogueManager.CurrentTrigger = this;
        dialogueManager.BeginDialogue(dialogueLines);

        hasBeenUsed = true;
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
}