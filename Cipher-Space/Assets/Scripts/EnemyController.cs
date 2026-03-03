using UnityEngine;
using System.Collections;
using System.Threading;

public class EnemyController : MonoBehaviour
{
    public EnemyTimer enemyTimer;

    public Transform hiddenSpawnPoint;
    public float visibleOffset = 60f;

    private Animator animator;
    private PlayerMovement playerMovement;

    public GameObject blackScreen;

    private Vector3 exitPoint = new Vector3(287.7f, -120f, 0f);
    public float moveSpeed = 80f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate(bool playerHidden, Transform player)
    {
        gameObject.SetActive(true);
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

        animator.SetBool("IsMoving", true);

        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", -1f);

        while (transform.position.y > exitPoint.y)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                exitPoint,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        animator.SetBool("IsMoving", false);
        gameObject.SetActive(false);
        enemyTimer.RestartTimer();
    }

    public void OnKillFinished() { 
        blackScreen.SetActive(true);
    }
}
