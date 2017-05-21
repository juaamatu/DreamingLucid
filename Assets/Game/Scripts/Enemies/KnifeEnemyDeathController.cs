using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Controls the death of knife enemy
/// </summary>
public class KnifeEnemyDeathController : MonoBehaviour {

    [SerializeField] private GameObject m_PlayerObtainableKnife;        // Knife that is instansiated e.g. the one player can pick up
    [SerializeField] private Transform m_Knife;     // Knife that can kill player
    [SerializeField] private AudioSource m_DeathSound; // Sound for dying
    private Animator m_Animator;    // Animator reference
    private Vector3 m_DeathPos;     // Death position e.g where do we want to die
    private Vector3 m_StartPos;     // Lerp start pos
    private float m_Time;           // Lerp time 
    private GameObject m_Dialogue;

    /// <summary>
    /// Set references and disable self
    /// </summary>
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        enabled = false;
        m_Dialogue = GameObject.FindGameObjectWithTag("Dialogue");
        m_Dialogue.SetActive(false);
    }

    /// <summary>
    /// Move to deathpos and play animation, instansiate knife and destroy self
    /// </summary>
    private void Update()
    {
        
        m_Time += Time.deltaTime * 4;
        transform.position = Vector3.Lerp(m_StartPos, m_DeathPos, m_Time);
        if (m_Time >= 1)
        {
            m_Animator.SetTrigger("Death");     // Play animation
            m_DeathSound.Play();
            Instantiate(m_PlayerObtainableKnife, m_Knife.position, m_Knife.rotation);   // Instansiate knife for player     
            Destroy(gameObject, 2); // Destroy after a while TODO: better time 
            Destroy(m_Knife.gameObject);
            enabled = false;    // disable updates
            m_Dialogue.SetActive(true);
        }
    }

    /// <summary>
    /// Starts the death process
    /// </summary>
    /// <param name="deathPos">position where we want to die</param>
    public void StartKnifeEnemyDeath(Vector3 deathPos)
    {
        enabled = true;
        m_StartPos = transform.position;
        m_DeathPos = deathPos;
        m_Time = 0;
        Destroy(m_Knife.GetComponent<Collider2D>());
    }
}
