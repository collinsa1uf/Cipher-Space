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

    List<string> entries = new List<string> {}; // list of journal entries to be displayed in the journal, populated as the user discovers new letters

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
        entries.Sort(); // sort entries alphabetically
        int index = 0;

        foreach (string entry in entries)
        {
            // split entries between the two pages if there are more than 13 entries
            if (index < 13)
            {
                journalText.text += entry + "\n";
                alienText.text += CipherGeneration.Encrypt(entry) + "\n";
                equalsText.text += "=\n";
            }
            else {
                journalText2.text += entry + "\n";
                alienText2.text += CipherGeneration.Encrypt(entry) + "\n";
                equalsText2.text += "=\n";
            }
            index++;
        }
    }
    public void UpdateJournalText(string discWord)
    {
        for (int i = 0; i < discWord.Length; i++)
        {
            if (!entries.Contains(discWord.Substring(i,1)))
            { 
                entries.Add(discWord.Substring(i,1));
            }
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
