using UnityEngine;

public class ParsingAI : MonoBehaviour
{
    public static ParsingAI Instance;
    private void Awake()
    {
        Instance = this;
    }

    public string[] objectDescriptors =
    {
        "MUG",
        "LMNOPQRS",
        "MEMO"
    };
}
