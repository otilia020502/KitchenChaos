using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    private const string IsWalking = "IsWalking";
    [SerializeField] private Player player;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsWalking,player.IsWalking());
    }
}
