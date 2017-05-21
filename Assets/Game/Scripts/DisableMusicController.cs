using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls disabling music in a certain area
public class DisableMusicController : MonoBehaviour {

    BoxCollider2D m_Collider;
    GameObject m_Music;
    public bool m_InTrigger = false;

	void Start () {

        //get references
        m_Collider = GetComponent<BoxCollider2D>();
        m_Music = GameObject.FindGameObjectWithTag("Music");
        		
	}
	
    void OnTriggerEnter2D(Collider2D collider) //disable music when entering trigger
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            m_Music.SetActive(false);
            m_InTrigger = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider) //enable music when exiting trigger
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            m_Music.SetActive(true);
            m_InTrigger = false;
        }
    }
}
