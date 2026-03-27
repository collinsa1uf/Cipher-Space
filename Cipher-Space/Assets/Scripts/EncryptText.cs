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
        Encrypt();
    }

    private void CacheOriginal()
    {
        if (!initialized)
        {
            originalText = tmp.text;
            initialized = true;
        }
    }
    public void Encrypt()
    {
        CacheOriginal();
        tmp.text = CipherGeneration.Encrypt(originalText);
    }

    public void ResetToOriginal()
    {
        CacheOriginal();
        tmp.text = originalText;
    }

    public void SetNewOriginal(string newText)
    {
        originalText = newText;
        tmp.text = newText;
    }

    public string EncryptInput(string input)
    {
        if (tmp == null) tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = CipherGeneration.Encrypt(input);
        return tmp.text;
    }
}