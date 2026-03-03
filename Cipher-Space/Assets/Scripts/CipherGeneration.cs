using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CipherGeneration : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI textToEncrypt;
    public static Dictionary<char, char> letterCipherMap;
    public static Dictionary<char, char> numberCipherMap;
    
    void Start()
    {
        GenerateLetterCipher();
        GenerateNumberCipher();
    }
    public static void GenerateLetterCipher()
    {
        List<char> letters = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        List<char> shuffledLetters = new List<char>(letters);
        System.Random rng = new System.Random();
        int n = shuffledLetters.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            char value = shuffledLetters[k];
            shuffledLetters[k] = shuffledLetters[n];
            shuffledLetters[n] = value;
        }

        Dictionary<char, char> randomCipherMap = new Dictionary<char, char>();
        for (int i = 0; i < letters.Count; i++)
        {
            randomCipherMap[letters[i]] = shuffledLetters[i];
        }
        letterCipherMap = randomCipherMap;

        Debug.Log("Generated Cipher:");
        foreach (var pair in randomCipherMap)        {
            Debug.Log(pair.Key + " -> " + pair.Value);
        }
    }

    public static void GenerateNumberCipher()
    {
        List<char> letters = new List<char>("123456789".ToCharArray());
        List<char> shuffledLetters = new List<char>(letters);
        System.Random rng = new System.Random();
        int n = shuffledLetters.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            char value = shuffledLetters[k];
            shuffledLetters[k] = shuffledLetters[n];
            shuffledLetters[n] = value;
        }

        Dictionary<char, char> randomCipherMap = new Dictionary<char, char>();
        for (int i = 0; i < letters.Count; i++)
        {
            randomCipherMap[letters[i]] = shuffledLetters[i];
        }
        numberCipherMap = randomCipherMap;

        Debug.Log("Generated Cipher:");
        foreach (var pair in randomCipherMap)        {
            Debug.Log(pair.Key + " -> " + pair.Value);
        }
    }

    public static string Encrypt(string input)
    {
        // if (letterCipherMap == null)
        // {
        //     GenerateLetterCipher(); // Ensure the cipher map is generated before encryption
        // }

        // if (numberCipherMap == null)
        // {
        //     GenerateNumberCipher(); // Ensure the cipher map is generated before encryption
        // }

        string encrypted = "";
        foreach (char c in input)
        {
            char upperChar = char.ToUpper(c);

            if (letterCipherMap.ContainsKey(upperChar))
            {
                encrypted += letterCipherMap[upperChar];
            }
            else if (numberCipherMap.ContainsKey(upperChar))
            {
                encrypted += numberCipherMap[upperChar];
            }
            else
            {
                encrypted += c;
            }
        }
        return encrypted;
    }
}