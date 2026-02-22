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
    void Start()
    {
        dialogueManager.SetActive(false);
    }

    void Update()
    {
        if (isInDialogue && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
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
        int letter = 0;
        dialogueText.text = "";
        isTyping = true;
        cancelTyping = false;

        if (speakerAnimator != null)
        {
            speakerAnimator.SetBool(isSpeakingParam, true);
        }
        while (isTyping && !cancelTyping && (letter < lineOfText.Length))
        {
            dialogueText.text += lineOfText[letter];
            letter++;
            yield return new WaitForSeconds(typeSpeed);
        }

        dialogueText.text = lineOfText;

        if (speakerAnimator != null)
        {
            speakerAnimator.SetBool(isSpeakingParam, false);
        }

        continueImage.SetActive(true);
        isTyping = false;
        cancelTyping = false;
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
}
