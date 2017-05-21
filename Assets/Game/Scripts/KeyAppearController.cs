using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls key appearing after button press
public class KeyAppearController : MonoBehaviour , IButtonInteractable
{

    SpriteRenderer m_Sprite; //sprite of the key
    Collider2D m_Collider;   //collider of the key

	void Start () {
        
        //<summary>
        //get components and disable them
        //</summary>
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<Collider2D>();
        m_Sprite.enabled = false;
        m_Collider.enabled = false;
		
	}
	
	public void ButtonPressed() //enable sprite and collider after button press
    {
        m_Sprite.enabled = true;
        m_Collider.enabled = true;
    }
}
