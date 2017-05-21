using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementControllerAlternative : MonoBehaviour, IButtonInteractable {

    [SerializeField] private Transform m_WaypointParent;
    [SerializeField] private float m_Speed = 1;
    private Transform[] m_Waypoints;
    private Vector2[] m_WaypointVectors;
    private int m_CurrentIndex = 0;
    private int m_NextIndex = 1;
    private float m_AccumulatedTime = 0;
    private float m_JourneyLength;

    void Start ()
    {
        m_WaypointVectors = new Vector2[m_WaypointParent.childCount];
        for (int i = 0; i < m_WaypointParent.childCount; i++)
        {
            m_WaypointVectors[i] = m_WaypointParent.GetChild(i).position;
        }
        if (m_WaypointVectors.Length < 2) 
        {
            Debug.LogError("GameObject " + gameObject.name + " has movement controller but less then 2 waypoints");
            enabled = false;
        }
        m_JourneyLength = Vector2.Distance(m_WaypointVectors[m_CurrentIndex], m_WaypointVectors[m_NextIndex]);
    }
	
	void Update ()
    {
        m_AccumulatedTime += Time.deltaTime * m_Speed;
        float fracJourney = m_AccumulatedTime / m_JourneyLength;
        transform.position = Vector2.Lerp(m_WaypointVectors[m_CurrentIndex], m_WaypointVectors[m_NextIndex], fracJourney);
        if (fracJourney >= 1)
        {
            if (m_NextIndex == m_WaypointVectors.Length - 1)
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
            m_JourneyLength = Vector2.Distance(m_WaypointVectors[m_CurrentIndex], m_WaypointVectors[m_NextIndex]);
        }
	}

    public void ButtonPressed()
    {
        enabled = !enabled;
    }
}
