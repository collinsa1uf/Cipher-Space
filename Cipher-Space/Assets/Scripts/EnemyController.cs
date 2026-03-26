using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Room-Dependent Settings")]
    private Transform hiddenSpawnPoint; 
    private float moveSpeed = 80f;
    public float visibleOffset = 60f;
    private Vector3 exitPoint;

    private Animator animator;
    private PlayerMovement playerMovement;

    //public GameObject blackScreen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate(bool playerHidden, Transform player)
    {
        if (hiddenSpawnPoint == null) // catch if spawn point is not assigned to avoid null reference errors
        {
            Debug.LogError("Hidden spawn point not assigned for this room!");
            return;
        }

        gameObject.SetActive(true); // Activate the enemy object

        if (playerHidden)
        {
            transform.position = hiddenSpawnPoint.position;
            StartCoroutine(HiddenRoutine());
        }
        else
        {
            transform.position = new Vector3(
                player.position.x - visibleOffset,
                player.position.y,
                player.position.z
            );

            // freeze player movement when enemy spawns
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = false;

            Debug.Log("Kill triggered");
            animator.SetTrigger("Kill");
            player.gameObject.SetActive(false);
        }
    }

    IEnumerator HiddenRoutine()
    {
        yield return new WaitForSeconds(2f);

        // Set the current room so we can open the correct door
        RoomEnemyData currentRoom = EnemyTimer.Instance.GetCurrentRoom();

        DoorController door = null;
        bool doorWasOpen = false;

        if (currentRoom != null && currentRoom.roomDoor != null) // null checks
        {
            door = currentRoom.roomDoor; // set current door for this room

            doorWasOpen = door.IsOpen; // track state of the door so we can restore it after the enemy leaves

            if (!doorWasOpen) // only open the door if it was closed to begin with, otherwise we might mess with the player's progress
            {
                door.OpenDoor();
                yield return new WaitForSeconds(0.5f);
            }
        }


        animator.SetBool("IsMoving", true);

        Vector2 direction = (exitPoint - transform.position).normalized;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        while (Vector3.Distance(transform.position, exitPoint) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                exitPoint,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        animator.SetBool("IsMoving", false);

        // Close door only if enemy opened it
        if (door != null && !doorWasOpen)
        {
            door.CloseDoor();
        }

        gameObject.SetActive(false);

        EnemyTimer.Instance.enemyRoutineActive = false;
        EnemyTimer.Instance.RestoreUI();
        GameStateManager.InputLocked = false;
    }

    public void OnKillFinished() { 
        //blackScreen.SetActive(true);
    }

    // Setters for room data
    public void SetExitPoint(Vector3 point)
    {
        exitPoint = point;
    }
    public void SetSpawnPoint(Transform spawn)
    {
        hiddenSpawnPoint = spawn;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

}
