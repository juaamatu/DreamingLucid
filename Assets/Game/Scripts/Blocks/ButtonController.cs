using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Juho Turpeinen, Petro Pitkänen
// Controls button objects
public class ButtonController : MonoBehaviour, IButtonCallbackable {

    [SerializeField] private GameObject[] m_TargetInteractables;            // All the objects that should interact when button in being interacted with
    [SerializeField] private Transform m_CameraPanTarget;                   // Where should we pan camera on button press
    [SerializeField] private bool m_PlayDialogOnTriggerEnter = false;       // Should we play dialog on button press
    [SerializeField] private bool m_PlayDialogOnlyOnce = false;             // Play dialog only once?
    [SerializeField] private string m_DialogOnTriggerEnter = "Press E to press button.";    // Dialog content
    [SerializeField] private bool m_TimeOut = false;        // Should button "uninteract" after a period of time?
    [SerializeField] private float m_TimeOutTime = 1;       // How long till button "uninteracts"
    [SerializeField] private bool m_UseOnlyOnce = false;        // How long till button "uninteracts"
    //[SerializeField] private bool m_UpOnExit = false;       // Should button bounce back after player exits
    private Animator m_Animator;
    private bool m_DialogPlayed = false;                    // Has dialog been played once    
    private Collider2D m_Collider;                          // Collider reference
    private bool m_TriggerOverlap = false;                  // Is trigger overlapping
    private IButtonInteractable[] m_ButtonInteractables;    // All the scripts from m_TargetInteractables that implement IButtonInteractable
    private AudioSource m_AudioSource;                      // Audio source reference
    private bool m_UseLock = false;                         // Can button be interacted with
    private bool m_UsedOnce = false;
    private bool m_BouncingBack = false;
    private BoxCollider2D m_BoxCollider;

    private bool m_Down = false;


    public MoveButtonCameraController m_CameraMove; 
    public float waitAfterButtonPress;
    public float waitForTrigger;
    public float waitForCameraBack;
    public float disableMovementTime;

    void Awake()
    {
        // Find references
        m_Collider = GetComponent<Collider2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Animator = GetComponent<Animator>();
    }

    void Start()
    {
        m_BoxCollider = GetComponentInChildren<BoxCollider2D>();
        // Try to get scripts that impelement IButtonInteractable from m_TargetInteractables
        m_ButtonInteractables = new IButtonInteractable[m_TargetInteractables.Length];
        for (int i = 0; i < m_TargetInteractables.Length; i++)
        {
            m_ButtonInteractables[i] = (IButtonInteractable)m_TargetInteractables[i].GetComponent(typeof(IButtonInteractable));
            // Log error if iterated object does not have script that impelements IButtonInteractable
            if (m_ButtonInteractables[i] == null)
            {
                Debug.LogError("Could not find component of type " + m_ButtonInteractables.GetType().ToString() + " in " + m_TargetInteractables[i].name);
            }
        }
    }

    void Update()
    {
       /* if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Up"))
        {
            m_BouncingBack = false;
        }*/
        if (m_BouncingBack && m_Down)
        {
            Vector2 size = new Vector2(m_BoxCollider.size.x, m_BoxCollider.size.y * 2f);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, size, 0);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag.Equals("Corpse") || colliders[i].gameObject.tag.Equals("Player"))
                {
                    return;
                }
            }
            Press();
            m_BouncingBack = false;
        }
    }
	
    // Called from player script when colliding against trigger
	public void TriggerEnter2D(Collider2D collider)
    {
        // if player is above collider and use lock is not in effect
        if ((collider.transform.position - transform.position).y > 0.22f && !m_UseLock && (!m_UseOnlyOnce || m_UseOnlyOnce.Equals(!m_UsedOnce)))
        {
            m_UsedOnce = true;
            Press(); // press button
            m_UseLock = true; // uselock on
            StartCoroutine(WaitLock()); // wait out waitlock
            if (m_TimeOut && !m_UseOnlyOnce) // if using timeout
            {
                StartCoroutine(WaitForBounceBack()); // bounce back
            }
        }
    }

    public void TriggerExit2D(Collider2D collider)
    {
        if (!m_TimeOut && !m_UseOnlyOnce)
        {
            m_BouncingBack = true;
        }
    }

    void Press()
    {
        m_Down = !m_Down;
        m_AudioSource.Play();
        GetComponent<Animator>().SetTrigger("Press");
        OnActionReady();
        m_UsedOnce = true;
        PlayerEventHandler.DisablePlayerInput(disableMovementTime);
    }

    IEnumerator WaitLock()
    {
        yield return new WaitForSeconds(.8f);
        m_UseLock = false;
    }

    IEnumerator WaitForBounceBack()
    {
        yield return new WaitForSeconds(m_TimeOutTime);
        m_BouncingBack = true;
    }

    public void OnActionReady()
    {
        foreach (IButtonInteractable interactable in m_ButtonInteractables)
        {            
            interactable.ButtonPressed();
        }
    }
}