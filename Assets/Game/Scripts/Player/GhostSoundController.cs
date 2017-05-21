using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Petro Pitkänen
// Controls the transition between ghost music and normal music
public class GhostSoundController : MonoBehaviour {

    public AudioMixerSnapshot ghostSounds;  //ghost snapshot
    public AudioMixerSnapshot normalSounds; //normal snapshot

    void Update()
    {
        if (PlayerEventHandler.IsGhost) //check if the player is a ghost
        {
            OnPlayerDeath();    //enable transition to ghost snapshot
        }
        if (!PlayerEventHandler.IsGhost) //check if the player is not a ghost
        {
            OnPlayerRespawn();  //enable transition to normal snapshot
        }
    }

    void OnPlayerDeath()    //transition to ghost snapshot
    {
        ghostSounds.TransitionTo(.1f);
    }

    void OnPlayerRespawn()  //transition to normal snapshot
    {
        normalSounds.TransitionTo(.1f);
    }
   
}


