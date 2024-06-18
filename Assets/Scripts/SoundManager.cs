using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public float EffectsVolume= 1f;
    
    [SerializeField] private AudioClipRefsSo _audioClipRefsSo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSucces += DeliveryManager_OnRecipeSucces;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CutingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter= sender as TrashCounter;
        PlaySound(_audioClipRefsSo.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter= sender as BaseCounter;
        PlaySound(_audioClipRefsSo.objectDrop, baseCounter.transform.position);
    }


    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSo.objectPickUp, Player.Instance.transform.position);
    }

    private void CutingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_audioClipRefsSo.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipRefsSo.deliveryFailed, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSucces(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipRefsSo.deliverySuccess,  deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position, float volume = 1f)
    {
       PlaySound(audioClip[Random.Range(0, audioClip.Length)],position,volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume*EffectsVolume);
    }

    public void PlayFootsStepsSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipRefsSo.footstep, position, volume);
    }
}
