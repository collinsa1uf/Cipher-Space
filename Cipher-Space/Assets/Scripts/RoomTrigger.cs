using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public bool playerInside; // Track if the player is currently inside the room
    public RoomEnemyData roomData; // Reference to the room's enemy choreo

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyTimer.Instance.SetRoom(roomData);
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyTimer.Instance.StopTimer();
            playerInside = false;
        }
    }
    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Player still inside room");
    //    }
    //}
}
