using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoomController : MonoBehaviour {

    [SerializeField] GameObject m_Visible;
    [SerializeField] GameObject m_Hidden;
    public bool ghostEnabled;
    private bool isGhost;

    void Start()
    {
        PlayerEventHandler.OnDeath += Ghost;
        PlayerEventHandler.OnRespawn += NotGhost;
        isGhost = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Hidden.SetActive(true);
        m_Visible.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Hidden.SetActive(false);
        m_Visible.SetActive(true);
    }

    void Update()
    {
        if (ghostEnabled && isGhost)
        {
            m_Hidden.SetActive(true);
            m_Visible.SetActive(false);
        }
        if (ghostEnabled && !isGhost)
        {
            m_Hidden.SetActive(false);
            m_Visible.SetActive(true);
        }
        
    }

    void Ghost()
    {
        isGhost = true;
    }

    void NotGhost()
    {
        isGhost = false;
    }
}
