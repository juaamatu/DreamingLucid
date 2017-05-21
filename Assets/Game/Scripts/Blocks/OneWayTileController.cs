using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayTileController : MonoBehaviour {

    [SerializeField] protected bool m_GhostOnly = false;
    protected bool m_OnTile = false;     // is player on tile
    protected Collider2D m_Collider;
    protected bool m_CharacterCollision;
    protected bool m_DownPressed;
    SpriteRenderer sprite;

    protected virtual void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        
    }
    
    void Start()
    {
        PlayerEventHandler.OnDeath += Ghost;
        PlayerEventHandler.OnRespawn += NotGhost;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Ghost()
    {
        if (m_GhostOnly)
        {
            //sprite.color = new Color(1f, 1f, 1f, .5f);
        }
    }
    void NotGhost()
    {
        if (m_GhostOnly)
        {
            //sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    protected virtual void Update () {

        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical != 0)
        {
            vertical = (vertical < -0.8) ? -1 : 1;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!m_CharacterCollision && m_Collider.bounds.center.y - player.position.y > 0.01f)
        {
            m_Collider.enabled = false;
        }
        else if (!m_CharacterCollision)
        {
            m_Collider.enabled = true;
        }
        if (vertical == -1 && m_CharacterCollision)
        {
            m_Collider.enabled = false;
            m_DownPressed = true;
            
        }
        if (m_CharacterCollision && m_DownPressed)
        {            
            if (transform.position.y - player.position.y > -0.1)
            {
                m_CharacterCollision = false;
                m_Collider.enabled = true;
                m_DownPressed = false;
            }
        }
    }

    public virtual void Disable(bool isGhost)
    {
        m_CharacterCollision = ((m_GhostOnly && isGhost) || !m_GhostOnly) ? true : false;
    }

    public virtual void Enable(bool isGhost)
    {
        m_CharacterCollision = false;
    }
}
