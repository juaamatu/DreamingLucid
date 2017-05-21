using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KnifeEnemyController : MonoBehaviour {

    [SerializeField] private float m_Step = 0.1f;           // Base for step length
    [SerializeField] private float m_AggroDistance = 1f;    // Distance from witch to player should we charge towards it
    [SerializeField] private float m_NearPlayerStepMultiplier = 2;  // Multiplier for step when near player
    [SerializeField] private LayerMask m_WhatIsGround;      // Layermask for environment
    private Collider2D m_Collider;                          // Collider reference
    private Vector3 m_MoveDirection = Vector3.right;        // Current move direction
    private bool m_PlayerIsNear = false;

    /// <summary>
    /// Called before first frame
    /// </summary>
    private void Start () {
        // Get references
        m_Collider = GetComponent<Collider2D>();
	}

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update () {
        float step = m_Step * Time.deltaTime;   // Step distance for this frame
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;    // Player transform
        Vector3 dirToPlayer = player.position - transform.position; // Direction to player
        if (m_PlayerIsNear != dirToPlayer.magnitude <= m_AggroDistance) // If player enters / exits aggro distance
        {
            m_PlayerIsNear = !m_PlayerIsNear;   // Flip flag
            if (m_PlayerIsNear && dirToPlayer.x * m_MoveDirection.x < 0)    // If facing away from player -> face towards player
            {
                m_MoveDirection = -m_MoveDirection;
            }
        }
        step *= (m_PlayerIsNear && dirToPlayer.x * m_MoveDirection.x > 0) ? m_NearPlayerStepMultiplier : 1; // Adds step multiplier near player if facing player direction
        // Calculate raycast origin and raycast forward and down.
        Vector3 origin = transform.position + new Vector3(m_Collider.bounds.extents.x * ((m_MoveDirection.x >= 0) ? 1 : -1), -m_Collider.bounds.extents.y / 2, 0);
        RaycastHit2D hitForward = Physics2D.Raycast(origin, m_MoveDirection, step, m_WhatIsGround);
        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector3.down, m_Collider.bounds.extents.y, m_WhatIsGround);    // TODO Origin needs to be tied to step distance?
        if (hitForward.collider != null || hitDown.collider == null)    // If something if front or nothing down -> change direction
        {
            m_MoveDirection = -m_MoveDirection;
        }
        Vector3 scale = transform.localScale;   // Flip scale if needed
        scale.x = (m_MoveDirection.x * transform.localScale.x >= 0) ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = scale;
        transform.Translate(m_MoveDirection * step);    // Move to direction
	}
}