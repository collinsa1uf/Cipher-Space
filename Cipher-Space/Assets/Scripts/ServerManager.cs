using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    private Process serverProcess;  
    public LoadScene loadScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        loadScene = FindObjectOfType<LoadScene>();
        StartServer();
        StartCoroutine(WaitForServer());
    }

    void StartServer()
    {
        string serverPath = Path.Combine(Application.dataPath, "../server.exe");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Path.GetFullPath(serverPath),
            UseShellExecute = false,
            CreateNoWindow = true
        };

        serverProcess = Process.Start(startInfo);
    }

    IEnumerator WaitForServer()
    {
        UnityEngine.Debug.Log("Waiting for server...");
        
        while (true)
        {
            using (var request = UnityEngine.Networking.UnityWebRequest.Get("http://localhost:8000"))
            {
                request.timeout = 2;
                yield return request.SendWebRequest();

                if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success || 
                    request.responseCode > 0)
                {
                    UnityEngine.Debug.Log("Server is ready!");
                    loadScene.LoadSceneByIndex();
                    yield break;
                }
            }

            UnityEngine.Debug.Log("Server not ready yet, retrying...");
            yield return new WaitForSeconds(2f);
        }
    }

    void OnApplicationQuit()
    {
        if (serverProcess != null && !serverProcess.HasExited)
        {
            // Kill the entire process tree, not just the parent
            Process killProcess = new Process();
            killProcess.StartInfo = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/PID {serverProcess.Id} /T /F",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            killProcess.Start();
            killProcess.WaitForExit();
            serverProcess.Dispose();
        }
    }
}
