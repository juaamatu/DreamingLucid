using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceController : MonoBehaviour, IButtonInteractable {

    [SerializeField] private bool m_CanShake;
    [SerializeField] private bool m_CanFlicker;
    [SerializeField] private float m_shakeAmount = 1;
    [SerializeField] private float m_shakeSpeed = 1;
    [SerializeField] private float m_TimeUntilFlick = 5f;
    [SerializeField] private float m_FlickRandomFactor = 1f;
    [SerializeField] private float m_FlickOffRandomFactor = 1f;
    [SerializeField] private float m_FlickTime = 0.2f;
    [SerializeField] SpriteRenderer m_LightSource;
    float m_ShakeTime = 0;
    float m_FlickerTime = 0;
    float m_FlickerTimeBase;
    float m_FlickTimeBase;
    float m_CenterX;
    float m_ApexX;
    int m_Direction = 1;
    public AudioClip[] clickSounds;
    public AudioSource myClickSounds;

    void Start () {
        m_CenterX = transform.position.x;
        m_FlickerTimeBase = Mathf.Clamp(Random.Range(m_TimeUntilFlick - m_FlickRandomFactor, m_TimeUntilFlick + m_FlickRandomFactor), 0.5f, m_TimeUntilFlick + m_FlickRandomFactor);
        m_FlickTimeBase = Mathf.Clamp(Random.Range(m_FlickTime - m_FlickOffRandomFactor, m_FlickTime + m_FlickOffRandomFactor), 0.05f, m_FlickTime + m_FlickOffRandomFactor);
    }
	
	void Update () {
        if (m_CanShake)
        {
            ShakeUpdate();
        }
        if (m_CanFlicker)
        {
            FlickerUpdate();
        }
	}

    /// <summary>
    /// 
    /// </summary>
    void FlickerUpdate()
    {
        m_FlickerTime += Time.deltaTime;
        if (m_LightSource.enabled && m_FlickerTime > m_FlickerTimeBase) //flick off
        {
            m_LightSource.enabled = false;
            m_FlickerTime = 0;
            m_FlickerTimeBase = Mathf.Clamp(Random.Range(m_TimeUntilFlick - m_FlickRandomFactor, m_TimeUntilFlick + m_FlickRandomFactor), 0.5f, m_TimeUntilFlick + m_FlickRandomFactor);
            myClickSounds.PlayOneShot(clickSounds[Random.Range(0, clickSounds.Length)]);
        }
        else if (!m_LightSource.enabled && m_FlickerTime > m_FlickTimeBase) //flick on
        {
            m_LightSource.enabled = true;
            m_FlickerTime = 0;
            m_FlickTimeBase = Mathf.Clamp(Random.Range(m_FlickTime - m_FlickOffRandomFactor, m_FlickTime + m_FlickOffRandomFactor), 0.05f, m_FlickTime + m_FlickOffRandomFactor);
            myClickSounds.PlayOneShot(clickSounds[Random.Range(0, clickSounds.Length)]);
        }
    }

    /// <summary>
    /// Applies shake to gameobject
    /// Moves gameobject in x asis my shake amount in shaketime
    /// </summary>
    void ShakeUpdate()
    {
        m_ApexX = m_CenterX + m_shakeAmount;
        m_ShakeTime += Time.deltaTime * m_shakeSpeed;
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.Lerp(m_CenterX + (m_shakeAmount * -m_Direction), m_CenterX + (m_shakeAmount * m_Direction), m_ShakeTime);
        transform.position = targetPosition;
        if (m_ShakeTime >= 1)
        {
            m_ShakeTime = 0;
            m_Direction = -m_Direction;
        }
    }

    public void ButtonPressed()
    {
        m_LightSource.gameObject.SetActive(!m_LightSource.gameObject.activeInHierarchy);
        
    }
}
