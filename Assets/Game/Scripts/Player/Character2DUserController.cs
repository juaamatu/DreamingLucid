using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Juho Turpeinen
// Catches user input and calls movement from CharacterController2D
// Handles collisions
[RequireComponent(typeof(CharacterController2D), typeof(Animator))]
public class Character2DUserController : MonoBehaviour {

    #region Attributes/Properties
    #region Serialized
    [SerializeField] protected float m_JumpHeight = 1;          // Jump force
    [SerializeField] protected float m_Gravity = -1;            // Gravity scale
    [SerializeField] protected float m_GroundDamping = 20f;     // How fast do we change direction? higher means faster
    [SerializeField] protected float m_InAirDamping = 5f;       // Damping for in air movement
    [SerializeField] protected float m_RunSpeed = 8f;           // Run speed multiplier
    [SerializeField] protected float m_ClimbSpeed = 0.8f;       // Climp speed multiplier
    #endregion
    #region Components
    protected SpriteRenderer m_SpriteRenderer;                      // Renderer refernce
    protected CharacterController2D m_CharacterController2D;        // Handles movement
    protected PlayerParticlesController m_PlayerParticlesController;    // Reference for particles controller
    protected Animator m_Animator;  // Animtor reference 
    protected Collider2D m_Collider2D;  // Collider reference
    protected PlayerLifeController m_PlayerLifeController;  // Player life reference
    protected static CameraZoomController m_CameraZoomController;
    #endregion
    #region Movement
    protected Vector3 m_Velocity;   // reference value paster for movement
    #endregion
    #region Flags
    protected bool m_JumpedThisFrame = false;               // Did player jump this frame
    protected bool m_RespawnedThisFrame = false;            // Did player spawn this frame
    protected bool m_GhostSpawnedThisFrame = false;         // Did player ghost spawn this frame
    protected bool m_LadderOverlap = false;                 // Is there ladder overlap
    protected bool m_ClimbingLadder = false;                // Is player climbing ladder
    protected bool m_CanClimb = false;                      // Is player ladder climb enabled
    protected bool m_IsOnMovingTile = false;                // Is player on moving tile
    protected bool m_IsGhost = false;                       // Is player ghost
    protected bool m_CanMove = true;                        // Is movement locked
    #endregion
    #region Properties
    protected bool m_IsSpawning { get { return (m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterSpawnAnimation") /*|| m_Animator.GetCurrentAnimatorClipInfo(1)[0].clip.name.Equals("CharacterGhostSpawnAnimation")*/);} }
    protected bool m_IsDying { get { return m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterDeathAnimation"); } }
    protected bool m_ClimbAnimationOn { get
        {
            return (m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterClimbLadderAnimation")
                || m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterBlackClimbLadderAnimation"));
        }
    }
    public bool IsGhost { get { return m_IsGhost; } }
    public bool JumpedThisFrame { get { return m_JumpedThisFrame; } }
    public bool CanMove
    {
        get
        {
            return (!(m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterRise1Animation")
                    || m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterSpawnAnimation")
                    || m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("CharacterBlackRise1Animation")
                    || m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("BlackAndWhiteCharacterSpawnAnimation")
                    )
                    && m_CanMove);
        }
        set
        {
            m_CanMove = value;
        }
    }
    #endregion
    #region Misc
    static protected Vector3 m_Spawn;       // Spawn point
    protected float m_IdleTime = 0;         // Amount of time player has been inactive (not moving)
    protected LayerMask m_PlayerPlatformMask;       // Layer mask for what playercontroller2d considers to be ground    
    protected Collider2D m_LadderCollider;  // Collider reference for overlapping collider
    protected Material m_Material;          // Material for player
    #endregion
    #endregion

    #region Monobehaviour callbacks

    /// <summary>
    /// Called when object is initialized
    /// </summary>
    protected virtual void Awake()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Get component references
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Animator = GetComponent<Animator>();
        m_PlayerParticlesController = GetComponentInChildren<PlayerParticlesController>();
        m_Collider2D = GetComponent<Collider2D>();
        m_PlayerLifeController = GetComponent<PlayerLifeController>();
        // Callbacks for collision/trigger overlaps
        m_CharacterController2D.onTriggerEnterEvent += OnCharacter2DTriggerEnter;
        m_CharacterController2D.onTriggerExitEvent += OnCharacter2DTriggerExit;
        m_CharacterController2D.onControllerCollidedEvent += OnCharacter2DCollided;
        m_Spawn = transform.position;   // Set spawn
        m_PlayerPlatformMask = m_CharacterController2D.platformMask;    // ?
        m_Material = m_SpriteRenderer.material;     // Material reference
    }

    /// <summary>
    /// Called before first frame
    /// </summary>
    protected virtual void Start()
    {
        // Get references from other objects
        if (Camera.main != null)
        {
            m_CameraZoomController = Camera.main.GetComponent<CameraZoomController>();
        }
        else
        {
            Debug.LogWarning("Did not find main camera object when trying to assign CameraZoomController");
        }
    }

    /// <summary>
    /// Called every frame. Call update methods in right order
    /// </summary>
    protected virtual void Update () {

        m_CharacterController2D.jumpedThisFrame = false;
        float horizontal = 0;
        float vertical = 0;
        if (CanMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");      // Horizontal input (-1...1)
            vertical = Input.GetAxisRaw("Vertical");         // Vertical input (-1...1)
        }
        MovementUpdate(horizontal, vertical);
        AnimationUpdate(horizontal, vertical);
        GeneralUpdate(horizontal, vertical);
    }
    #endregion

    #region Collisions

    /// <summary>
    /// Charactercontroller subscribed event for trigger enters
    /// TODO: use switch case
    /// </summary>
    /// <param name="collider">collided collider</param>
    protected virtual void OnCharacter2DTriggerEnter(Collider2D collider)
    {
        // Check one moving tiles
        OneWayTileController oneWayTileController;
        if (collider.tag.Equals("MovingTile"))
        {
            // calculate bottom point for player collider
            Vector3 bottom = m_Collider2D.bounds.center + Vector3.down * m_Collider2D.bounds.extents.y;
            if (bottom.y >= collider.bounds.center.y)
            {
                transform.parent = collider.transform;
                m_IsOnMovingTile = true;
            }
        }

        // Check one way tiles
        else if (oneWayTileController = collider.transform.gameObject.GetComponent<OneWayTileController>())
        {
            oneWayTileController.Disable(this.GetType() == typeof(GhostController));
        }

        // Check ladders
        else if (collider.gameObject.tag.Equals("Ladder"))
        {
            m_LadderOverlap = true;
            m_LadderCollider = collider;
            m_CanClimb = true;
        }

        // Check checkpoints
        else if (collider.gameObject.tag.Equals("Checkpoint"))
        {
            Collider2D checkPointCollider = collider.transform.parent.GetComponent<Collider2D>();
            if (checkPointCollider != null)
            {
                Vector3 topPoint = checkPointCollider.bounds.center + Vector3.up * checkPointCollider.bounds.extents.y;
                topPoint += Vector3.up * m_Collider2D.bounds.size.y / 2;
                topPoint.z = transform.position.z;
                m_Spawn = topPoint;
                Destroy(collider.gameObject);
            }
        }

        // lifes
        else if (collider.gameObject.tag.Equals("Life"))
        {
            if (m_PlayerLifeController.LifeAmount < m_PlayerLifeController.MaxLifeAmount)
            {
                m_PlayerLifeController.AddLife();
                Destroy(collider.gameObject);
            }
        }

        // buttons -> try to find buttoncontroller and call triggerenter
        else if (collider.gameObject.tag.Equals("Button"))
        {
            ButtonController buttonController = collider.GetComponent<ButtonController>();
            if (buttonController != null)
            {
                buttonController.TriggerEnter2D(m_Collider2D);
            }
            else
            {
                Debug.LogError("Did not find ButtonController in button " + collider.gameObject.name);
            }
        }

        // Handle camera zoom
        else if (collider.gameObject.tag.Equals("ZoomTrigger"))
        {
            m_CameraZoomController.Enter(collider);
        }
    }

    /// <summary>
    /// Charactercontroller subscribed event for trigger exits
    /// TODO: use switch case
    /// </summary>
    /// <param name="collider">collided collider</param>
    protected virtual void OnCharacter2DTriggerExit(Collider2D collider)
    {
        // Check one way moving tiles
        OneWayTileController oneWayTileController;
        if (collider.tag.Equals("MovingTile"))
        {
            transform.parent = collider.transform.root.parent;
            m_IsOnMovingTile = false;
        }
        else if (oneWayTileController = collider.transform.gameObject.GetComponent<OneWayTileController>())
        {
            oneWayTileController.Enable(m_IsGhost);
        }

        // Check ladders
        else if (collider.gameObject.tag.Equals("Ladder"))
        {
            m_LadderOverlap = false;
            m_ClimbingLadder = false;            
        }

        else if (collider.gameObject.tag.Equals("Button"))
        {
            ButtonController buttonController = collider.GetComponent<ButtonController>();
            if (buttonController != null)
            {
                buttonController.TriggerExit2D(m_Collider2D);
            }
            else
            {
                Debug.LogError("Did not find ButtonController in button " + collider.gameObject.name);
            }
        }

        else if (collider.gameObject.tag.Equals("ZoomTrigger"))
        {
            m_CameraZoomController.Exit(collider);
        }
    }

    /// <summary>
    /// Charactercontroller subscribed event for collisions
    /// </summary>
    /// <param name="hit">raycast hit</param>
    protected virtual void OnCharacter2DCollided(RaycastHit2D hit)
    {        
        // parent self on moving tile on enter
        if (m_IsOnMovingTile && !hit.transform.tag.Equals("MovingTile"))
        {
            transform.parent = transform.root.parent;
        }
    }
    #endregion

    #region Helper functions

    /// <summary>
    /// Checks if character fully overlaps with ladder in y axis
    /// </summary>
    /// <returns></returns>
    protected virtual bool FullLadderOverlap(float direction)
    {
        Vector3 pos = new Vector3(transform.position.x, m_Collider2D.bounds.center.y + m_Collider2D.bounds.extents.y * direction);
        Collider2D[] colliders = Physics2D.OverlapPointAll(pos);
        foreach (Collider2D collider in colliders)
        {
            if (collider == m_LadderCollider)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if we overlap a ladder
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckLadderOverLap()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_Collider2D.bounds.center, m_Collider2D.bounds.size, 0);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag.Equals("Ladder"))
            {
                m_LadderCollider = colliders[i];
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Stuff to do animated things in update
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected virtual void AnimationUpdate(float horizontal, float vertical)
    {
        
    }

    /// <summary>
    /// Handles movement
    /// </summary>
    /// <param name="horizontal">horizontal input</param>
    /// <param name="vertical">vertical input</param>
    protected virtual void MovementUpdate(float horizontal, float vertical)
    {
        if (horizontal != 0)    // If movement is not zero -> flip character to face right direction
        {
            horizontal = (horizontal > 0) ? 1 : -1;
            if (horizontal > 0 && transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (horizontal < 0 && transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        // An awful lot of checks to see if we cant start to climb ladder....
        else if (vertical != 0 && m_LadderOverlap && ((FullLadderOverlap(vertical) && vertical > 0.8) || vertical < -0.8) && !m_ClimbingLadder && (m_CharacterController2D.isGrounded || m_CanClimb))
        {
            m_ClimbingLadder = true;
            transform.position = new Vector3(m_LadderCollider.bounds.center.x, transform.position.y, transform.position.z);
        }

        // check if we cant jump and jump if posible
        if (m_CanMove && (m_CharacterController2D.isGrounded || (m_ClimbingLadder && horizontal != 0)) && Input.GetButtonDown("Jump"))      // Can we jump?
        {
            m_Velocity.y = Mathf.Sqrt(2f * m_JumpHeight * -m_Gravity);      // Calculate vertical velocity
            m_CharacterController2D.jumpedThisFrame = true;                 // Set flag
            m_ClimbingLadder = false;                                       // Cancel ladder climb
            m_Animator.SetTrigger("Jump");                                  // Play animation     
            m_ClimbingLadder = false;       
        }

        if ((m_ClimbingLadder && horizontal != 0 && vertical == -1) || (m_ClimbingLadder && vertical == -1 && m_CharacterController2D.isGrounded))
        {   
            m_ClimbingLadder = false;
        }

        // apply movement parameters for charactercontroller
        float smoothedMovementFactor = m_CharacterController2D.isGrounded ? m_GroundDamping : m_InAirDamping;  // How fast do we change direction?
        m_Velocity.x = Mathf.Lerp(m_Velocity.x, horizontal * m_RunSpeed, Time.deltaTime * smoothedMovementFactor);
        m_Velocity.x = (m_ClimbingLadder) ? 0 : m_Velocity.x;
        m_Velocity.y = (m_ClimbingLadder) ? m_ClimbSpeed * vertical * Time.deltaTime : m_Velocity.y + m_Gravity * Time.deltaTime;

        m_CharacterController2D.move(m_Velocity * Time.deltaTime);
        m_Velocity = m_CharacterController2D.velocity;      // Grab our current velocity to use as a base for all calculations
    }

    /// <summary>
    /// Stuff for inherited stuff to do general stuff in update
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    protected virtual void GeneralUpdate(float horizontal, float vertical)
    {

    }

    /// <summary>
    /// Starts gameover process from gameover ui
    /// </summary>
    protected virtual void GameOver()
    {
        GameOverUIController.FadeIn();
        m_CanMove = false;
    }

    /// <summary>
    /// Checks if collider overlaps light colliders
    /// </summary>
    /// <returns>is overlapping light collider</returns>
    protected bool LightOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_Collider2D.bounds.center, m_Collider2D.bounds.size, 0);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag.Equals("Light"))
            {
                return true;
            }
        }
        return false;
    }
}
#endregion