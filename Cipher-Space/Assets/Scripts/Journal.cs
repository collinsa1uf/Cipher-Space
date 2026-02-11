using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Journal : MonoBehaviour
{
    [SerializeField] private GameObject journalObject; // assign the UI panel or root GameObject in the Inspector
    [SerializeField] private TextMeshProUGUI journalText;
    [SerializeField] private GameObject toggleJ;

    [SerializeField] private PasswordManager passwordManager; // Reference to the PasswordManager component

    List<string> entries = new List<string>
     {
        "A=F",
        "6=7",
        "F=R",
        "T=Y"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        journalObject.SetActive(false); // Ensure the journal is hidden at the start
        toggleJ.SetActive(true); // Ensure the toggle button is visible
        journalText.text = ""; // initialize journal text
        createJournal(); // populate journal with initial entries
        UpdateJournalText("M=C"); // Example of adding a new entry, testing if it sorts correctly
    }

    // Update is called once per frame
    void Update()
    {
        if (passwordManager != null && passwordManager.gameObject.activeSelf) return; // Don't allow toggling the journal if the password manager is open
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            journalObject.SetActive(!journalObject.activeSelf);
            toggleJ.SetActive(!toggleJ.activeSelf);
        }

    }
    void createJournal()
    {
        entries.Sort();
        foreach (string entry in entries)
        {
            journalText.text += entry + "\n";
        }
    }
    void UpdateJournalText(string newEntry)
    {
        entries.Add(newEntry);
        journalText.text = "";
        createJournal(); // recreate the journal text with the new entry so it's sorted
    }

}
