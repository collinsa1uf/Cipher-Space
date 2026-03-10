using UnityEngine;

public class RoomEnemyData : MonoBehaviour
{
    private RoomEnemyData roomData;
    [Header("Timer")]
    public float countdownDuration = 30f;

    [Header("Enemy Spawn")]
    public Transform hiddenSpawnPoint;

    [Header("Enemy Exit")]
    public Vector3 exitPoint;

    [Header("Enemy Movement")]
    public float moveSpeed = 80f;


    private void Awake()
    {
        roomData = GetComponent<RoomEnemyData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        EnemyTimer.Instance.SetRoom(roomData);
    }
}
