using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class EnemyTimer : MonoBehaviour
{
    [Header("Lighting")]
    private Light2D[] allLights;
    private Light2D[] sceneLights;

    [Header("Timer")]
    private float countdownDuration = 30f; 
    public float timeLeft;
    public bool timerRunning = false;
    public bool enemySystemEnabled = false;
    private RoomEnemyData lastRoom;

    [Header("Sprites")]
    public GameObject enemySprite;
    public EnemyController enemyController;
    public Transform player;
    private PlayerHiding playerHiding;
    public bool enemyRoutineActive = false;

    [Header("Audio")]
    public AudioSource alarmSound;
    [Range(0f, 1f)]
    public float startingVolume = 0.05f;
    private readonly float[] warningTimes = { 9f, 7f, 5f, 3f, 2f, 1f }; // Times at which to play warnings
    private int warningIndex = 0;

    [Header("UI")]
    private Dictionary<GameObject, bool> uiStateCache = new Dictionary<GameObject, bool>();
    public GameObject canvas;
    private int uiLayer;
    
    [Header("Rooms")]
    public static EnemyTimer Instance;
    private RoomEnemyData currentRoom;
    public RoomEnemyData startingRoom;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerHiding = player.GetComponent<PlayerHiding>(); // get reference to the PlayerHiding component on the player
        enemySprite.SetActive(false); // ensure enemy is hidden at the start
        alarmSound.volume = startingVolume; // set the initial volume of the alarm sound
        uiLayer = LayerMask.NameToLayer("UI"); // cache the UI layer index for later use

        SetRoom(startingRoom); // initialize the timer with the starting room's settings
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0f)
        {
            TryActivateTimer();
        }

        if (!timerRunning) return; // during enemy attack, timer stops and does not update
        timeLeft -= Time.deltaTime;
        
        // Debug.Log(timeLeft);
        

        // Check for alarm warnings
        if (warningIndex < warningTimes.Length && timeLeft <= warningTimes[warningIndex])
        {
            PlayWarning();
            warningIndex++;
        }
        if (timeLeft <= 0f)
        {
            timerRunning = false;
            StartCoroutine(SpawnSequence());
            
        }
    }

    void PlayWarning()
    {
        alarmSound.Play();

        // Increase volume each time (clamped)
        alarmSound.volume = Mathf.Clamp(
            alarmSound.volume + 0.15f,
            0f,
            1f
        );
    }

    IEnumerator SpawnSequence()
    {
        GameStateManager.InputLocked = true; // lock player input during spawn sequence
        enemyRoutineActive = true;
        yield return StartCoroutine(FlickerLights());

        // Store original intensities
        float[] originalIntensities = new float[allLights.Length];
        for (int i = 0; i < allLights.Length; i++)
            originalIntensities[i] = allLights[i].intensity;

        // FULL DARK
        for (int i = 0; i < allLights.Length; i++)
            allLights[i].intensity = 0f;

        yield return new WaitForSeconds(1.5f);

        // Lights snap back on
        for (int i = 0; i < allLights.Length; i++)
            allLights[i].intensity = originalIntensities[i];

        bool hidden = player.GetComponent<PlayerHiding>().getIsHiding();
        enemyController.Activate(hidden, player);
    }

    IEnumerator FlickerLights()
    {
        CacheUIState();
        HideUI(); // hide UI during flicker

        float duration = 1f;
        float elapsed = 0f;

        float[] originalIntensities = new float[sceneLights.Length];
        for (int i = 0; i < sceneLights.Length; i++)
            originalIntensities[i] = sceneLights[i].intensity;

        while (elapsed < duration)
        {
            for (int i = 0; i < sceneLights.Length; i++)
            {
                sceneLights[i].intensity = UnityEngine.Random.Range(0.05f, originalIntensities[i]);
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
            elapsed += UnityEngine.Random.Range(0.05f, 0.1f);
        }

        // Restore
        for (int i = 0; i < sceneLights.Length; i++)
            sceneLights[i].intensity = originalIntensities[i];
    }


    public void RestartTimer()
    {
        timeLeft = countdownDuration;
        warningIndex = 0;
        alarmSound.volume = 0.05f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    void HideUI()
    {
        canvas.SetActive(false);
    }

    public void RestoreUI()
    {
        canvas.SetActive(true);
        foreach (var entry in uiStateCache)
        {
            entry.Key.SetActive(entry.Value);
        }
    }

    void CacheUIState() // cache the active state of all UI objects so we can restore them after enemy attack
    {
        uiStateCache.Clear();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == uiLayer)
            {
                uiStateCache[obj] = obj.activeSelf;
            }
        }
    }

    public void SetRoom(RoomEnemyData room)
    {
        currentRoom = room;
        if (lastRoom != room)
        {
            countdownDuration = room.countdownDuration;
        }

        if (enemyController != null)
        {
            enemyController.SetSpawnPoint(room.hiddenSpawnPoint);
            enemyController.SetMoveSpeed(room.moveSpeed);
            enemyController.SetExitPoint(room.exitPoint.position);
            SetRoomLights(room.roomLights);
            SetAllLights(room.allLights);
        }
        
    }

    void SetRoomLights(Light2D[] lights)
    {
        sceneLights = lights;
    }
    void SetAllLights(Light2D[] lights)
    {
        allLights = lights;
    }

    // Prevention against restarting the timer with an overlapping trigger
    public void TryActivateTimer()
    {
        // System not enabled yet (JSON not loaded)
        if (!enemySystemEnabled) return;

        // Must be inside a room
        if (currentRoom == null) return;

        // Prevent restarting the timer mid countdown
        if (timerRunning) return;

        // Prevent activation during enemy routine
        if (enemyRoutineActive) return;

        // entering a NEW room, restart timer
        if (currentRoom != lastRoom)
        {
            RestartTimer();
            return;
        }

        // re-entering SAME room or object interaction, resume timer
        if (timeLeft > 0f)
        {
            StartTimer();
        }
        else
        {
            RestartTimer();
        }
    }

    public void ClearRoom()
    {
        lastRoom = currentRoom;
        currentRoom = null;

        if (timerRunning)
            StopTimer();
    }

    public RoomEnemyData GetCurrentRoom()
    {
        return currentRoom;
    }
    public void startEnemySystem()
    {
        enemySystemEnabled = true;
    }
}
