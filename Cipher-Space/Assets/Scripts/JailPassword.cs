using UnityEngine;
using System.Collections.Generic;

public class JailPassword : MonoBehaviour
{
    public static string jailPassword = "";
    public Lock lockObject;
    void Start()
    {
        List<char> digits = new List<char>("123456789".ToCharArray());
        System.Random rng = new System.Random();

        int n = digits.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);

            char value = digits[k];
            digits[k] = digits[n];
            digits[n] = value;
        }

        string passcode = new string(digits.ToArray());
        lockObject.password = passcode;
        lockObject.message = passcode;
    }
}
