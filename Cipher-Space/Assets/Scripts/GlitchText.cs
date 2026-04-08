using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GlitchText : MonoBehaviour
{
    private string characters =  "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string prevText = "";
    private float timer = 0f;
    private bool isAlienText = false;
    private TMP_FontAsset prevFont;
    [SerializeField] public TextMeshProUGUI textMeshProUGUI;
    [SerializeField] public TMP_FontAsset font;

    [Header("Display Time Settings")]
    public float englishDisplayLength = 10f;
    public float alienDisplayLength = 1f;

    void Start()
    {
        prevText = textMeshProUGUI.text;
        prevFont = textMeshProUGUI.font;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= englishDisplayLength && !isAlienText) // Converts to alien text every (englishDisplayLength) seconds
        {
            ConvertToAlienText();
            isAlienText = true;
            timer = 0f;
        }
        else if (timer >= alienDisplayLength && isAlienText) // Converts back to English text after (alienDisplayLength) seconds
        {
            ConvertToEnglishText();
            isAlienText = false;
            timer = 0f;
        }
    }

    void ConvertToAlienText()
    {
        string alienText = "";
        string charSubstring = characters;
        Dictionary<char, char> characterEquiv = new Dictionary<char, char>();

        foreach (char c in textMeshProUGUI.text)
        {
            char c_lower = char.ToLower(c);
            int randomIndex = Random.Range(0, charSubstring.Length);

            if (characterEquiv.ContainsKey(c_lower)) // If letter is already associated with alien character, use that value for same letter
            {
                alienText += characterEquiv.GetValueOrDefault(c_lower);
            }
            else // If letter is not assigned to an alien character, assign it
            {
                alienText += charSubstring[randomIndex];
                characterEquiv.Add(c_lower, charSubstring[randomIndex]);
                charSubstring = charSubstring.Remove(randomIndex, 1);
            }
        }

        textMeshProUGUI.text = alienText;
        textMeshProUGUI.font = font;
    }

    void ConvertToEnglishText()
    {
        textMeshProUGUI.text = prevText;
        textMeshProUGUI.font = prevFont;
    }
}
