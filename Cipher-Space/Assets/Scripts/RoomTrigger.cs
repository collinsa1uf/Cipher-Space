using UnityEngine;
using TMPro;

public class RoomTrigger : MonoBehaviour
{
    public bool playerInside; // Track if the player is currently inside the room
    public RoomEnemyData roomData; // Reference to the room's enemy choreo
    public TextMeshProUGUI roomTag; // Reference to the TextMeshProUGUI component for displaying the room tag
    public FadeTransition appears;
    public FadeTransition disappears;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyTimer.Instance.SetRoom(roomData);
            EnemyTimer.Instance.TryActivateTimer();

            playerInside = true;

            roomTag.text = roomData.roomName; // Update the room tag text to display the current room's name
            appears.Fade();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyTimer.Instance.ClearRoom();
            playerInside = false;

            disappears.Fade();
        }
    }
}
