using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientManager : MonoBehaviour
{
    
    private string baseUrl = "http://localhost:8000";
    private string generateEndpoint = "/generate_objects";
    private int timeoutSeconds = 30;
    private bool requestInProgress = false;

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
        Debug.Log(responseJson);
        requestInProgress = false;
    }
}