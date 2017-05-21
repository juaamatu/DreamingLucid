using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ActiveSpriteOnEvent : MonoBehaviour {

    public enum EnableEvent { PlayerSpawn, PlayerDeath, GhostSpawn, None }
    public enum DisableEvent { PlayerSpawn, PlayerDeath, GhostSpawn, None }
    [SerializeField] EnableEvent m_EnableEvent = EnableEvent.PlayerDeath;
    [SerializeField] DisableEvent m_DisableEvent = DisableEvent.PlayerSpawn;
    private SpriteRenderer m_SpriteRenderer;

	void Start () {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerEventHandler.OnDeath += OnDeath;
        PlayerEventHandler.OnRespawn += OnRespawn;
        PlayerEventHandler.OnGhostSpawn += OnRespawn;
    }


    

    void OnDeath()
    {
        if (m_EnableEvent == EnableEvent.PlayerDeath && PlayerEventHandler.IsGhost)
        {
            m_SpriteRenderer.enabled = true;
        }
        if (m_DisableEvent == DisableEvent.PlayerDeath)
        {
            m_SpriteRenderer.enabled = false;
        }

    }

    void OnRespawn()
    {
        if (m_EnableEvent == EnableEvent.PlayerSpawn)
        {
            m_SpriteRenderer.enabled = true;
        }
        if (m_DisableEvent == DisableEvent.PlayerSpawn)
        {
            m_SpriteRenderer.enabled = false;
        }
    }

    void OnGhostSpawn()
    {
        if (m_EnableEvent == EnableEvent.GhostSpawn && !PlayerEventHandler.IsGhost)
        {
            m_SpriteRenderer.enabled = true;
        }
        if (m_DisableEvent == DisableEvent.GhostSpawn && !PlayerEventHandler.IsGhost)
        {
            m_SpriteRenderer.enabled = false;
        }
    }
}
