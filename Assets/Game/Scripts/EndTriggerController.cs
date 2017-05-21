using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Petro Pitkänen
// Controls the end credits
public class EndTriggerController : MonoBehaviour {

    BoxCollider2D m_collider;           //trigger collider
    Animator m_Animator;                //player animator
    public SpriteRenderer m_Sprite;     //sprite for fade out
    public AudioSource m_Audio;         //audiosource for stabbing sound
    public AudioSource m_KnifeSound;    //audiosource for picking up knife
    public AudioSource m_Music;         //audiosource for end music
    private bool loop = true;           //check if we are still looping the crying animation
    private bool animStarted = false;   //check if the animation has started
    private bool ending = false;        //check if we are starting the ending
    private bool disable = false;       //disable the start of ending after button press
    public GameObject m_PlayerAudio;    //player audio object
    public SpriteRenderer m_CreditBar;  //sprite for fading part of credits bar
    public SpriteRenderer m_CreditBar2; //sprite for rest of credits bar
    public SpriteRenderer m_Credits;    //sprite for credits
    private bool startCredits = false;  //check if we are ready to start the credits
    private float t;                    //float for the fading in
    private float t2;                   //float for the fading in
    public GameObject m_CreditObject;   //object, which has the credits sprite
    private bool fadeOut = false;       //check if we are ready to fade out

    /// <summary>
    /// Get components and set everything to start values
    /// </summary>
	void Start () {
   
        m_collider = GetComponent<BoxCollider2D>();
        m_Animator = GetComponent<Animator>();
        m_Sprite.enabled = false;
        m_CreditBar.enabled = false;
        m_CreditBar2.enabled = false;
        m_CreditBar.color = Color.clear;
        m_CreditBar2.color = Color.clear;
        m_CreditObject.SetActive(false);
	}
	
	void FixedUpdate () {

        if (Input.anyKeyDown && ending && !disable) //start the ending stab animation 
        {
            m_Animator.SetTrigger("StabAgain");
            StartCoroutine(Ending());
        }
        if (startCredits) //start the fading in of credits bar
        {
            float duration = 2f;
            m_CreditBar.color = Color.Lerp(Color.clear, Color.white, t);
            m_CreditBar2.color = Color.Lerp(Color.clear, Color.white, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
        }
        if (fadeOut) //start the fading out of credits bar
        {
            float duration = 2f;
            m_CreditBar.color = Color.Lerp(Color.white, Color.clear, t2);
            m_CreditBar2.color = Color.Lerp(Color.white, Color.clear, t2);
            if (t2 < 1)
            {
                t2 += Time.deltaTime / duration;
            }
        }

	}

    void Stab() //run the animation for getting ready to sudoku
    {
        loop = false;
        m_Animator.SetTrigger("Stab");
        StartCoroutine(WaitEnd());
    }

    void OnTriggerEnter2D(Collider2D col) //check when player collides with trigger and disable player input
    {
        if (col.gameObject.tag.Equals("EndTrigger"))
        {
            PlayerEventHandler.DisablePlayerInput();
            StartCoroutine(WaitStart());
            m_PlayerAudio.SetActive(false);      
        }
    }

    IEnumerator WaitStart() //wait a while before starting the credits and animations
    {
        yield return new WaitForSeconds(3f);
        m_Animator.SetTrigger("End");
        StartCoroutine(WaitPause());
        animStarted = true;
        m_Music.Play();
        startCredits = true;
        m_CreditBar.enabled = true;
        m_CreditBar2.enabled = true;
        m_CreditObject.SetActive(true);
        StartCoroutine(WaitCredits());
    }

    IEnumerator WaitCredits() //wait for the credits to roll fully before accepting input from the player
    {
        yield return new WaitForSeconds(46.5f);
        Stab();
        fadeOut = true;
    }
  
    IEnumerator WaitPause() //wait for the crying animation to pause
    {
        yield return new WaitForSeconds(3f);
        m_Animator.SetBool("Pause", true);
        if (loop)
        {
            StartCoroutine(WaitBack());
        }
    }

    IEnumerator WaitBack() //wait for the crying animation to resume
    {
        yield return new WaitForSeconds(2f);
        m_Animator.SetBool("Pause", false);
        if (loop)
        {
            StartCoroutine(WaitPause());
        }
    }

    IEnumerator WaitEnd() //wait for the ending 
    {
        yield return new WaitForSeconds(2f);
        if (!ending)
        {
            ending = true;
            m_KnifeSound.Play();
        }
    }

    IEnumerator Ending() //wait a bit and then run the final animation and sound effect
    {
        yield return new WaitForSeconds(.5f);
        m_Sprite.enabled = true;
        m_Audio.Play();
        disable = true;
        StartCoroutine(EndSomeMore());
        m_Music.Stop();
    }

    IEnumerator EndSomeMore() //wait a while before loading the start menu
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Startmenu");
    }
}
