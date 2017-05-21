using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Petro Pitkänen
// Controls music
[RequireComponent(typeof(AudioSource))]
public class BackgroundAudioController : MonoBehaviour {

    public AudioSource intro;       //audiosource for the intro
    public AudioSource normalMusic; //audiosource for normal music
    public AudioSource ghostMusic;  //audisource for ghostmusic
    public AudioClip[] introClip;   //audioclip for the intro
    private bool introPlayed = false;   //bool to check if the intro has been played
    private bool musicPlaying = false;  //bool to check if the music is playing
    public bool destroyOnLoad = false;  //bool to check if we want to continue the music playing between scenes

	void Awake () {

        if (!destroyOnLoad) //make the music not destroy on load if the bool is false
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        if (introClip.Length > 0) //checks if there is an intro for the music, and plays it
        {
            PlayIntro();
        }
        else if (introClip.Length == 0) //skips intro, if there is none
        {
            PlayMusic();
        }

	}

    void PlayIntro() //plays intro music
    {
        intro.PlayOneShot(introClip[Random.Range(0, introClip.Length)]);
        introPlayed = true;
        intro.volume = .5f;
    }

    void PlayMusic() //plays music
    {
       
        normalMusic.Play();
        ghostMusic.Play();
        musicPlaying = true;
        normalMusic.volume = .7f;
        ghostMusic.volume = .7f;
        
    }

    void Update ()
    {
      if (introPlayed && !intro.isPlaying && !musicPlaying)  //waits for the intro to finish and starts the music after it 
        {
            PlayMusic();
            intro.Stop();
            
        }
      if (musicPlaying && !normalMusic.isPlaying && !ghostMusic.isPlaying) //start the music again, if it has been disabled and re-enabled
        {
            PlayMusic();
        }
    }
}
