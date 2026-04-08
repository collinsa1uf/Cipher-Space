using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static bool InputLocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputLocked = false; // reset on scene load
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
