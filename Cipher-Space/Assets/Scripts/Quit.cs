using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    // Quits game (does not work in editor)
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
