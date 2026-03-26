using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomEnemyData : MonoBehaviour
{
    private RoomEnemyData roomData;
    [Header("Timer")]
    public float countdownDuration = 30f;

    [Header("Enemy Spawn")]
    public Transform hiddenSpawnPoint;

    [Header("Enemy Exit")]
    public Transform exitPoint;

    [Header("Enemy Movement")]
    public float moveSpeed = 80f;

    [Header("Room Lighting")]
    public Light2D[] roomLights;
    public Light2D[] allLights;

    [Header("Door")]
    public DoorController roomDoor;
}

