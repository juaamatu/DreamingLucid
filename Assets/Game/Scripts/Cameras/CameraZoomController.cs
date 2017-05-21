using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Controls camera zoom and targets
/// </summary>
[RequireComponent(typeof(Camera), typeof(FollowCameraController))]
public class CameraZoomController : MonoBehaviour {

    private Camera m_Camera;    // Camera reference
    private FollowCameraController m_FollowCameraController;    // Camera that controls camera movement
    private float m_StartSize = 1;  // Start size for lerp
    private ZoomTriggerController m_TargetController;   // Target controller that has information about zoom level
    private float m_Time = 0;   // Time used for lerpong
    public float Size { get { return (m_TargetController != null) ? m_TargetController.OrthoSize : 1; } }

    /// <summary>
    /// Get references
    /// </summary>
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
        m_FollowCameraController = GetComponent<FollowCameraController>();
    }

    /// <summary>
    /// Move towards wanted orhtosize
    /// </summary>
    private void Update()
    {
        m_Time += Time.deltaTime;
        if (m_TargetController != null)
        {
            m_Camera.orthographicSize = Mathf.Lerp(m_StartSize, m_TargetController.OrthoSize, m_Time);
        }
        else
        {
            m_Camera.orthographicSize = Mathf.Lerp(m_StartSize, 1, m_Time);
            m_FollowCameraController.SetBounds(m_Camera.orthographicSize);
        }
    }

    /// <summary>
    /// Called from Character2DUserController when triggering zoom trigger
    /// </summary>
    /// <param name="collider"></param>
    public void Enter(Collider2D collider)
    {
        m_Time = 0;
        m_StartSize = m_Camera.orthographicSize;
        m_TargetController = collider.GetComponent<ZoomTriggerController>();
        m_FollowCameraController.SetBounds(m_TargetController.OrthoSize);
        m_FollowCameraController.SetTarget(m_TargetController.transform);
    }

    /// <summary>
    /// Called from Character2DUserController when triggering zoom trigger
    /// </summary>
    /// <param name="collider"></param>
    public void Exit(Collider2D collider)
    {
        if (m_TargetController.Equals(collider.GetComponent<ZoomTriggerController>()))
        {
            m_StartSize = m_Camera.orthographicSize;
            m_Time = 0;
            m_TargetController = null;            
            m_FollowCameraController.SetTargetToPlayer();
        }
    }
}
