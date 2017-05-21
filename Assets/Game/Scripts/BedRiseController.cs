using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls the animation for rising from the bed
public class BedRiseController : MonoBehaviour {

    SpriteAnimator m_SpriteAnimator;        //sprite animator script
    public Object bed;                      //bed gameobject
    private bool hold = false;              //bool to check if the start hold is finished
    public float waitTime = 2f;             //float to how long we wait after loading the scene to accept input
    private AudioListener startListener;    //audiolistener for the start
    public AudioListener playerListener;    //audiolistener attached to the player
    public GameObject playerAudio;          //player audio object

	void Start () {

        //<summary>
        //get components and set all components and object to starting positions
        //</summary>
        m_SpriteAnimator = GetComponent<SpriteAnimator>();
        startListener = GetComponent<AudioListener>();
        m_SpriteAnimator.enabled = false;
        StartCoroutine(HoldOn());
        startListener.enabled = true;
        playerListener.enabled = false;
        playerAudio.SetActive(false);
		
	}

	void Update () {
		
        if (Input.anyKeyDown && hold) //start the animation if any button is pressed
        {
            m_SpriteAnimator.enabled = true;
            StartCoroutine(WaitForEnd());
            PlayerEventHandler.DisablePlayerInput();
        }

	}

    IEnumerator HoldOn() //IEnumerator for waiting in the start before accepting input
    {
        yield return new WaitForSeconds(waitTime);
        hold = true;
    }

    IEnumerator WaitForEnd() //IEnumerator for waiting for the animation to finish
    {
        yield return new WaitForSeconds(2.3f);
        Vector3 position = new Vector3(transform.position.x - 0.033f, transform.position.y - 0.008f, transform.position.z); //set the target position for the player
        Instantiate(bed, transform.position, Quaternion.identity); //insatiate the bed in the correct position
        GameObject.FindGameObjectWithTag("Player").transform.position = position; //move the player in the correct position
        startListener.enabled = false; //disable the start audiolistener
        playerListener.enabled = true; //enable the player audiolistener
        StartCoroutine(WaitSomeMore());
    }

    IEnumerator WaitSomeMore() //wait a bit before enabling the player audio, then destroys the unneeded sprites
    {
        yield return new WaitForSeconds(.2f);
        playerAudio.SetActive(true);
        Destroy(gameObject);
        PlayerEventHandler.EnablePlayerInput();
    }
}
