using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Journal : MonoBehaviour
{
    [SerializeField] private GameObject journalObject; // the journal UI object to toggle on and off
    [SerializeField] private TextMeshProUGUI alienText; // left page alien text 
    [SerializeField] private TextMeshProUGUI journalText; // left page translation
    [SerializeField] private TextMeshProUGUI equalsText;
    [SerializeField] private TextMeshProUGUI alienText2; // right page alien text
    [SerializeField] private TextMeshProUGUI journalText2; // right page translation
    [SerializeField] private TextMeshProUGUI equalsText2;
    [SerializeField] private GameObject toggleJ; // UI button to toggle journal on and off

    [SerializeField] private PasswordManager passwordManager; // Reference to the PasswordManager component
    [SerializeField] private TranslationManager translationManager;

    List<(string letter, string guess)> entries = new List<(string, string)> {}; // list of journal entries
    // each entry is a tuple of the human letter and the player's guess for the alien letter visually represented 

    void Start()
    {
        journalObject.SetActive(false); // Ensure the journal is hidden at the start
        toggleJ.SetActive(true); // Ensure the toggle button is visible

        // initialize all the text fields to be empty at the start of the game
        journalText.text = ""; 
        alienText.text = ""; 
        equalsText.text = "";
        journalText2.text = "";
        alienText2.text = "";
        equalsText2.text = "";

        createJournal(); // populate journal with initial entries
    }

    void Update()
    {
        if (passwordManager != null && passwordManager.gameObject.activeSelf) return; // Don't allow toggling the journal if the password manager is open
        if (translationManager != null && translationManager.gameObject.activeSelf) return; // Don't allow toggling the journal if the translation manager is open
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            journalObject.SetActive(!journalObject.activeSelf);
            toggleJ.SetActive(!toggleJ.activeSelf);
        }

    }
    void createJournal() // recreates the journal text with the current list of entries 
    {
        entries.Sort((a, b) => a.letter.CompareTo(b.letter)); // sort entries alphabetically

        for (int i = 0; i < entries.Count; i++)
        {
            {
                // split entries between the two pages if there are more than 13 entries
                if (i < 13)
                {
                    journalText.text += entries[i].letter + "\n";
                    alienText.text += entries[i].guess + "\n";
                    equalsText.text += "=\n";
                }
                else
                {
                    journalText2.text += entries[i].letter + "\n";
                    alienText2.text += entries[i].guess + "\n";
                    equalsText2.text += "=\n";
                }
            }
        }
    }
    public void UpdateJournalText(string guessWord, string givenWord)
    {
        for (int i = 0; i < guessWord.Length; i++)
        {
            entries.RemoveAll(p => p.letter == guessWord.Substring(i,1));
            entries.Add((guessWord.Substring(i,1), givenWord.Substring(i,1)));
        }

        // reinitialize all the text fields to be empty before repopulating them with the updated list of entries
        journalText.text = "";
        alienText.text = "";
        equalsText.text = "";
        journalText2.text = "";
        alienText2.text = "";
        equalsText2.text = "";

        createJournal(); // recreate the journal text with the new entry so it's sorted
    }

}
