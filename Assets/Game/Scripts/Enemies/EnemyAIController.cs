using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Controls standard enemy movement.
/// Based on physics... Currently very messy.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyAIController : MonoBehaviour {

    [SerializeField] private LayerMask m_WhatIsGround;              // Mask for ground
    [SerializeField] private float m_Speed = 0.5f;                  // Speed multiplier
    [SerializeField] private float m_SpeedNearPlayer = 0.2f;        // Speed near player multiplier
    [SerializeField] private bool m_CanFallDown = true;             // Can enemy fall down. NOT SUPPRORTED ATM.
    [SerializeField] private float m_AggroDistance = 1;             // Distance from which enemy aggroes and starts to move towards player
    Transform m_PlayerTransform;                                    // Player transform
    Character2DUserController m_PlayerCharacter2D;                  // Player character script reference
    private bool m_PlayerIsNear;                                    // Is player near
    private Rigidbody2D m_RigidBody2D;                              // Rigidbody reference
    private Collider2D m_Collider;                                  // Collider reference
    private Vector2 m_Direction = Vector2.right;                    // Movement direction

    void Start () {
        // Find references
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerCharacter2D = m_PlayerTransform.gameObject.GetComponent<Character2DUserController>();
    }

    void Update()
    {
        // Find if player is near
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerIsNear = ((m_PlayerTransform.position - transform.position).magnitude <= m_AggroDistance && !m_PlayerCharacter2D.IsGhost);
    }

    // Physics should be calculated in fixed update
    void FixedUpdate()
    {   
        if (m_PlayerIsNear && CanSeePlayer())
        {
            RaycastHit2D hit;
            // calculate direction to player and normalize it
            Vector2 dirToPlayer = new Vector2((m_PlayerTransform.position - transform.position).x, m_RigidBody2D.velocity.y).normalized;
            hit = Physics2D.Raycast(transform.position, (Vector3)dirToPlayer, dirToPlayer.magnitude, m_WhatIsGround);
            if (hit.collider != null)
            {
                m_RigidBody2D.velocity = new Vector2(GetDirection().x * m_Speed, m_RigidBody2D.velocity.y);
            }
            dirToPlayer = new Vector2(dirToPlayer.x, 0);    // no need for y component       
            // raycast if we can move towawrds player in x axis
            hit = Physics2D.Raycast(transform.position + (Vector3)dirToPlayer * (m_Collider.bounds.extents.x + 0.02f), Vector3.down, m_Collider.bounds.extents.y + 0.1f, m_WhatIsGround);
            {
                if (hit.collider == null && !m_CanFallDown) // obstacle and we cant move from ledge
                {
                    m_RigidBody2D.velocity = Vector2.zero;
                }
                else // if we can fall from ledge. not supported!
                {
                    m_RigidBody2D.velocity = Mathf.Abs(m_PlayerTransform.position.x - transform.position.x) < 0.1f ? Vector2.zero : new Vector2(dirToPlayer.x * m_SpeedNearPlayer, m_RigidBody2D.velocity.y);
                }
            }

        }
        else // if player is not near
        {
            m_RigidBody2D.velocity = new Vector2(GetDirection().x * m_Speed, m_RigidBody2D.velocity.y);
        }
        // flip sprite to heading
        transform.localScale = new Vector3((m_RigidBody2D.velocity.x > 0) ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Gets direction for movement
    /// </summary>
    /// <returns>move direction</returns>
    private Vector2 GetDirection()
    {
        // raycast origin which we use as a base for raycasts
        Vector2 origin = transform.position + (Vector3)Vector2.down * (m_Collider.bounds.extents.y / 1.1f);
        // raycast all to current direction and see if we come across anything
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, m_Direction, m_Collider.bounds.extents.x + 0.1f, m_WhatIsGround);
        if (hits.Length > 1) // we always hit this gameobject so thats why > 1
        {
            m_Direction = -m_Direction; // flip direction if we came across something
        }
        else if (!m_CanFallDown) // if we cant fall down raycast that direction also
        {
            RaycastHit2D hit;
            // new origin from collider edge
            origin = (Vector2)m_Collider.bounds.center + m_Direction * (m_Collider.bounds.extents.x + 0.02f);
            Debug.DrawLine(origin, origin + Vector2.down * (m_Collider.bounds.extents.y + 0.02f));
            // raycast from origin to down and see if we have floor beneath us
            hit = Physics2D.Raycast(origin, Vector3.down, m_Collider.bounds.extents.y + 0.02f, m_WhatIsGround);
            {
                if (hit.collider == null) // no floor under
                {
                    m_Direction = -m_Direction; // flip direction
                }
            }
        }
        return m_Direction.normalized; // return direction
    }

    /// <summary>
    /// Checks if enemy can see player
    /// </summary>
    /// <returns>can enemy see player</returns>
    private bool CanSeePlayer()
    {
        RaycastHit2D hit;   // Use raycast to figure it out
        LayerMask mask = m_WhatIsGround | (1 << LayerMask.NameToLayer("Default"));  // We want to use movement mask but add player layer
        mask &= ~(1 << gameObject.layer);   // remove gameobject layer from mask so raycast doesnt return self
        Vector3 dirToPlayer = m_PlayerTransform.position - transform.position;  // direction to player
        dirToPlayer.z = transform.position.z;   // reset depth so that doesnt mess with vector magnitude
        hit = Physics2D.Raycast(transform.position, (Vector3)dirToPlayer, dirToPlayer.magnitude, mask); // perform raycast
        Debug.DrawLine(transform.position, transform.position + dirToPlayer);   // draw debug line to player
        // return if collided object was player or not and if enemy is is moving towards player in x axis
        return (hit.collider.tag.Equals("Player") && dirToPlayer.x * m_Direction.x > 0) ? true : false; 
    }
}
