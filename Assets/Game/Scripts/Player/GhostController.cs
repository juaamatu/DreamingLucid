using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Handles ghost behaviour
/// </summary>
public class GhostController : Character2DUserController
{
    #region Attributes
    [SerializeField] SpriteRenderer m_OutLineRenderer;      // Spriterenderer for ghost outline. Used for fading purposes
    [SerializeField] float m_LightBounceForce = 3f;        // Force multiplier for bounceback when colliding with light
    [SerializeField] float m_LightBounceAngle = 10f;             // How much we want to angle light bounce upwards
    [SerializeField] float m_MaxGhostTime = 30f;               // How long can ghost last
    private float m_GhostTime = 0;                          // Amount of time player has been a ghost
    PlayerController m_PlayerController;                    // Controller used when not in ghost mode
    public AudioClip[] bounceSounds;
    public AudioSource myBounceSounds;
    #endregion

    #region Monobehaviour callbacks
    /// <summary>
    /// Called when this is enabled
    /// </summary>
    protected virtual void OnEnable()
    {
        m_GhostTime = 0;
        m_SpriteRenderer.color = Color.white;
        m_OutLineRenderer.enabled = true;   // Enable outline
        m_Animator.Play(Animator.StringToHash("GhostSpawn"));   // Play spawn animation
        m_CharacterController2D.platformMask |= (1 << LayerMask.NameToLayer("Light"));  // Add collision to light when in ghost mode
        m_CharacterController2D.platformMask &= ~(1 << LayerMask.NameToLayer("MovingEnemies")); // Remove moving enemies collision when in ghost mode
    }

    /// <summary>
    /// Called when this is disabled
    /// </summary>
    protected virtual void OnDisable()
    {
        m_OutLineRenderer.enabled = false;  // Disable outline
        m_SpriteRenderer.color = Color.white;
        m_CharacterController2D.platformMask &= ~(1 << LayerMask.NameToLayer("Light")); // Remove light collision when exiting light mode
        m_CharacterController2D.platformMask |= (1 << LayerMask.NameToLayer("MovingEnemies")); // Add moving enemies collision when when exiting ghost mode
    }

    /// <summary>
    /// Called when gameobject is initialized
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        m_PlayerController = GetComponent<PlayerController>();  // Add reference to player component
    }
    #endregion

    #region Character2DUserController override functions

    /// <summary>
    /// Animations are handled here every frame
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected override void AnimationUpdate(float horizontal, float vertical)
    {
        m_Animator.SetLayerWeight(1,1); // 1 layer is ghost layer -> we want to use that
        m_Animator.SetFloat("Speed", Mathf.Abs(horizontal));    // set speed
    }

    /// <summary>
    /// General update called once a frame
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected override void GeneralUpdate(float horizontal, float vertical)
    {
        // Check ghost time and lerp outline alpha
        m_GhostTime += Time.deltaTime;
        Color target = Color.white;
        target.a = 0;
        m_SpriteRenderer.color = Color.Lerp(Color.white, target, m_GhostTime / m_MaxGhostTime);
        if (m_GhostTime > m_MaxGhostTime)
        {
            RegularDeath();
        }

        if (PlayerEventHandler.GameObjectUnderPlayer != null && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("Corpse"))    // If we are overlapping with a corpse
        {
            GameObject overlappedCorpse = PlayerEventHandler.GameObjectUnderPlayer;
            if (vertical == -1) // If we press down --> posess corpse
            {
                enabled = false; // disable self
                transform.position = overlappedCorpse.transform.position; // apply corpse position
                transform.rotation = overlappedCorpse.transform.rotation; // apply corpse rotation
                RaycastHit2D hit;   // Raycast down to get more specific location for transform when posessing corpse
                hit = Physics2D.Raycast(transform.position, Vector3.down, Mathf.Infinity, m_CharacterController2D.platformMask);
                if (hit.collider != null)
                {
                    // we found something underneath -> lets calculate position just above it
                    transform.position = hit.collider.bounds.center + Vector3.up * (hit.collider.bounds.extents.y + m_Collider2D.bounds.extents.y);
                }
                m_PlayerController.m_Corpses.Remove(overlappedCorpse);    // remove found corpse from corpse list
                if (overlappedCorpse != null)
                {
                    Destroy(overlappedCorpse);    // Destroy corpse if it still exists
                }
                m_PlayerController.m_RisedThisFrame = true; // add flag to indicate we posessed corpse this frame
                m_PlayerController.enabled = true; // enable playercontroller
                PlayerEventHandler.PlayerSpawn();   // Call event that we spawned
                CameraRigController.SetVignette(false); // Remove image effect
            }
            else if (PlayerEventHandler.GameObjectUnderPlayer.Equals(m_PlayerController.Corpses[m_PlayerController.Corpses.Count - 1])) // we are overlapping target corpse
            {
                if (vertical == 1)  // press up
                {
                    transform.position = m_Spawn; // move to spawn
                    enabled = false;    // disable ghost
                    m_PlayerController.m_SpawnedThisFrame = true;   // add flag to playercontroller that it spawned this frame. Prolly should be called in playercontroller
                    m_PlayerController.enabled = true; // enable playercontroller
                    PlayerEventHandler.LifeRedeemed(); // call events
                    PlayerEventHandler.PlayerSpawn();
                    CameraRigController.SetVignette(false); // remove image effect
                }
            }
        }
        // we pressed suicide
        if (Input.GetButtonDown("Suicide"))
        {  
            RegularDeath();
        }
    }

    /// <summary>
    /// Handle collisions
    /// </summary>
    /// <param name="hit"></param>
    protected override void OnCharacter2DCollided(RaycastHit2D hit)
    {        
        if (!enabled)   // dont handle if we are not ghost
            return;
        base.OnCharacter2DCollided(hit);
        switch (hit.transform.gameObject.tag)
        {
            case "Light":
                // When colliding with light, bounce self to opposing direction
                m_CharacterController2D.velocity = Vector2.zero;
                Vector3 dir = (transform.position - hit.transform.position).normalized;
                Debug.DrawLine(transform.position, transform.position + dir, Color.blue, 0.1f);
                dir = Quaternion.AngleAxis((dir.x < 0) ? -m_LightBounceAngle : m_LightBounceAngle, Vector3.forward) * dir;
                Debug.DrawLine(transform.position, transform.position + dir, Color.red, 0.1f);
                m_CharacterController2D.velocity += dir * m_LightBounceForce;
                if (!myBounceSounds.isPlaying && bounceSounds.Length > 0)
                {
                    myBounceSounds.PlayOneShot(bounceSounds[Random.Range(0, bounceSounds.Length)]);
                }
                break;
        }
    }

    /// <summary>
    /// Handle trigger enters if enabled
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnCharacter2DTriggerEnter(Collider2D collider)
    {
        if (!enabled)   // dont handle if we are not ghost
            return;
        base.OnCharacter2DTriggerEnter(collider);
    }

    /// <summary>
    /// Handle trigger exits if enabled
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnCharacter2DTriggerExit(Collider2D collider)
    {
        if (!enabled)   // dont handle if we are not ghost
            return;
        base.OnCharacter2DTriggerExit(collider);
    }
    #endregion

    /// <summary>
    /// Do regular death and spawn as player
    /// </summary>
    private void RegularDeath()
    {
        transform.position = m_Spawn;       // move to spawn
        enabled = false;    // disable self
        m_PlayerController.m_SpawnedThisFrame = true;
        m_PlayerController.enabled = true; // enable playercontroller
        PlayerEventHandler.PlayerSpawn();   // call events  
        CameraRigController.SetVignette(false); // remove image effect
    }
}