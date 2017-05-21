using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlockController : MonoBehaviour, IButtonInteractable {

    [SerializeField] private float m_MaxActiveTime = 5;
    [Range(0.0f, 1.0f)] [SerializeField] private float m_GhostAlpha = 0.2f;
    [Range(0.0f, 1.0f)] [SerializeField] private float m_NormalAlpha = 1;
    Collider2D m_Collider;
    bool m_Active = false;
    private float m_ActiveTime = 0;
    private Material m_Material;

	protected virtual void Start () {
        m_Collider = GetComponent<Collider2D>();
        m_Material = GetComponent<Renderer>().material;
        SetGhost(false);
    }

    protected virtual void Update () {
		if (m_Active)
        {
            m_ActiveTime += Time.deltaTime;
            if (m_ActiveTime >= m_MaxActiveTime)
            {                
                m_Active = false;
                SetGhost(m_Active);
            }
        }
	}

    public void ButtonPressed()
    {
        if (!m_Active)
        {
            m_Active = true;
            m_ActiveTime = 0;
            SetGhost(m_Active);
        }
    }

    protected virtual void SetGhost(bool active)
    {
        Color color = m_Material.GetColor("_Color");
        color.a = (active) ? m_NormalAlpha : m_GhostAlpha;
        m_Material.SetColor("_Color", color);
        m_Collider.enabled = (active) ? true : false;
    }
}
