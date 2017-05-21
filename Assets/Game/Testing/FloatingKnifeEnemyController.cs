using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Controls knife enemy that follows waypoints when player is not around
/// When player enters aggro zone knife enemy starts to charge towards player
/// </summary>
[RequireComponent(typeof(Animator))]
public class FloatingKnifeEnemyController : MonoBehaviour, IButtonInteractable {

    [SerializeField] private float m_Speed = 0.1f;          // Base for step length
    [SerializeField] private float m_AggroDistance = 1f;    // Distance from witch to player should we charge towards it
    [SerializeField] private float m_NearPlayerSpeedMultiplier = 2;  // Multiplier for step when near player
    [SerializeField] private LayerMask m_WhatIsGround;      // Layermask for environment
    [SerializeField] private Transform[] m_Waypoints;       // Waypoints to circle around
    [SerializeField] private float m_OverShootMultiplier = 0.3f;     // How much do we want to overshoot the target
    [SerializeField] private Transform m_DeathTransform;         // transform for the location where we want to die
    private KnifeEnemyDeathController m_KnifeEnemyDeathController;      // Script that controlls the death of this enemy
    private Vector3[] m_WorldWayPoints;                     // Waypoints corrected for world space
    private Vector3 m_StartPosition;                        // Lerp start position
    private Vector3 m_TargetPosition;                       // Lerp target position
    private Animator m_Animator;                            // Animator reference
    private Vector3 m_MoveDirection = Vector3.right;        // Current move direction
    private bool m_PlayerIsNear = false;                    // Flag if player is near
    private int m_CurrentIndex = 0;                         // Current waypoint index
    private int m_TargetIndex = 1;                          // Target waypoint index
    private float m_Frac = 0;                               // Frac of our lerp
    private float m_JourneyLength;                          // Lerp length
    private bool m_ChargeFinished = true;                   // Has enemy finished his charge towards player
    public AudioClip[] knifeSounds;
    public AudioSource myKnifeSounds;
    private Collider2D m_Collider;                          // Collider reference
    private bool m_LastCharge = false;                      // Are we currently on last charge before destroying self
    private bool m_ReadyToDestroySelf = false;              // Should we destroy after killing player
    private Vector2 m_DeathPos;

    /// <summary>
    /// Called before first frame
    /// </summary>
    private void Start ()
    {
        // Get references
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponentInChildren<Collider2D>();
        m_KnifeEnemyDeathController = GetComponent<KnifeEnemyDeathController>();
        // Log error if waypoints length < 2
        if (m_Waypoints.Length < 2)
        {
            Debug.LogError(gameObject.name + " has less than two waypoints");
            return;
        }
        // Set waypoints so they stay in worldspace
        m_WorldWayPoints = new Vector3[m_Waypoints.Length];
        for (int i = 0; i < m_Waypoints.Length; i++)
        {   
            m_WorldWayPoints[i] = m_Waypoints[i].position;
        }
        // Set lerp variables
        m_StartPosition = m_WorldWayPoints[m_CurrentIndex];
        m_TargetPosition = m_WorldWayPoints[m_TargetIndex];
        m_JourneyLength = Vector3.Distance(m_StartPosition, m_TargetPosition);

        m_DeathPos = m_DeathTransform.position;
	}

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update ()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;    // Get player transform
                                                                                    // Draw debug line towards player that has aggrodistance as length
        Debug.DrawLine(transform.position, transform.position + (player.position - transform.position).normalized * m_AggroDistance);
        // If player enters aggro distance
        if (!m_PlayerIsNear && (player.position - transform.position).magnitude < m_AggroDistance && !PlayerEventHandler.IsGhost && !PlayerEventHandler.IsSpawnProtected && CanSeePlayer(player.position))
        {
            // Set flag that player is near + set lerp variables towards player
            m_PlayerIsNear = true;
            m_StartPosition = transform.position;
            m_TargetPosition = player.position + (player.position - m_StartPosition).normalized * m_OverShootMultiplier;
            m_Frac = 0;
            m_JourneyLength = Vector3.Distance(m_StartPosition, m_TargetPosition);
            StartCoroutine(DisableAndEnable(1));    // Wait a while before chargning
            if (knifeSounds.Length > 0 && !myKnifeSounds.isPlaying)
            {
                myKnifeSounds.PlayOneShot(knifeSounds[Random.Range(0, knifeSounds.Length)]);
            }
            m_Animator.SetTrigger("Charge");        // Play charge animation
            m_ChargeFinished = false;               // Tell the game that we are going to charge
        }
        if (PlayerEventHandler.IsGhost || PlayerEventHandler.IsSpawnProtected)  // If player is immune dont aggro
        {
            m_PlayerIsNear = false;
        }
        // Lerp towards target
        m_Frac = (m_PlayerIsNear) ? m_Frac + Time.deltaTime * m_Speed * m_NearPlayerSpeedMultiplier : m_Frac + Time.deltaTime * m_Speed;
        float step = m_Frac / m_JourneyLength;
        Vector3 targetPosition = Vector3.Lerp(m_StartPosition, m_TargetPosition, step);
        if (step >= 1)  // lerp finished
        {
            if (m_ReadyToDestroySelf)   // If knife enemy is ready to be destroyd
            {
                Destroy(gameObject);
            }
            // if player is not near anymore -> set flag and move towards waypoints
            if (m_PlayerIsNear != (player.position - transform.position).magnitude < m_AggroDistance && !PlayerEventHandler.IsGhost && !PlayerEventHandler.IsSpawnProtected)
            {
                m_PlayerIsNear = !m_PlayerIsNear;
            }
            if (m_PlayerIsNear && CanSeePlayer(player.position)) // Charge again
            {
                // Set lerp variables
                m_StartPosition = transform.position;
                m_TargetPosition = player.position + (player.position - m_StartPosition).normalized * m_OverShootMultiplier;
                StartCoroutine(DisableAndEnable(1));
                if (knifeSounds.Length > 0 && !myKnifeSounds.isPlaying)
                {
                    myKnifeSounds.PlayOneShot(knifeSounds[Random.Range(0, knifeSounds.Length)]);
                }
                m_Animator.SetTrigger("Charge");    // Play animation
                m_ChargeFinished = false;
            }
            else    // Move towards waypoints
            {
                m_CurrentIndex = m_TargetIndex;
                m_TargetIndex = (m_TargetIndex == m_WorldWayPoints.Length - 1) ? 0 : m_TargetIndex + 1;
                m_StartPosition = transform.position;
                m_TargetPosition = m_WorldWayPoints[m_TargetIndex];
                m_ChargeFinished = true;
            }
            m_JourneyLength = Vector3.Distance(m_StartPosition, m_TargetPosition);
            m_Frac = 0;
        }
        // Old rotation code still used to flip player
        Vector3 moveDirection = m_TargetPosition - m_StartPosition;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
        transform.position = targetPosition;
        //transform.rotation = targetRotation;  // Use if u want to rotate towards target
        Vector3 scale = transform.localScale;   // Flip scale if needed
        scale.x = (moveDirection.x * transform.localScale.x >= 0) ? transform.localScale.x : -transform.localScale.x;
        transform.localScale = scale;

        if (m_LastCharge && !m_ReadyToDestroySelf)   // If this is the last charge and we want to destroy this after we are finished
        {
            Collider2D[] collisions = Physics2D.OverlapBoxAll(m_Collider.bounds.center, m_Collider.bounds.size, 0);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i].tag.Equals("Player"))
                {
                    m_ReadyToDestroySelf = true;
                }
            }
        }
    }

    /// <summary>
    /// Checks if player is in sight, e.g is there anything in between this and player transforms
    /// </summary>
    /// <param name="playerPos">player position</param>
    /// <returns></returns>
    private bool CanSeePlayer(Vector3 playerPos)
    {
        Vector3 dir = playerPos - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, m_WhatIsGround);
        if (hit.collider != null && hit.collider.tag.Equals("Player"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Disable and enable after a while
    /// </summary>
    /// <param name="duration">duration to stay disabled</param>
    /// <returns></returns>
    IEnumerator DisableAndEnable(float duration)
    {
        enabled = false;
        yield return new WaitForSeconds(duration);
        enabled = true;
    }

    /// <summary>
    /// Called when button connected with this script is pressed
    /// </summary>
    public void ButtonPressed()
    {
        enabled = false;
        Destroy(this);
        m_KnifeEnemyDeathController.StartKnifeEnemyDeath(m_DeathPos);
    }
}