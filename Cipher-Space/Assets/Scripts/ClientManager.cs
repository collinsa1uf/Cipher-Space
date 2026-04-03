using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ClientManager : MonoBehaviour
{

    [Header("Events")]
    public UnityEngine.Events.UnityEvent onPuzzleReceived;
    
    private string baseUrl = "http://localhost:8000";
    private string generateEndpoint = "/generate_objects";
    private int timeoutSeconds = 300;
    private bool requestInProgress = false;
    public static Dictionary<string, string> objects;
    public TextMeshProUGUI requestStatus;

    public DialogueManager dialogueManager;

    void Start()
    {
        objects = new Dictionary<string, string>()
        {

            //break room objects
            ["mugObject"] = "null",
            ["pitcherObject"] = "null",
            ["TVObject"] = "null",
            ["chipsObject"] = "null",

            //other objects
            ["boardObject"] = "null",
            ["crateObject"] = "null",

            //medbay objects
            ["vialObject"] = "null",
            ["vitalsObject"] = "null",

            //engine room objects
            ["liquidObject"] = "null",
            ["computerObject"] = "null",
            ["circuitObject"] = "null",
            ["toolsObject"] = "null",
            ["screwsObject"] = "null",
            
            //passwords
            ["jailCode"] = JailPassword.jailPassword,
            ["engineCode"] = "null",
            ["medbayCode"] = "null",
            ["cockpitCode"] = "null",
            ["leverCode"] = "null",
            ["buttonCode"] = "null",
            ["blueprintCode"] = "null"
        };
        RequestPuzzle();
    }

    public void RequestPuzzle() // Called at start of game
    {
        if (requestInProgress) // Only send one request
        {
            return;
        }
        StartCoroutine(SendGenerateRequest());
    }
    private UnityWebRequest currentRequest;
    private IEnumerator SendGenerateRequest()
    {
        requestInProgress = true;
        string url = baseUrl + generateEndpoint;

        string jsonBody = "{}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody); // Convert JSON string to byte array

        currentRequest = new UnityWebRequest(url, "POST"); // Create POST request

        currentRequest.uploadHandler = new UploadHandlerRaw(bodyRaw); // Set the request body
        currentRequest.downloadHandler = new DownloadHandlerBuffer();
        currentRequest.SetRequestHeader("Content-Type", "application/json"); // Set content type header
        currentRequest.timeout = timeoutSeconds;

        Debug.Log($"Sending request to: {url}");

        yield return currentRequest.SendWebRequest(); // Wait for the request to complete

        if (currentRequest == null) yield break; // Aborted mid request

        if (currentRequest.result != UnityWebRequest.Result.Success) // Check for errors
        {
            Debug.LogError($"[ClientManager] Error: {currentRequest.error}");
            yield break;
        }

        string responseJson = currentRequest.downloadHandler.text; // Get the response text
        requestInProgress = false;
        currentRequest.Dispose();
        currentRequest = null;
        ParseJSON(responseJson);
    }

    public void ParseJSON(string responseJson)
    {
        int dataStart = responseJson.IndexOf("\"data\":");
        if (dataStart == -1)
        {
            return;
        }

        int braceStart = responseJson.IndexOf('{', dataStart);
        int braceEnd = responseJson.IndexOf('}', braceStart);

        if (braceStart == -1 || braceEnd == -1)
        {
            return;
        }

        string dataContent = responseJson.Substring(braceStart + 1, braceEnd - braceStart);
        string[] pairs = dataContent.Split(',');

        foreach (string pair in pairs)
        {
            string[] kvPair = pair.Split(':');
            if (kvPair.Length != 2) continue;

            string key = kvPair[0].Trim().Replace("\"", "");
            string value = kvPair[1].Trim().Replace("\"", "");
            value = value.TrimEnd('}');

            if (objects.ContainsKey(key))
            {
                objects[key] = value;
            }
        }

        foreach (var kv in objects)
        {
            Debug.Log($"{kv.Key} = {kv.Value}");
        }

        StartCoroutine(StartUnityEvent());
    }

    private IEnumerator StartUnityEvent()
    {
        while (dialogueManager.isInDialogue)
        {
            yield return null;
        }

        onPuzzleReceived?.Invoke();
    }

    void OnDestroy()
    {
        if (currentRequest != null)
        {
            currentRequest.Abort();
            currentRequest.Dispose();
            currentRequest = null;
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        requestStatus.text += $"\n[{type}] {message}";

        string[] lines = requestStatus.text.Split('\n');
        if (lines.Length > 20)
            requestStatus.text = string.Join("\n", lines[^20..]);
    }
}