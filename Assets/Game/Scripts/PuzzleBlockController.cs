using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen, Juho Turpeinen
// Controls puzzle blocks
public class PuzzleBlockController : MonoBehaviour, IButtonInteractable {

    BoxCollider2D m_Collider;       //collider of the block
    SpriteRenderer m_Sprite;        //sprite of the block
    public bool m_BlockOn = true;   //bool for checking if the block is on
    public bool m_CanPassAsGhost = false;   //bool for checking if the player can pass through the block as a ghost

	// Use this for initialization
	void Start () {

        //<summary>
        //get components and enabe/disable them
        //</summary>
        m_Collider = GetComponent<BoxCollider2D>();
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Sprite.enabled = m_BlockOn;
        m_Collider.enabled = m_BlockOn;
	}

	void Update () {

        m_Sprite.enabled = m_BlockOn; //enable sprite if the block on bool is true
        //enable collider if block on is true and disable if if canpass as ghost is true and the player is a ghost
        m_Collider.enabled = m_BlockOn && !(PlayerEventHandler.IsGhost && m_CanPassAsGhost);
	}

    public void ButtonPressed() //toggle block on after button press
    {
        m_BlockOn = !m_BlockOn;
    }
}
