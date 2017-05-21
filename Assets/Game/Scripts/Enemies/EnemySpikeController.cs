using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Petro Pitkänen, Juho Turpeinen
/// Controls spikedudes spike behaviour
/// </summary>
public class EnemySpikeController : MonoBehaviour {

    private bool m_PlayerIsNear;                // is player object near
    Transform m_PlayerTransform;                // reference to player transform
    public float speed;                         // spike speed
    public float m_AggroDistance;               // distance to aggro from player
    private bool isGhost;                       // is player ghost
    Transform m_CenterTransform;                // parent transform
    private Collider2D m_Collider;              // collider reference
    private PlayerController m_PlayerController;    // player controller reference so we can see if player is spawn protected
    private int layer;                          // original layer for this object
    AudioSource m_Audio;                        // audio for spike
    private bool audioPlayed;

    void Start()
    {
        // Find references
        m_Collider = GetComponent<Collider2D>();
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_PlayerController = m_PlayerTransform.GetComponent<PlayerController>();
        m_CenterTransform = transform.parent;
        isGhost = false;
        layer = gameObject.layer;
        m_Audio = GetComponent<AudioSource>();
        audioPlayed = false;
    }
	
	void Update () {
        // set object layer depending on player spawn protection
        gameObject.layer = (m_PlayerController.m_IsSpawnProtected) ? 0 : layer;
        if (m_PlayerTransform != null) // rotate to player direction
        {
            Vector3 vectorToTarget = m_PlayerTransform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
        }
        // is player near
        m_PlayerIsNear = ((m_PlayerTransform.position - m_CenterTransform.position).magnitude <= m_AggroDistance && !isGhost);
        if (m_PlayerIsNear && !isGhost && !m_PlayerController.m_IsSpawnProtected) // move spike towards player
        {
            // calculate target position
            Vector3 target = m_CenterTransform.position + (m_PlayerTransform.position - m_CenterTransform.position).normalized * m_Collider.bounds.size.x;
            // lerp towards position
            Vector3 position = Vector3.Lerp(transform.position, target, .05f);
            transform.position = position;
            if (!m_Audio.isPlaying && !audioPlayed)
            {
                m_Audio.Play();
                audioPlayed = true;
            }
        }
        else
        {
            // lerp towards start position
            transform.position = Vector3.Lerp(transform.position, m_CenterTransform.position, .05f);
        }
        if (transform.position == m_CenterTransform.position)
        {
            audioPlayed = false;
        }
    }

}
