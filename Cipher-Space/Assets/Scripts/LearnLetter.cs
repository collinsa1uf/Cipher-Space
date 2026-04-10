using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LearnLetter : MonoBehaviour
{
    private List<char> mostCommonLetters = new List<char>(){'E', 'A', 'T', 'S', 'N', 'L', 'C'};
    public Dictionary<char, char> cipher = new Dictionary<char, char>();
    public Journal journal;

    void Start()
    {
        cipher = CipherGeneration.GetLetterCipher();
    }

    public void LearnRandomLetter()
    {
        if (mostCommonLetters.Count == 0) return;

        int randomIndex = Random.Range(0, mostCommonLetters.Count);
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
}
