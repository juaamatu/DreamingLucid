using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen, Timo Koski
// Controls the turning lights on and off
public class LightTurnOnController : MonoBehaviour {

    public SpriteRenderer m_Sprite; //sprite of the light
    SpriteAnimator m_Animator;      //light animator

	void Start () {

        m_Animator = GetComponent<SpriteAnimator>(); //get spriteanimator component
        m_Sprite.enabled = false;   //disable the sprite at the start
        m_Animator.enabled = false; //disable the animator at the start

	}

	void OnTriggerEnter2D(Collider2D collider) //turns the light on when player collides with it
	{
		if (collider.gameObject.tag.Equals ("Player")) {
			m_Animator.enabled = true;
			m_Sprite.enabled = true;
		} 
	}
	void OnTriggerExit2D(Collider2D collider) //turns the light off when player is no longer colliding with it
	{
		if (collider.gameObject.tag.Equals ("Player")) {
			m_Animator.enabled = false;
			m_Sprite.enabled = false;
		} 
	}
}
