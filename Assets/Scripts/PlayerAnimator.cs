using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    
    private const string IsWalking = "IsWalking";
    [SerializeField] private Player player;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsWalking,player.IsWalking());
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        _animator.SetBool(IsWalking,player.IsWalking());
        
    }
}
