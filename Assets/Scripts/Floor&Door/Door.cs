using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    public float detectionRange = 5f;

    private bool playerIsThere = false;
    private bool fromKitchen = false;

    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector3 doorPosition = transform.position;

        // Cast a ray to the left
        RaycastHit hit;
        
        if (Physics.Raycast(doorPosition, Vector3.left, out hit, detectionRange, playerLayerMask))
        {
            if (hit.collider != null && hit.collider.GetComponent<Collider>() != null)
            {
                Debug.Log("Player detected to the left: " + hit.collider.name);
                playerIsThere = true;
                fromKitchen = false;
                return;
            }
            else
            {
                Debug.Log("Hit something to the left, but it's not the player: " + hit.collider.name);
            }
        }

        // Cast a ray to the right
        
        if (Physics.Raycast(doorPosition, Vector3.right, out hit, detectionRange, playerLayerMask))
        {
            if (hit.collider != null && hit.collider.GetComponent<Collider>() != null)
            {
                Debug.Log("Player detected to the right: " + hit.collider.name);
                playerIsThere = true;
                fromKitchen = true;
                return;
            }
            else
            {
                Debug.Log("Hit something to the right, but it's not the player: " + hit.collider.name);
            }
        }

        playerIsThere = false;
        Debug.Log("Player not detected");
    }

    public bool PlayerIsThere()
    {
        return playerIsThere;
    }

    public bool FromKitchen()
    {
        return fromKitchen;
    }
}
