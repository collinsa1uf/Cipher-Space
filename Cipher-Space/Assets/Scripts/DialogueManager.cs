using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueManager;
    public TMP_Text dialogueText;
    public GameObject continueImage;
    public Animator speakerAnimator;
    private string isSpeakingParam = "isSpeaking";

    [Header("Settings")]
    public bool scrollText = true;
    public float typeSpeed = 0.01f;
    public bool isInDialogue = false;
    public DialogueTrigger CurrentTrigger { get; set; }
    public bool isTyping = false;
    private bool cancelTyping = false;
    private Queue<string> dialogueLines = new();
    private Coroutine scrollCoroutine;

    [Header("Unity Events")]
    public UnityEvent onDialogueBegan;
    public UnityEvent onDialogueEnded;
    private bool isPaused = false;
    private string currentLine;
    private int currentLetterIndex = 0;

    void Start()
    {
        dialogueManager.SetActive(false);
    }

    void Update()
    {
        if (GameStateManager.InputLocked || isPaused)
            return;
        if (isInDialogue && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && !PauseMenu.isPaused)
        {
            AdvanceDialogue();
        }
    }

    public void BeginDialogue(Queue<string> dialogue)
    {

        isInDialogue = true;
        dialogueManager.SetActive(true);
        continueImage.SetActive(true);

        dialogueLines = dialogue;
        AdvanceDialogue();
        onDialogueBegan.Invoke();
    }

    public void AdvanceDialogue()
    {
        if (!isInDialogue) return;

        if (isTyping)
        {
            cancelTyping = true;
            if (speakerAnimator != null){
                speakerAnimator.SetBool(isSpeakingParam, false);
            }
            return;
        }

        if (dialogueLines.Peek().Contains("End"))
        {
            dialogueLines.Dequeue();
            EndDialogue();
        }

        else
        {
            if (scrollText){
                if (!isTyping)
                {
                    string textString = dialogueLines.Dequeue();

                    if (scrollCoroutine != null)
                    {
                        StopCoroutine(scrollCoroutine);
                    }
                    scrollCoroutine = StartCoroutine(TextScroll(textString));
                }

                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }

            else
            {
                dialogueText.text = dialogueLines.Dequeue();
                continueImage.SetActive(true);
            }
        }
    }

    private IEnumerator TextScroll(string lineOfText)
    {
        currentLine = lineOfText;
        //int letter = 0;
        dialogueText.text = "";
        isTyping = true;
        cancelTyping = false;

        if (speakerAnimator != null)
        {
            speakerAnimator.SetBool(isSpeakingParam, true);
        }
        while (currentLetterIndex < currentLine.Length)
        {
            if (isPaused)
            {
                if (speakerAnimator != null)
                {
                    speakerAnimator.SetBool(isSpeakingParam, false);
                }
                yield return null;
                continue;
            }

            if (cancelTyping)
            {
                break;
            }

            dialogueText.text += lineOfText[currentLetterIndex];
            currentLetterIndex++;
            yield return new WaitForSeconds(typeSpeed);
        }

        dialogueText.text = currentLine;

        if (speakerAnimator != null)
        {
            speakerAnimator.SetBool(isSpeakingParam, false);
        }

        continueImage.SetActive(true);
        isTyping = false;
        cancelTyping = false;
        currentLetterIndex = 0;
        currentLine = lineOfText;
        scrollCoroutine = null;
    }

    public void EndDialogue()
    {
        if (!isInDialogue)
        {
            return;
        }

        dialogueText.text = "";
        dialogueLines.Clear();
        dialogueManager.SetActive(false);
        isInDialogue = false;
        cancelTyping = false;
        isTyping = false;

        onDialogueEnded.Invoke();
    }

    private void OnValidate()
    {
        typeSpeed = Mathf.Max(0f, typeSpeed);
    }

    public void PauseDialogue()
    {
        if (!isInDialogue) return;

        //If currently typing instantly finish the line
        if (isTyping)
        {
            cancelTyping = true;

            // Force full line display
            dialogueText.text = currentLine;

            isTyping = false;
            cancelTyping = false;
            currentLetterIndex = 0;

            continueImage.SetActive(true);

            // Stop coroutine cleanly
            if (scrollCoroutine != null)
            {
                StopCoroutine(scrollCoroutine);
                scrollCoroutine = null;
            }
        }

        // Now pause before next line
        isPaused = true;

        if (speakerAnimator != null)
        {
            speakerAnimator.SetBool(isSpeakingParam, false);
        }
    }

    public void ResumeDialogue()
    {
        if (!isInDialogue) return;

        isPaused = false;

        if (!dialogueManager.activeSelf)
            dialogueManager.SetActive(true);
    }

}
