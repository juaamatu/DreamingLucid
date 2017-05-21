using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls ambient sounds
[RequireComponent(typeof(AudioSource))]
public class AmbientSoundController : MonoBehaviour {
    public AudioSource audioSource1;
    public AudioClip[] ambientSounds;
    private bool toggle = false;


    void Update() 
    {
        if (!toggle) //waits for a while, before playing another sound
        {
            StartCoroutine(WaitForSoundPlaying());
            toggle = true;
        }
    }

    IEnumerator WaitForSoundPlaying() //wait loop
    {
        yield return new WaitForSeconds(3f);
        PlaySound();
        toggle = false;
    }

    void PlaySound()
    {
        if (!audioSource1.isPlaying) //plays sound
        {
            audioSource1.PlayOneShot(ambientSounds[Random.Range(0, ambientSounds.Length)]);
        }
    }
}
