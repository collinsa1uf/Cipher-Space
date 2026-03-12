using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Journal : MonoBehaviour
{
    //[SerializeField] private GameObject journalObject; // the journal UI object to toggle on and off
    [SerializeField] private GameObject journalObject;

    public bool IsOpen => journalObject.activeSelf;
    [SerializeField] private TextMeshProUGUI alienText; // left page col 1 alien text 
    [SerializeField] private TextMeshProUGUI journalText; // left page col 1 translation
    [SerializeField] private TextMeshProUGUI equalsText;
    [SerializeField] private TextMeshProUGUI alienText2; // col 2 alien text
    [SerializeField] private TextMeshProUGUI journalText2; // col 2 translation
    [SerializeField] private TextMeshProUGUI equalsText2;
    [SerializeField] private TextMeshProUGUI alienText3; // right page alien text
    [SerializeField] private TextMeshProUGUI journalText3; // right page translation
    [SerializeField] private TextMeshProUGUI equalsText3;

    [SerializeField] private GameObject toggleTab; // UI button to toggle journal on and off

    [SerializeField] private PasswordManager passwordManager; // Reference to the PasswordManager component
    [SerializeField] private TranslationManager translationManager;

    List<(string letter, string guess)> entries = new List<(string, string)> {}; // list of journal entries
    // each entry is a tuple of the human letter and the player's guess for the alien letter visually represented 

    void Start()
    {
        journalObject.SetActive(false); // Ensure the journal is hidden at the start
        toggleTab.SetActive(true); // Ensure the toggle button is visible

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
        //if (passwordManager != null && passwordManager.gameObject.activeSelf) return; // Don't allow toggling the journal if the password manager is open
        if (translationManager != null && translationManager.gameObject.activeSelf) return; // Don't allow toggling the journal if the translation manager is open
        if (GameStateManager.InputLocked) return;
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            journalObject.SetActive(!journalObject.activeSelf);
            toggleTab.SetActive(!toggleTab.activeSelf);
        }

    }
    void createJournal() // recreates the journal text with the current list of entries 
    {
        entries.Sort((a, b) => a.letter.CompareTo(b.letter)); // sort entries alphabetically
        int num_count = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            if (int.TryParse(entries[i].letter, out int result)) {
                num_count++;
                journalText3.text += entries[i].letter + "\n";
                alienText3.text += entries[i].guess + "\n";
                equalsText3.text += "=\n";
            }

            // split entries between the two columns if there are more than 13 entries, not counting the numbers which are on a separate page
            else if (i-num_count < 13)
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
        journalText3.text = "";
        alienText3.text = "";
        equalsText3.text = "";

        createJournal(); // recreate the journal text with the new entry so it's sorted
    }

}
