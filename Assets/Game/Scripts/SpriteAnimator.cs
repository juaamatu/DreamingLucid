using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {

    [SerializeField] private Sprite[] m_Sprites;
    [SerializeField] private float m_FrameTime = 0.1f;
    [SerializeField] private bool m_PlayOnce = false;
    [SerializeField] private bool m_DeleteOnEnd = false;
    SpriteRenderer m_SpriteRenderer;
    float m_AccumulatedTime = 0;
    int m_CurrentIndex = 0;

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (m_Sprites.Length == 0)
            return;
        m_AccumulatedTime += Time.deltaTime;
        if (m_AccumulatedTime >= m_FrameTime)
        {
            if (m_CurrentIndex == m_Sprites.Length - 1 && (m_PlayOnce || m_DeleteOnEnd))
            {
                enabled = false;
                if (m_DeleteOnEnd)
                    Destroy(gameObject);
                return;
            }
            m_CurrentIndex = (m_CurrentIndex != m_Sprites.Length - 1) ? m_CurrentIndex + 1 : 0;
            m_SpriteRenderer.sprite = m_Sprites[m_CurrentIndex];
            m_AccumulatedTime = 0;
        }
    }
}
