using UnityEngine;

public class LogConsole : MonoBehaviour
{
    public void LogMessage()
    {
        string word = "Cup";
        string encryptedWord = CipherGeneration.Encrypt(word);
        Debug.Log("Word: " + word + ", Encrypted: " + encryptedWord); // Logs the provided message to the Unity console
    }
}
