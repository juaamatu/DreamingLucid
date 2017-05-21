using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTextController : MonoBehaviour {

    private SpriteRenderer m_Sprite;

	// Use this for initialization
	void Start () {

        m_Sprite = GetComponent<SpriteRenderer>();
        m_Sprite.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

        if (PlayerEventHandler.IsGhost)
        {
            m_Sprite.enabled = true;
        }
        if (!PlayerEventHandler.IsGhost)
        {
            m_Sprite.enabled = false;
        }
		
	}
}
