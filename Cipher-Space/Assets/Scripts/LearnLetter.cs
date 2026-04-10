using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LearnLetter : MonoBehaviour
{
    private List<char> mostCommonLetters = new List<char>(){'E', 'A', 'T', 'S', 'N', 'L', 'R'};
    public Dictionary<char, char> cipher = new Dictionary<char, char>();
    private int listSize;
    public Journal journal;

    void Start()
    {
        cipher = CipherGeneration.GetLetterCipher();
    }

    public void LearnRandomLetter()
    {
        if (mostCommonLetters.Count == 0) return;

        int randomIndex = Random.Range(0, listSize);
        char learnedLetter = mostCommonLetters[randomIndex];

        if (journal.ContainsLetter(learnedLetter.ToString()))
        {
            mostCommonLetters.RemoveAt(randomIndex);
            LearnRandomLetter();
            return;
        }

        char translated = cipher[learnedLetter];
        journal.AddEntry(learnedLetter.ToString(), translated.ToString());
        mostCommonLetters.RemoveAt(randomIndex); 
    }

    void Update()
    {
        listSize = mostCommonLetters.Count;
    }
}
