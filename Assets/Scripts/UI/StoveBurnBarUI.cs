using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter _stoveCounter;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgresChanged;
        _animator.SetBool(IS_FLASHING,false);
    }

    private void StoveCounter_OnProgresChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        bool show =_stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
        _animator.SetBool(IS_FLASHING, show);
    }

   
}


