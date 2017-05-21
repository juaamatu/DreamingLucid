using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementController : MonoBehaviour, IButtonInteractable {

    [SerializeField] private Vector2[] m_Waypoints;
    [SerializeField] private float m_Speed = 1;
    private int m_CurrentIndex = 0;
    private int m_NextIndex = 1;
    private float m_AccumulatedTime = 0;
    private float m_JourneyLength;
    public bool active = true;
    private bool toggle;

    void Start ()
    {
        if (m_Waypoints.Length < 2)
        {
            Debug.LogError("GameObject " + gameObject.name + " has movement controller but less then 2 waypoints");
            enabled = false;
        }
        m_JourneyLength = Vector2.Distance(m_Waypoints[m_CurrentIndex], m_Waypoints[m_NextIndex]);
        toggle = false;
        
    }
	
	void Update ()
    {
        if (active)
        {
            m_AccumulatedTime += Time.deltaTime * m_Speed;
            float fracJourney = m_AccumulatedTime / m_JourneyLength;
            transform.position = Vector2.Lerp(m_Waypoints[m_CurrentIndex], m_Waypoints[m_NextIndex], fracJourney);
            if (fracJourney >= 1)
            {
                if (m_NextIndex == m_Waypoints.Length - 1)
                {
                    m_NextIndex = 0;
                    m_CurrentIndex++;
                }
                else
                {
                    m_CurrentIndex = m_NextIndex;
                    m_NextIndex++;
                }
                m_AccumulatedTime = 0;
                m_JourneyLength = Vector2.Distance(m_Waypoints[m_CurrentIndex], m_Waypoints[m_NextIndex]);
            }
        }
	}
    public void ButtonPressed()
    {
        active = !active;
        /* ?
        if (!toggle)
        {
            active = true;
            StartCoroutine(ToggleOn());
        }
        if (toggle)
        {
            active = false;
            StartCoroutine(ToggleOn());
        }
        */
    }

    IEnumerator ToggleOn()
    {
        yield return new WaitForSeconds(2f);
        toggle = !toggle;
    }
}
