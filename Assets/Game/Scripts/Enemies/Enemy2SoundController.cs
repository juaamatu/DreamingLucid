using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls sounds on Spike Enemy
public class Enemy2SoundController : MonoBehaviour {

    public AudioClip[] idleSounds;      //array of idle sounds
    public AudioSource myIdleSounds;    //audiosource of idle sounds    
    private bool wait = false;          //bool to toggle waiting

	void Update () {
		
        if (!myIdleSounds.isPlaying && idleSounds.Length > 0 && !wait) //play idle sounds and then wait
        {
            myIdleSounds.PlayOneShot(idleSounds[Random.Range(0, idleSounds.Length)]);
            wait = true;
            StartCoroutine(WaitSome());
        }

	}

    IEnumerator WaitSome() //wait for 1 second before playing idle sounds again
    {
        yield return new WaitForSeconds(1f);
        wait = false;
    }
}
