using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        
        volume =  PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlayeSound(audioClipRefsSO.trash,trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {   
        BaseCounter baseCounter = sender as BaseCounter;
        PlayeSound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlayeSound(audioClipRefsSO.objectPickup,Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {   
        // Because the sender is already known as CuttingCounter
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlayeSound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlayeSound(audioClipRefsSO.deliveryFail,deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
       PlayeSound(audioClipRefsSO.deliverySuccess,deliveryCounter.transform.position);
    }

    private void PlayeSound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {   
        // this method is a 3d sound play method you can choose where the sound effect will be played
        // if you want it play globally , just use Camera.main.transform.position
        
        //This is a static function that doesn't need to be attached to any specific GameObject or script and can be called directly from anywhere
        //The function works by creating a temporary GameObject at the specified position, attaching an AudioSource component to it, and playing the provided audio clip on this AudioSource. Once the audio finishes playing, the temporary GameObject is automatically destroyed, and you don't need to manually manage the AudioSource component.
        AudioSource.PlayClipAtPoint(audioClip,position,volumeMultiplier * volume);
    }  
    
    private void PlayeSound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {   
        PlayeSound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }

    public void PlayFootstepSound(Vector3 position ,float volume)
    {
        PlayeSound(audioClipRefsSO.footstep,position,volume);
    }
    public void PlayCountdownSound()
    {
        PlayeSound(audioClipRefsSO.warning,Vector3.zero);
    }


    public void  ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
