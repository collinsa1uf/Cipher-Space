// using UnityEngine; 
// using TMPro; 
// [RequireComponent(typeof(TextMeshProUGUI))] 
// public class EncryptText : MonoBehaviour 
// { 
//     private TextMeshProUGUI tmp; 
//     private string originalText; 
//     void OnEnable() 
//     { 
//         tmp = GetComponent<TextMeshProUGUI>(); 
//         originalText = tmp.text; 
//         Encrypt(); 
//     } 
//     public void Encrypt() 
//     { 
//         tmp.text = CipherGeneration.Encrypt(originalText); 
//     } 
// }

using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EncryptText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private string originalText;
    private bool initialized = false;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        CacheOriginal();
    }

    private void CacheOriginal()
    {
        if (!initialized)
        {
            originalText = tmp.text;
            initialized = true;
        }
    }

    /// <summary>
    /// Encrypts using the cached original text.
    /// Safe to call multiple times.
    /// </summary>
    public void Encrypt()
    {
        CacheOriginal();
        tmp.text = CipherGeneration.Encrypt(originalText);
    }

    /// <summary>
    /// Restores original (unencrypted) text.
    /// </summary>
    public void ResetToOriginal()
    {
        CacheOriginal();
        tmp.text = originalText;
    }

    /// <summary>
    /// Updates the original text (use this if you manually change .text elsewhere)
    /// </summary>
    public void SetNewOriginal(string newText)
    {
        originalText = newText;
        tmp.text = newText;
    }
}