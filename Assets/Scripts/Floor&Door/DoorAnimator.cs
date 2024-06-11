using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    private const string openLeftAnimation = "DoorOpensToKitchen"; 
    private const string openRightAnimation = "DoorOpensToOtherRoom"; 
    private const string closedAnimation = "DoorStaysClosed";
    [SerializeField] private Door door;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (door == null)
        {
            Debug.LogWarning("Door reference is not assigned in the DoorAnimator script.");
            return;
        }
        if (door.PlayerIsThere())
        {
            if (door.FromKitchen())
            {
                Debug.Log("Playing OpenRight Animation");
                _animator.Play(openRightAnimation);
            }
            else
            {
                Debug.Log("Playing OpenLeft Animation");
                _animator.Play(openLeftAnimation);
            }
        }
        else
        {
            Debug.Log("Playing Close Animation");
            _animator.Play(closedAnimation);
        }
    }
}