using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls disappearing block
public class DisappearingBlockController : MonoBehaviour {

    public SpriteRenderer m_Sprite; //blocks spriterenderer
    public Collider2D m_Collider;   //collider
    public float duration = 5.0f;   //fade out/in duration
    private bool m_FadeOut = false; //bool for triggering fade out
    private bool m_FadeIn = false;  //bool for triggering fade in
    private float t;                //float for mathf
    private Color currentColor;     //current color of the sprite
    public bool inTrigger = false;  //debug for checking if player is in trigger
    public float fadeInWait;
    public float fadeOutWait;

	void Start()
    {
        t = 0f;     //setting float to 0
    }
	
	// Update is called once per frame
	void Update () {

        currentColor = m_Sprite.color;  //tracks current color

        if (m_FadeIn)   //trigger fading in
        {
            m_Sprite.color = Color.Lerp(currentColor, Color.clear, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
        }

        if (m_FadeOut) //trigger fading out
        {
            m_Sprite.color = Color.Lerp(currentColor, Color.white, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
        }

        if (m_Sprite.color.a < 0.4f) //trigger disabling collider
        {
            m_Collider.enabled = false;
            
        }
        if (m_Sprite.color.a > 0.4f) //trigger enabling collider
        {
            m_Collider.enabled = true;
        }
      
		
	}

    void OnTriggerEnter2D (Collider2D collider) //checking when player enters trigger
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOut());
            inTrigger = true;
        }
    }
    void OnTriggerExit2D (Collider2D collider) //checking when player exits trigger
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeIn());
            inTrigger = false;
        }
    }

    IEnumerator FadeIn()    //wait of fading in
    {
        yield return new WaitForSeconds(fadeInWait);
        m_FadeIn = false;
        m_FadeOut = true;
        t = 0f;
    }

    IEnumerator FadeOut()   //wait for fading out
    {
        yield return new WaitForSeconds(fadeOutWait);
        m_FadeIn = true;
        m_FadeOut = false;
        t = 0f;
    }
}
