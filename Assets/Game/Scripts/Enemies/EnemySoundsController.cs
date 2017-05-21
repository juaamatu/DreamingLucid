using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls sounds on Enemy1
public class EnemySoundsController : MonoBehaviour {

    [SerializeField] private float m_AggroDistance = 1; //aggrorange from the player
    public AudioClip[] detectSounds;        //array for detect sounds
    public AudioClip[] hitSounds;           //array for hitting sounds
    public AudioClip[] movementSounds;      //array for movement sounds
    public AudioSource myDetectSounds;      //audiosource for detect sounds
    public AudioSource myHitSounds;         //audiosource for hitting sounds
    public AudioSource myMovementSounds;    //audiosource for movement sounds
    private float prevPos;                  //previous position float
    private float curPos;                   //current position float
    private bool isMoving = false;          //bool to check if the enemy is moving
    private bool m_PlayerIsNear;            //bool to check if the player is in range
    private bool detectSoundPlayed = false; //bool to check if the detection sound has been played
    Transform m_PlayerTransform;            //players transform
   

    void Start ()
    {

        prevPos = transform.position.x; //set previous position
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;   //find the players transform
    }

	void Update () {

        curPos = transform.position.x;
        //<summary>
        //checks if the enemy is moving horizontally
        //</summary>
        if (curPos == prevPos) 
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        prevPos = curPos;
        if (isMoving && !myMovementSounds.isPlaying && movementSounds.Length > 0) //play movement sounds if the enemy is moving
        {
            myMovementSounds.PlayOneShot(movementSounds[Random.Range(0, movementSounds.Length)]);
        }
        if (m_PlayerIsNear && !myDetectSounds.isPlaying && !detectSoundPlayed && detectSounds.Length > 0) //play detection sounds when player is in aggrorange
        {
            myDetectSounds.PlayOneShot(detectSounds[Random.Range(0, detectSounds.Length)]);
            detectSoundPlayed = true;
        }

        if (!m_PlayerIsNear && detectSoundPlayed && !myDetectSounds.isPlaying) //makes sound play again only after it has finished playing
        {
            detectSoundPlayed = false;
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !myHitSounds.isPlaying && hitSounds.Length > 0) //play hitting sound, when colliding with the player
        {
            myHitSounds.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
        }
       
    }

}
