using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOffController : MonoBehaviour, IButtonInteractable {

    SpriteRenderer sprite;
    PolygonCollider2D m_Collider;
    AudioSource myActivatingSound;
    public bool trigger;
    

	// Use this for initialization
	void Start () {

        sprite = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<PolygonCollider2D>();
        myActivatingSound = GetComponent<AudioSource>();
        trigger = false;
        
	}
	
	// Update is called once per frame
	public void ButtonPressed () {


        if (trigger)
        {
            sprite.enabled = false;
            m_Collider.enabled = false;
            myActivatingSound.Play();
        }
        if (!trigger)
        {
            sprite.enabled = true;
            m_Collider.enabled = true;
            myActivatingSound.Play();
        }


	}
    void Update()
    {
        if (sprite.enabled && m_Collider.enabled)
        {
            trigger = true;
        }
        if (!sprite.enabled && !m_Collider.enabled)
        {
            trigger = false;
        }
    }
}
