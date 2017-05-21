using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Petro Pitkänen
// Controls all the sounds that the player makes
public class PlayerSoundsController : MonoBehaviour
{

    public AudioClip[] stoneSounds;         //array of sounds for walking on stone
    public AudioClip[] woodSounds;          //array of sounds for walking on wood
    public AudioClip[] ghostSounds;         //array of sounds for moving as a ghost
    public AudioClip[] jumpSounds;          //array of sounds for jumping
    public AudioClip[] fallSounds;          //array of sounds for falling on ground
    public AudioClip[] deathSounds;         //array of sounds for dying
    public AudioClip[] keySounds;           //array of sounds for obtaining a key
    public AudioClip[] corpseWalkSounds;    //array of sounds for walking on a corpse
    public AudioClip[] respawnSound;        //array of sounds for respawning
    public AudioClip[] checkpointSound;     //array of sounds for entering a checkpoint
    public AudioClip[] lifeGainSound;       //array of sounds for obtaining a life
    public AudioClip[] ghostRespawnSound;   //array of sounds for respawning as a ghost
    public AudioClip[] ladderClimbSound;    //array of sounds for climbing ladders
    public AudioSource myWalkingSound;      //audiosource for walking
    public AudioSource myJumpSound;         //audiosource for jumping
    public AudioSource myFallingSound;      //audiosource for falling on ground
    public AudioSource myDeathSound;        //audiosource for dying
    public AudioSource myKeySound;          //audiosource for obtaining a key
    public AudioSource myGhostSound;        //audiosource for moving as a ghost
    public AudioSource myRespawnSound;      //audiosource for respawning
    public AudioSource myCheckpointSound;   //audiosource for entering a checkpoint
    public AudioSource myLifeGainSound;     //audiosource for obtaining a life
    public AudioSource myGhostRespawnSound; //audiosource for respawning as a ghost
    public AudioSource myLadderClimbSound;  //audiosource for climbing a ladder

    private bool isGhost = false;           //bool to check if the player is a ghost
    public bool isMoving = false;           //bool to check if the player is moving
    private float prevPos;                  //float of previous position
    private float curPos;                   //float of current position
    public bool inLadder = false;           //bool to check if player is in ladder

    public float minPitch = .95f;           //float of minimum pitch of randomizing
    public float maxPitch = 1.05f;          //float of maximum pitch of randomizing
    public float minVol = .90f;             //float of minimum volume of randomizing
    public float maxVol = 1.10f;            //float of maximum volume of randomizing

    void Start()
    {
        PlayerEventHandler.OnDeath += DeathActive;      //activate on players death
        PlayerEventHandler.OnRespawn += RespawnActive;  //activate on respawn
        PlayerEventHandler.OnJumpStart += PlayJump;     //activate on jumping
        PlayerEventHandler.OnFallEnd += PlayFall;       //activate on falling on ground
        PlayerEventHandler.OnGhostSpawn += GhostRespawnActive;  //activate on ghost respawn
          
    }

    void Update()
    {
        if (!isGhost) //play sounds when player is not a ghost
        {
            if (PlayerEventHandler.GameObjectUnderPlayer != null)
            {
                //checks if player is on stone and plays walking sound
                if (PlayerEventHandler.IsGrounded && isMoving && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("stoneFloor") && !isGhost) 
                {
                    PlayStone();
                }
                //checks if player is on wood and plays walking sound
                if (PlayerEventHandler.IsGrounded && isMoving && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("woodFloor") && !isGhost) 
                {
                    PlayWood();
                }
                //checks if player is on top of corpse and plays walking sound
                if (PlayerEventHandler.IsGrounded && isMoving && PlayerEventHandler.GameObjectUnderPlayer != null && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("Corpse") && !isGhost) 
                {
                    PlayCorpseWalk();
                }
                //checks if the player is in a ladder and pressing a vertical button and plays ladder climbing sounds
                if (inLadder && Input.GetButton("Vertical"))
                {
                    PlayLadder();
                }
            }
            if (!PlayerEventHandler.IsGrounded || !Input.GetButton("Horizontal") || !isMoving) //stops playing sound when player is not moving
            {
                myWalkingSound.Stop();
                myGhostSound.Stop();
            }
            
            
        }

        if (isGhost) //play sounds when player is a ghost
        {
            //checks if player is ghost and plays ghost movement sounds
            if (PlayerEventHandler.IsGrounded && Input.GetButton("Horizontal") && isMoving)
            {
                PlayGhost();
            }
        }

        curPos = transform.position.x;
        if (curPos == prevPos) //checks if the player is moving horizontally
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        prevPos = curPos;

        if (PlayerEventHandler.IsGhost) //checks if the player is a ghost
        {
            isGhost = true;
        }
        if (!PlayerEventHandler.IsGhost) //checks if the player is not a ghost
        {
            isGhost = false;
        }


    }

    void DeathActive() //checks when player dies and activates death and ghost sounds
    {
        if (deathSounds.Length > 0 && !myDeathSound.isPlaying)
        {
            myDeathSound.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
            
        }
    }
    void RespawnActive() //checks when player respawns and deactivates ghost sounds and plays respawn soudn
    {
        myGhostSound.Stop();
        myRespawnSound.PlayOneShot(respawnSound[Random.Range(0, respawnSound.Length)]);
    }

    void GhostRespawnActive() //checks when player respawns as a ghost and plays a sound for it
    {
        myGhostRespawnSound.PlayOneShot(ghostRespawnSound[Random.Range(0, ghostRespawnSound.Length)]);
    }

    void PlayStone() //play sound for walking on stone
    {     
        if (stoneSounds.Length > 0 && !myWalkingSound.isPlaying && isMoving)
        {
            myWalkingSound.PlayOneShot(stoneSounds[Random.Range(0, stoneSounds.Length)]);
            myWalkingSound.pitch = (Random.Range(minPitch, maxPitch));
            myWalkingSound.volume = (Random.Range(minVol, maxVol));  
        }
    }

    void PlayWood() //play sound for walking on wood
    {
        if (woodSounds.Length > 0 && !myWalkingSound.isPlaying && isMoving)
        {
            myWalkingSound.PlayOneShot(woodSounds[Random.Range(0, woodSounds.Length)]);
            myWalkingSound.pitch = (Random.Range(minPitch, maxPitch));
            myWalkingSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    void PlayCorpseWalk() //play sounds of walking on q corpse
    {
        if(corpseWalkSounds.Length > 0 && !myWalkingSound.isPlaying && isMoving)
        {
            myWalkingSound.PlayOneShot(corpseWalkSounds[Random.Range(0, corpseWalkSounds.Length)]);
            myWalkingSound.pitch = (Random.Range(minPitch, maxPitch));
            myWalkingSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    void PlayGhost() //play ghost movement sounds
    {
        if (ghostSounds.Length > 0 && !myGhostSound.isPlaying && isMoving)
        {
            myGhostSound.PlayOneShot(ghostSounds[Random.Range(0, ghostSounds.Length)]);
            myGhostSound.pitch = (Random.Range(minPitch, maxPitch));
            myGhostSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    void PlayJump() //play jumping sound
    {
        if (jumpSounds.Length > 0 && !myJumpSound.isPlaying && !isGhost)
        {
            myJumpSound.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
            myJumpSound.pitch = (Random.Range(1.1f, 1.2f));
            myJumpSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    void PlayFall() //play falling on surface sound
    {
        if (fallSounds.Length > 0 && !myFallingSound.isPlaying && !isGhost && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("stoneFloor")) //plays falling on stone sound
        {
            myFallingSound.PlayOneShot(fallSounds[Random.Range(0, fallSounds.Length)]);
            myFallingSound.pitch = (Random.Range(0.4f, 0.6f));
            myFallingSound.volume = (Random.Range(minVol, maxVol));
        }
        if (fallSounds.Length > 0 && !myFallingSound.isPlaying && !isGhost && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("Corpse")) //plays falling on corpse sound
        {
            myFallingSound.PlayOneShot(corpseWalkSounds[Random.Range(0, corpseWalkSounds.Length)]);
            myFallingSound.pitch = (Random.Range(0.4f, 0.6f));
            myFallingSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    void OnTriggerEnter2D(Collider2D collider) //play sounds when colliding with things
    {
        if (collider.gameObject.tag.Equals("Key") && !isGhost && keySounds.Length > 0) //play sound when collecting a key
        {
                myKeySound.PlayOneShot(keySounds[Random.Range(0, keySounds.Length)]);
        }
        if (collider.gameObject.tag.Equals("Checkpoint") && !isGhost && checkpointSound.Length > 0) //play sound when hitting a checkpoint
        {
            myCheckpointSound.PlayOneShot(checkpointSound[Random.Range(0, checkpointSound.Length)]);
        }
        if (collider.gameObject.tag.Equals("Life") && !isGhost && lifeGainSound.Length > 0) //play sound when collecting a life
        {
            myLifeGainSound.PlayOneShot(lifeGainSound[Random.Range(0, lifeGainSound.Length)]);
        }
        if (collider.gameObject.tag.Equals("Ladder") && !isGhost)
        {
            inLadder = true;
           
        }
    }

    void OnTriggerExit2D(Collider2D collider) //check when player is not in a ladder anymore
    {
        if (collider.gameObject.tag.Equals("Ladder"))
        {
            inLadder = false;
        }
    }
    
    void PlayLadder()   //play sound for climbing a ladder
    {
        if (ladderClimbSound.Length > 0 && !myLadderClimbSound.isPlaying && inLadder)
        {
            myLadderClimbSound.PlayOneShot(ladderClimbSound[Random.Range(0, ladderClimbSound.Length)]);
            myLadderClimbSound.pitch = (Random.Range(minPitch, maxPitch));
            myLadderClimbSound.volume = (Random.Range(minVol, maxVol));
        }
    }

    
}

