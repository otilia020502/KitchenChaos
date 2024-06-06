using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator; 
    public string openLeftAnimation = "DoorOpensToKitchen"; 
    public string openRightAnimation = "DoorOpensToOtherRoom"; 
    public string closedAnimation = "DoorStaysClosed"; 
    public Transform player; 
    public float detectionRange = 2f; 

    private bool playerIsThere = false;
    private bool fromKitchen = false;

    void Update()
    {
        DetectPlayer();
        PlayDoorAnimation();
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        playerIsThere = distanceToPlayer <= detectionRange;

        if (playerIsThere)
        {
            Vector3 doorToPlayer = player.position - transform.position;
            fromKitchen = doorToPlayer.x > 0;
        }
    }

    private void PlayDoorAnimation()
    {
        if (playerIsThere)
        {
            if (fromKitchen)
            {
                animator.Play(openRightAnimation);
            }
            else
            {
                animator.Play(openLeftAnimation);
            }
        }
        else
        {
            animator.Play(closedAnimation);
        }
    }
}