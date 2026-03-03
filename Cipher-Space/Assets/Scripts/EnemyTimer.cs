using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EnemyTimer : MonoBehaviour
{
    [Header("Lighting")]
    public Light2D[] allLights;
    public Light2D[] sceneLights;

    [Header("Timer")]
    public float countdownDuration = 30f; 
    private float timeLeft;
    private bool timerRunning = false;

    [Header("Sprites")]
    public GameObject enemySprite;
    public EnemyController enemyController;
    public Transform player;
    private PlayerHiding playerHiding;


    [Header("Audio")]
    public AudioSource alarmSound;
    [Range(0f, 1f)]
    public float startingVolume = 0.05f;
    private readonly float[] warningTimes = { 9f, 7f, 5f, 3f, 2f, 1f }; // Times at which to play warnings
    private int warningIndex = 0;

   

    void Start()
    {
        timeLeft = countdownDuration; // set countdown time
        playerHiding = player.GetComponent<PlayerHiding>(); // get reference to the PlayerHiding component on the player
        enemySprite.SetActive(false); // ensure enemy is hidden at the start
        timerRunning = true; // start the countdwon
        alarmSound.volume = startingVolume; // set the initial volume of the alarm sound
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerRunning) return; // during enemy attack, timer stops and does not update
        timeLeft -= Time.deltaTime;
        Debug.Log(timeLeft);

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
        float duration = 1f;
        float elapsed = 0f;

        float[] originalIntensities = new float[sceneLights.Length];
        for (int i = 0; i < sceneLights.Length; i++)
            originalIntensities[i] = sceneLights[i].intensity;

        while (elapsed < duration)
        {
            for (int i = 0; i < sceneLights.Length; i++)
            {
                sceneLights[i].intensity = Random.Range(0.05f, originalIntensities[i]);
            }

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            elapsed += Random.Range(0.05f, 0.1f);
        }

        // Restore
        for (int i = 0; i < sceneLights.Length; i++)
            sceneLights[i].intensity = originalIntensities[i];
    }


    public void RestartTimer()
    {
        timeLeft = countdownDuration;
        warningIndex = 0;
        alarmSound.volume = 0.15f;
        timerRunning = true;
    }
    
}
