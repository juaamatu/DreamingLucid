using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Petro Pitkänen
//Controls stabbing on story room
public class PlayerStabAnimationController : MonoBehaviour {

    //<summary>
    //Get references for sprite animators and sprite renderers
    //</summary>
    public SpriteAnimator StabUp;
    public SpriteAnimator StabDown;
    public SpriteAnimator walkAway;
    SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer upRenderer;
    public SpriteRenderer downRenderer;
    public SpriteRenderer idleRenderer;
    public SpriteRenderer upIdleRenderer;
    public SpriteRenderer walkRenderer;
    private bool trigger = false;           //bool for triggering stabbing animations back and forth
    private bool wait = false;              //bool for making animations wait for them to be finished
    public StabAnimationController targetStabAnimator;  //reference to script on other stabbing animator
    private bool stabOnce = true;           //bool for animating the other stabber
    private bool stop = false;              //bool for making the animations stop
    public Transform endGoal;               //goal for player to move to
    public Transform startGoal;             //starting position
    private bool upOnce = true;             //bool for framebreaking
    private bool downOnce = true;           //bool for framebreaking
    public Object[] blood;                  //array of bloodstain objects
    private bool startWalking = false;      //bool for when to start walking
    public string m_LevelName;
    private bool start = false;
    public SpriteRenderer blackScreen;
    private float t;
    AudioSource stabSound;
   


    void Start () {

        //<summary>
        //Get references and set up renderers and animations
        //</summary>
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        stabSound = GetComponent<AudioSource>();
        StabUp.enabled = false;
        StabDown.enabled = false;
        upRenderer.enabled = true;
        downRenderer.enabled = true;
        idleRenderer.enabled = true;
        upIdleRenderer.enabled = false;
        walkAway.enabled = false;
        walkRenderer.enabled = true;
        StartCoroutine(WaitForStart());
        blackScreen.enabled = true;
        blackScreen.color = Color.clear;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (start)
        {
            if (Input.anyKeyDown && !wait && !stop) //make animation if a button is pressed
            {
                if (!trigger)
                {
                    StabUpActivate();
                }
                if (trigger)
                {
                    StabDownActivate();
                }
                if (stabOnce)
                {
                    targetStabAnimator.StabEnabled();
                    stabOnce = false;
                }
                StopCoroutine("WaitForBreak");      //break waiting loop if key is pressed
                StartCoroutine("WaitForBreak");     //start waiting loop

            }

        }
     if (stop)                                      //stops stabbing
        {
            StartCoroutine(FrameBreakIdle());
            targetStabAnimator.DeathEnabled();
            idleRenderer.enabled = true;
            StabUp.enabled = false;
            StabDown.enabled = false;
            if (Input.anyKeyDown)                   //starts the walking away if the player presses any key
            { 

                startWalking = true;
                stop = false;

            }

            
        }
        if (startWalking)                           //start the walking animation and move the player away
        {
            StartCoroutine(FrameBreakWalk());
            walkAway.enabled = true;
            transform.position = Vector3.MoveTowards(startGoal.position, endGoal.position, .005f);
            StartCoroutine(WaitForNextScene());
        }

    }

    void StabUpActivate()      //stabs up
    {
        if (upOnce)
        {
            StartCoroutine(FrameBreakUp());
        }
        StabUp.enabled = true;
        StabDown.enabled = false;
        upRenderer.enabled = true;
        StartCoroutine(WaitUp());
        wait = true;
        upIdleRenderer.enabled = false;
        m_SpriteRenderer.enabled = false;
        if (!upOnce)
        {
            idleRenderer.enabled = false;
        }
        Vector3 position = new Vector3(Random.Range(2.287f, 2.5f), Random.Range(0.862f, 1f), 0f);   //adds bloodstains with random position and rotation
        Instantiate(blood[Random.Range(0, blood.Length)], position, Quaternion.identity);
        
            stabSound.Play();
        

    }

    void StabDownActivate()     //stabs down
    {
        if (downOnce)
        {
            StartCoroutine(FrameBreakDown());
        }
        
        StabUp.enabled = false;
        StabDown.enabled = true;
        downRenderer.enabled = true;
        StartCoroutine(WaitDown());
        wait = true;
        idleRenderer.enabled = false;
        m_SpriteRenderer.enabled = false;
        upRenderer.enabled = false;
        if (!downOnce)
        {
            upIdleRenderer.enabled = false;

        }
        Vector3 position = new Vector3(Random.Range(2.287f, 2.5f), Random.Range(0.862f, 1f), 0f);   //adds bloodstains with random position and rotation
        Instantiate(blood[Random.Range(0, blood.Length)], position, Quaternion.identity);
        
            stabSound.Play();
        


    }

    IEnumerator WaitUp()    //wait for the stabbing up animation to finish and then stop it
    {
        yield return new WaitForSeconds(.2f);
        StabUp.enabled = false;
        StabDown.enabled = false;
        downRenderer.enabled = false;
        upRenderer.enabled = false;
        upIdleRenderer.enabled = true;
        wait = false;
        trigger = true;
    }

    IEnumerator WaitDown()  //wait for the stabbing down animation to finish and then stop it
    {
        yield return new WaitForSeconds(.2f);
        StabUp.enabled = false;
        StabDown.enabled = false;
        downRenderer.enabled = false;
        upRenderer.enabled = false;
        idleRenderer.enabled = true;
        wait = false;
        trigger = false;
    }

    IEnumerator WaitForBreak()  //waits until theres no keys pressed for 2 seconds and then stops the stabbing
    {
        yield return new WaitForSeconds(2f);
        stop = true;
        start = false;
    }

    IEnumerator FrameBreakUp()  //wait for the first frame of stabbing up
    {
        yield return new WaitForSeconds(.1f);
        idleRenderer.enabled = false;
        upOnce = false;


    }
    IEnumerator FrameBreakDown()    //wait for the first frame of stabbing down
    {
        yield return new WaitForSeconds(.1f);
        upIdleRenderer.enabled = false;
        downOnce = false;

    }
    IEnumerator FrameBreakIdle()    //wait for the first frame after stopping the stabbing
    {
        yield return new WaitForSeconds(.1f);
        downRenderer.enabled = false;
        upIdleRenderer.enabled = false;
        upRenderer.enabled = false;
        m_SpriteRenderer.enabled = false;
    }
    IEnumerator FrameBreakWalk()    //wait for the first time for walking
    {
        yield return new WaitForSeconds(.1f);
        idleRenderer.enabled = false;
    }
    IEnumerator WaitForNextScene()  //wait for 5 seconds, then load next scene
    {
        yield return new WaitForSeconds(5f);
        blackScreen.color = Color.Lerp(Color.clear, Color.white, t);
        if (t < 1)
        {
            t += Time.deltaTime / 1f;
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(m_LevelName);
    }
    IEnumerator WaitForStart()      //wait for 1 second before starting the stabbing
    {
        yield return new WaitForSeconds(1.5f);
        start = true;
    }
}
