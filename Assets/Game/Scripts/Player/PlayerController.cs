using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Handles player behaviour
/// </summary>
public class PlayerController : Character2DUserController {

    [SerializeField] protected float m_TimeToWaitBeforeIdleAnimations = 10;     // Time to idle before playing idle animations
    [SerializeField] protected GameObject m_DeadPlayerPrefab;                   // Prefab to spawn when dying
    [SerializeField] protected Sprite[] m_DeadPlayerSprites;                    // Sprites to apply for corpses
    [SerializeField] protected float m_SpawnProtectionDuration = 5;             // How long to be spawn protected
    [SerializeField] protected float m_SpawnProtectionAlpha = 0.5f;             // How much to change alpha when spawn protected
    [SerializeField] protected bool m_CanSuicide = true;                        // Can we commit suicide
    private GhostController m_GhostController;                                  // Ghost controller
    [HideInInspector] public List<GameObject> m_Corpses; public List<GameObject> Corpses { get { return m_Corpses; } }  // Corpse list... bad design but whatever for now
    private float m_SpawnTime;      // total time since spawn
    public bool m_IsSpawnProtected { get { return m_SpawnTime < m_SpawnProtectionDuration; } }  // are we spawn protected
    public bool m_SpawnedThisFrame = false; // flag for spawning -> animations use
    public bool m_RisedThisFrame = false;   // flag for posessing player -> animations use
    public AudioListener m_GameOverListener;

    /// <summary>
    /// Object init
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // References...
        m_GhostController = GetComponent<GhostController>();
        m_GhostController.enabled = false;
        m_Corpses = new List<GameObject>();
        m_SpawnTime = m_SpawnProtectionDuration;
        if (m_GameOverListener != null)
        {
            m_GameOverListener.enabled = false;
        }
    }

    /// <summary>
    /// Handle movement
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected override void MovementUpdate(float horizontal, float vertical)
    {
        transform.rotation = Quaternion.identity;   // Reset rotation if it is somehow changed
        base.MovementUpdate(horizontal, vertical);  // Do base calculations

        m_SpawnTime += Time.deltaTime;  // add to spawn time

        // do suicide
        if (Input.GetButtonDown("Suicide") && m_CanSuicide) // This should be moved to general update
        {
            if (!LightOverlap())
            {
                OnDeath();
            }
            else  // Cant suicide if overlapping with light
            {
                DialogueController.PlayDialog("I can't die here.", 5);
            }
        }
    }

    /// <summary>
    /// Set animation parameters for animator
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected override void AnimationUpdate(float horizontal, float vertical)
    {
        m_Animator.SetLayerWeight(1, 0);    // Set ghost layer weight to zero
        m_IdleTime = (horizontal == 0 && !m_IsGhost && !m_ClimbingLadder) ? m_IdleTime + Time.deltaTime : 0;    // add to idle time if we are idle, else reset timer
        if (m_IdleTime > m_TimeToWaitBeforeIdleAnimations && !m_IsGhost && !m_ClimbingLadder)   // If we are idle for long enough play idle animation
        {
            int random = Random.Range(1, 5); // lisää jälkimmäiseen 1
            m_Animator.Play(Animator.StringToHash("IdleVariation" + random.ToString()));
            m_IdleTime = 0;
        }
        // Set basic parameters
        m_Animator.speed = (m_ClimbingLadder && m_Velocity.y == 0 && m_ClimbAnimationOn) ? 0 : 1;
        m_Animator.SetFloat("Speed", Mathf.Abs(horizontal));
        m_Animator.SetFloat("YVelocity", m_CharacterController2D.isGrounded ? 0 : m_CharacterController2D.velocity.y);
        m_Animator.SetBool("IsGrounded", m_CharacterController2D.isGrounded);
        m_Animator.SetBool("ClimbingLadder", m_ClimbingLadder);

        // Set spawn protection alpha
        Color color = m_SpriteRenderer.color;
        if (m_IsSpawnProtected)
        {
            color.a = m_SpawnProtectionAlpha;
        }
        else
        {
            color.a = 1;
        }
        m_SpriteRenderer.color = color;

        // if we posessed corpse this frame play animation
        if (m_RisedThisFrame)
        {
            m_Animator.Play(Animator.StringToHash("CharacterRise1"));
        }
        // if we spawned this frame play animation
        if (m_SpawnedThisFrame)
        {
            m_Animator.Play(Animator.StringToHash("Spawn"));
        }

        // reset flags
        m_RisedThisFrame = false;
        m_SpawnedThisFrame = false;
    }

    /// <summary>
    /// Dont run if this not enabled. Else die when colliding with enemy
    /// </summary>
    /// <param name="hit"></param>
    protected override void OnCharacter2DCollided(RaycastHit2D hit)
    {
        if (!enabled)
            return;
        base.OnCharacter2DCollided(hit);
        switch (hit.transform.gameObject.tag)
        {
            case "Enemy":  // enemy contact! player dies
                if (m_IsSpawnProtected)
                    return;
                OnDeath();
                break;
        }
    }

    /// <summary>
    /// Dont run if this is not enabled
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnCharacter2DTriggerEnter(Collider2D collider)
    {
        if (!enabled)
            return;
        base.OnCharacter2DTriggerEnter(collider);
    }

    /// <summary>
    /// Dont run if this is not enabled
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnCharacter2DTriggerExit(Collider2D collider)
    {
        if (!enabled)
            return;
        base.OnCharacter2DTriggerExit(collider);
    }

    /// <summary>
    /// Called when gameobject is enabled
    /// </summary>
    protected void OnEnable()
    {
        if (Time.frameCount > 0)    // Set spawn protection
            m_SpawnTime = 0;
    }


    /// <summary>
    /// Handles player death
    /// </summary>
    private void OnDeath()
    {
        bool gameOver = false;  // init flag
        if (m_PlayerLifeController.LifeAmount == 0) // No lifes -> game over
        {
            GameOver();
            gameOver = true;    // set flag
        }

        if (!m_SpriteRenderer.isVisible)    // If we are not visible in camera. dont spawn corpse or spawn as ghost
        {
            if (!gameOver)  // move to spawn
            {
                transform.position = m_Spawn;
            }
            PlayerEventHandler.PlayerDeath();   // Call event
            m_SpawnTime = 0;
            return;
        }
        Collider2D[] collisions = Physics2D.OverlapBoxAll(m_Collider2D.bounds.center, m_Collider2D.bounds.size, 0);
        for (int i = 0; i< collisions.Length; i++)  // if we die in light dont spawn as ghost
        {
            if (collisions[i].tag.Equals("Light"))
            {
                SpawnCorpse();
                transform.position = m_Spawn;
                PlayerEventHandler.PlayerDeath();
                m_SpawnTime = 0;                
                return;
            }
        }
        SpawnCorpse();  // spawn corpse
        if (!gameOver)  // spawn as ghost
        {
            enabled = false;  // disable self
            m_GhostController.enabled = true;  // enable ghostcontroller
            PlayerEventHandler.GhostSpawn();
            PlayerEventHandler.PlayerDeath();
            CameraRigController.SetVignette(true);
        }
        else
        {
            if (m_GameOverListener != null)
            {
                m_GameOverListener.enabled = true;
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawns a corpse in current position
    /// </summary>
    private void SpawnCorpse()
    {
        GameObject deadPlayer;  // instansiate dead player
        deadPlayer = Instantiate(m_DeadPlayerPrefab, transform.position, transform.rotation) as GameObject;
        m_Corpses.Add(deadPlayer);  // add dead player to list of corpses
        if (m_Corpses.Count > 3) // remove last corpse if count is over limit
        {
            Destroy(m_Corpses[0]);
            m_Corpses.RemoveAt(0);
        }
        if (m_Corpses.Count > 1)    // Destroy animations and change sprite in older corpses
        {
            for (int i = 0; i < m_Corpses.Count - 1; i++)
            {
                Animator animator = m_Corpses[i].GetComponent<Animator>();
                if (animator != null)
                {
                    Destroy(animator);
                }
                m_Corpses[i].GetComponent<SpriteRenderer>().sprite = m_DeadPlayerSprites[m_Corpses.Count - 1 - i];
            }
        }
        Collider2D corpseCollider = deadPlayer.GetComponent<Collider2D>();  // get collider from corpse and move player on top
        deadPlayer.transform.localScale = transform.localScale;
        transform.position = deadPlayer.transform.position + Vector3.up * (corpseCollider.bounds.extents.x + m_Collider2D.bounds.extents.y);
    }
}
