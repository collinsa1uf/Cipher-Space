using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TutorialCipher : MonoBehaviour
{
    public TextMeshProUGUI textToEncrypt;
    public static Dictionary<char, char> cipherMap = new Dictionary<char, char>(){
        {'A', 'Q'},
        {'B', 'W'},
        {'C', 'E'},
        {'D', 'R'},
        {'E', 'T'},
        {'F', 'Y'},
        {'G', 'U'},
        {'H', 'I'},
        {'I', 'O'},
        {'J', 'P'},
        {'K', 'A'},
        {'L', 'S'},
        {'M', 'D'},
        {'N', 'F'},
        {'O', 'G'},
        {'P', 'H'},
        {'Q', 'J'},
        {'R', 'K'},
        {'S', 'L'},
        {'T', 'Z'},
        {'U', 'X'},
        {'V', 'C'},
        {'W', 'V'},
        {'X', 'B'},
        {'Y', 'N'},
        {'Z', 'M'}, 
        {'1', '9'},
        {'2', '8'},
        {'3', '7'},
        {'4', '6'},
        {'5', '5'},
        {'6', '4'},
        {'7', '3'},
        {'8', '2'},
        {'9', '1'},
        {'0', '0'}
    };

    void Start()
    {
        textToEncrypt.text = Encrypt(textToEncrypt.text); // Example usage: Encrypt the text in the TextMeshProUGUI component at the start
    }

    public static string Encrypt(string input)
    {
        string encrypted = "";
        foreach (char c in input)
        {
            char upperChar = char.ToUpper(c);
            if (cipherMap.ContainsKey(upperChar))
            {
                encrypted += cipherMap[upperChar];
            }
            else
            {
                encrypted += c; // If character is not in the map, keep it unchanged
            }
        }
        return encrypted;
    }
}
