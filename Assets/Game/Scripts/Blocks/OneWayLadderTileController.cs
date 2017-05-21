using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayLadderTileController : OneWayTileController
{
    Transform m_Player;
    Collider2D m_PlayerCollider;

    protected override void Awake()
    {
        base.Awake();
        m_Collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerCollider = m_Player.GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical != 0)
        {
            vertical = (vertical < -0.8) ? -1 : 1;
        }
        bool enabledCollider = m_Collider.enabled;
        if (!m_CharacterCollision && m_Collider.bounds.center.y > (m_PlayerCollider.bounds.center.y - m_PlayerCollider.bounds.extents.y))
        {
            enabledCollider = false;
        }
        else if (!m_CharacterCollision)
        {
            enabledCollider = true;
        }
        if (m_CharacterCollision && m_Collider.bounds.center.y < (m_PlayerCollider.bounds.center.y - m_PlayerCollider.bounds.extents.y))
        {
            enabledCollider = true;
        }
        if (vertical == -1 && m_CharacterCollision)
        {
            enabledCollider = false;
            m_DownPressed = true;
        }
        if (m_CharacterCollision && m_DownPressed)
        {
            if (m_Collider.bounds.center.y > (m_PlayerCollider.bounds.center.y - m_PlayerCollider.bounds.extents.y))
            {
                m_CharacterCollision = false;
                enabledCollider = true;
                m_DownPressed = false;
            }
        }        
        m_Collider.enabled = enabledCollider;
    }
}
