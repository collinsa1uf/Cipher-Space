using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] public int sceneIndex;

    // Loads scene by index
    public void LoadSceneByIndex()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
