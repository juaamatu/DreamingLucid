using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrbSoundController : MonoBehaviour {

    public AudioClip[] whooshSounds;
    public AudioSource myWhooshSounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (whooshSounds.Length > 0 && !myWhooshSounds.isPlaying)
        {
            myWhooshSounds.PlayOneShot(whooshSounds[Random.Range(0, whooshSounds.Length)]);
        }
        else if (whooshSounds.Length == 0)
        {
            Debug.Log("No sounds on array!");
        }
	}
}
