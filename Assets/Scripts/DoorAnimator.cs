using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    public Animator animator; 
    public string openLeftAnimation = "DoorOpensToKitchen"; 
    public string openRightAnimation = "DoorOpensToOtherRoom"; 
    public string closedAnimation = "DoorStaysClosed"; 
    public LayerMask playerLayerMask; 
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
        Vector3 doorPosition = transform.position;

        // Cast a ray to the left
        RaycastHit hit;
        if (Physics.Raycast(doorPosition, Vector3.left, out hit, detectionRange, playerLayerMask))//cu un obiect(boxxast) sau creez o functie un ontrigger enter on trigger exit
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerIsThere = true;
                fromKitchen = false;
                return;
            }
        }

        // Cast a ray to the right
        if (Physics.Raycast(doorPosition, Vector3.right, out hit, detectionRange, playerLayerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerIsThere = true;
                fromKitchen = true;
                return;
            }
        }

        playerIsThere = false;
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