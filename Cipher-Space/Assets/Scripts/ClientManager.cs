using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientManager : MonoBehaviour
{

    [Header("Events")]
    public UnityEngine.Events.UnityEvent onPuzzleReceived;
    
    private string baseUrl = "http://localhost:8000";
    private string generateEndpoint = "/generate_objects";
    private int timeoutSeconds = 300;
    private bool requestInProgress = false;
    public static Dictionary<string, string> objects;

    public DialogueManager dialogueManager;

    void Start()
    {
        objects = new Dictionary<string, string>()
        {
            ["mugObject"] = "null",

            //break room objects
            ["pitcherObject"] = "null",
            ["TVObject"] = "null",
            ["boardObject"] = "null",
            ["chipsObject"] = "null",
            ["crateObject"] = "null",

            //medbay objects
            ["vialObject"] = "null",
            ["vitalsObject"] = "null",
            ["liquidObject"] = "null",

            //engine room objects
            ["computerObject"] = "null",
            ["circuitObject"] = "null",
            ["toolsObject"] = "null",
            ["screwsObject"] = "null",
            
            //passwords
            ["jailCode"] = JailPassword.jailPassword,
            ["engineCode"] = "null",
            ["medbayCode"] = "null",
            ["cockpitCode"] = "null"
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

    private IEnumerator SendGenerateRequest()
    {
        requestInProgress = true;
        string url = baseUrl + generateEndpoint;
        //string url = baseUrl;

        string jsonBody = "{}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody); // Convert JSON string to byte array

        using UnityWebRequest request = new UnityWebRequest(url, "POST"); // Create POST request

        request.uploadHandler = new UploadHandlerRaw(bodyRaw); // Set the request body
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json"); // Set content type header
        request.timeout = timeoutSeconds;

        Debug.Log($"Sending request to: {url}");

        yield return request.SendWebRequest(); // Wait for the request to complete

        if (request.result != UnityWebRequest.Result.Success) // Check for errors
        {
            Debug.LogError($"[ClientManager] Error: {request.error}");
            yield break;
        }

        string responseJson = request.downloadHandler.text; // Get the response text
        // Debug.Log(responseJson);
        requestInProgress = false;
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
}