using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calls events about player
public class PlayerEventHandler : MonoBehaviour
{

    private static PlayerEventHandler handler;
    private CharacterController2D m_PlayerCharacted2D;          // Reference to another player script
    private Collider2D m_Collider;
    private PlayerController m_PlayerController;
    private GhostController m_GhostController;

    private bool m_IsWalking = false;                           // Is player walking
    private bool m_IsGrounded = true;                           // Is player grounded
    private string m_TagOfTileBelow;
    private GameObject m_GameObjectUnderPlayer;

    public static bool IsGrounded { get { return handler.m_IsGrounded; } }
    public static bool IsWalking { get { return handler.m_IsWalking; } }
    public static bool IsGhost { get { return handler.m_GhostController.enabled; } }
    public static bool IsSpawnProtected { get { return handler.m_PlayerController.m_IsSpawnProtected; } }
    public static GameObject GameObjectUnderPlayer { get { return handler.m_GameObjectUnderPlayer; } }
    public static List<GameObject> Corpses { get { return handler.m_PlayerController.Corpses; } }

    public delegate void FallStartAction();
    public static event FallStartAction OnFallStart;            // Called when falling starts
    public delegate void FallEndAction();
    public static event FallEndAction OnFallEnd;                // Called when falling ends
    public delegate void JumpStartAction();
    public static event JumpStartAction OnJumpStart;            // Called when jump stars
    public delegate void WalkStartAction();
    public static event WalkStartAction OnWalkStart;            // Called when walk stars
    public delegate void WalkEndAction();
    public static event WalkStartAction OnWalkEnd;              // Called when walk ends
    public delegate void DeathAction();
    public static event DeathAction OnDeath;
    public delegate void RespawnAction();
    public static event DeathAction OnRespawn;
    public delegate void LifeRedeemAction();
    public static event DeathAction OnLifeRedeemed;
    public delegate void OnGhostSpawnAction();
    public static event OnGhostSpawnAction OnGhostSpawn;

    #region Monobehaviour callbacks
    private void Awake()
    {
        if (handler == null)
            handler = this;
        else
            Destroy(this);
        m_PlayerCharacted2D = GetComponent<CharacterController2D>();
        m_Collider = GetComponent<Collider2D>();
        m_GhostController = GetComponent<GhostController>();
        m_PlayerController = GetComponent<PlayerController>();
        OnFallStart = null;
        OnFallEnd = null;
        OnJumpStart = null;
        OnWalkStart = null;
        OnWalkEnd = null;
        OnDeath = null;
        OnRespawn = null;
        OnLifeRedeemed = null;
        OnGhostSpawn = null;
    }

    void Start()
    {
        SetGameObjectUnderPlayer();
    }

    private void LateUpdate()
    {
        SetGameObjectUnderPlayer();
        CheckGroundedStatus();
        CheckWalkingStatus();
    }
    #endregion

    #region Status checks
    /// <summary>
    /// Checks if grounded status has changed
    /// </summary>
    private void CheckGroundedStatus()
    {
        if (!m_IsGrounded.Equals(m_PlayerCharacted2D.isGrounded))
        {
            m_IsGrounded = !m_IsGrounded;
            if (m_IsGrounded)
            {
                if (OnFallEnd != null)
                {
                    SetGameObjectUnderPlayer();
                    OnFallEnd();
                }
            }
            else if (!m_PlayerCharacted2D.jumpedThisFrame)
            {
                if (OnFallStart != null)
                    OnFallStart();
            }
            else
            {
                if (OnJumpStart != null)
                    OnJumpStart();
            }
        }
    }
    /// <summary>
    /// Checks if walking status has changed
    /// </summary>
    private void CheckWalkingStatus()
    {
        if (!(m_IsGrounded && Mathf.Abs(m_PlayerCharacted2D.velocity.x) > 0.02).Equals(m_IsWalking))
        {
            m_IsWalking = !m_IsWalking;
            if (m_IsWalking)
            {
                if (OnWalkStart != null)
                    OnWalkStart();
            }
            else
            {
                if (OnWalkEnd != null)
                    OnWalkEnd();
            }
        }
    }

    private void SetGameObjectUnderPlayer()
    {
        RaycastHit2D hit;
        Debug.DrawLine(transform.position, transform.position + Vector3.down * (m_Collider.bounds.extents.y + 0.1f), Color.green);
        if (hit = Physics2D.Raycast(transform.position, Vector3.down, m_Collider.bounds.extents.y + 0.1f, m_PlayerCharacted2D.platformMask))
        {
            m_GameObjectUnderPlayer = hit.collider.gameObject;
        }
        else
        {            
            Collider2D circleHit = Physics2D.OverlapCircle(m_Collider.bounds.center + Vector3.down * m_Collider.bounds.extents.y, 0.1f);
            if (circleHit != null)
            {
                m_GameObjectUnderPlayer = circleHit.gameObject;
            }
            else
            {
                m_GameObjectUnderPlayer = null;
            }
        }
    }
    #endregion

    /// <summary>
    /// Called from PlayerCharacter2DUserController when ghost hits targetcorpse
    /// </summary>
    public static void LifeRedeemed()
    {
        if (OnLifeRedeemed != null)
            OnLifeRedeemed();
    }

    public static void GhostSpawn()
    {
        if (OnGhostSpawn != null)
            OnGhostSpawn();
    }

    public static void PlayerSpawn()
    {
        if (OnRespawn != null)
            OnRespawn();
    }

    public static void PlayerDeath()
    {
        if (OnDeath != null)
            OnDeath();
    }

    /// <summary>
    /// Disables and enables player after a while
    /// </summary>
    /// <param name="duration">time to be disabled</param>
    public static void DisablePlayerInput(float duration)
    {
        handler.StartCoroutine(handler.DisableAndEnablePlayerInput(duration));       
    }

    /// <summary>
    /// Disables player ability to get input
    /// </summary>
    public static void DisablePlayerInput()
    {
        if (handler != null && handler.m_PlayerController != null && handler.m_GhostController != null)
        {
            handler.m_PlayerController.CanMove = false;
            handler.m_GhostController.CanMove = false;
        }
    }

    /// <summary>
    /// Enables player ability to get input
    /// </summary>
    public static void EnablePlayerInput()
    {
        if (handler != null && handler.m_PlayerController != null && handler.m_GhostController != null)
        {
            handler.m_PlayerController.CanMove = true;
            handler.m_GhostController.CanMove = true;
        }
    }

    /// <summary>
    /// Disables and enables player after a while
    /// </summary>
    /// <param name="duration">time to be disabled</param>
    /// <returns></returns>
    IEnumerator DisableAndEnablePlayerInput(float duration)
    {
        m_PlayerController.CanMove = false;
        m_GhostController.CanMove = false;
        yield return new WaitForSeconds(duration);
        m_PlayerController.CanMove = true;
        m_GhostController.CanMove = true;
    }
}
