using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Pans camera to center so player can see full map
/// </summary>
public class CameraPanToCenterController : MonoBehaviour {

    [SerializeField] private float m_MaxOrthoSize = 2.4f;        // Maximium orho size. 2.4 is standard
    private FollowCameraController m_FollowCameraController;    // Reference to main follow script so we can disable it while panning
    private CameraZoomController m_CameraZoomController;        // Reference to zoom controller
    private Camera m_Camera;    // Camera reference
    private Transform m_PlayerTransform;    // Player transform for position tracking
    private float m_OrthoSize = 1;      // Original ortho size;
    private float m_StartOrthoSize;     // Start value for lerping
    private float m_TargetOrthoSize;    // Target value for lerping
    private Vector3 m_StartPosition;    // Start value for lerping
    private Vector3 m_TargetPosition;   // End value for lerping
    private float m_Time = 0;   // Time for lerping
    private bool m_IsPanning = false;   // Are we currently panning
    private bool m_PanningToPlayer;     // Are we panning towards player

    private float m_Dist;
    private float m_OrthoDist;

    /// <summary>
    /// Called when object is initalized.
    /// Get references
    /// </summary>
    private void Awake()
    {
        m_FollowCameraController = GetComponent<FollowCameraController>();
        m_CameraZoomController = GetComponent<CameraZoomController>();
        m_Camera = GetComponent<Camera>();        
    }

    /// <summary>
    /// Called just before first frame
    /// Get player information
    /// </summary>
    private void Start()
    {
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Called every frame after normal Update()
    /// </summary>
    private void LateUpdate()
    {
        m_Time += Time.deltaTime;
        if (m_IsPanning)
        {
            float frac = m_Time * 2 / m_Dist; // get even zoom for different distances
            m_Camera.orthographicSize = Mathf.Lerp(m_StartOrthoSize, m_TargetOrthoSize, frac);
            Vector3 targetPosition = Vector3.Lerp(m_StartPosition, m_TargetPosition, frac);
            Vector4 bounds = CalculateBounds(m_Camera.orthographicSize);
            targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.x, bounds.y);   // Clamp position x to fit map
            targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.z, bounds.w);   // Clamp position y to fit map
            transform.position = targetPosition;
            if (frac     >= 1 && m_PanningToPlayer)
            {
                m_IsPanning = false;
                m_FollowCameraController.enabled = true;
            }
        }
        if (Input.GetButtonDown("Zoom"))
        {
            PanToCenter();
        }
        if (Input.GetButtonUp("Zoom"))
        {
            PanToPlayer();
        }
    }

    /// <summary>
    /// Gets bounds for ortho size
    /// </summary>
    /// <param name="orthographicSize">Desired ortho size</param>
    /// <param name="mapCenter">Where map in centered</param>
    /// <returns></returns>
    private Bounds OrthographicBounds(float orthographicSize, Vector3 mapCenter)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = orthographicSize * 2;
        Bounds bounds = new Bounds(
            mapCenter,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    /// <summary>
    /// Calculate bounds for camera;
    /// </summary>
    Vector4 CalculateBounds(float size)
    {
        Bounds staticBounds = OrthographicBounds(2.4f, Vector3.zero);  // whole map bounds
        Bounds dynamicBounds = OrthographicBounds(m_Camera.orthographicSize, Vector3.zero);  // bounds for current camera
        // Calculate map bounds
        float minX = staticBounds.min.x + dynamicBounds.extents.x;
        float minY = staticBounds.min.y + dynamicBounds.extents.y;
        float maxX = staticBounds.max.x - dynamicBounds.extents.x;
        float maxY = staticBounds.max.y - dynamicBounds.extents.y;
        return new Vector4(minX, maxX, minY, maxY);
    }

    /// <summary>
    /// Starts to pan camera to center and full screen
    /// </summary>
    public void PanToCenter()
    {
        m_FollowCameraController.enabled = false;        
        m_StartOrthoSize = m_Camera.orthographicSize;
        m_TargetOrthoSize = m_MaxOrthoSize;
        m_StartPosition = transform.position;
        m_TargetPosition = new Vector3(0, 0, transform.position.z);
        m_Time = 0;
        m_IsPanning = true;
        m_PanningToPlayer = false;
        m_Dist = Vector3.Distance(m_StartPosition, m_TargetPosition);
        m_OrthoDist = Mathf.Abs(m_StartOrthoSize - m_TargetOrthoSize);
    }

    /// <summary>
    /// Starts to pan camera back to player
    /// </summary>
    public void PanToPlayer()
    {
        m_TargetOrthoSize = m_CameraZoomController.Size;
        m_FollowCameraController.enabled = false;        
        m_StartOrthoSize = m_Camera.orthographicSize;
        m_StartPosition = transform.position;
        m_TargetPosition = m_FollowCameraController.Target.position + Vector3.back * m_FollowCameraController.DistanceToPlayer;
        m_Time = 0;
        m_IsPanning = true;
        m_PanningToPlayer = true;
        m_Dist = Vector3.Distance(m_StartPosition, m_TargetPosition);
        m_OrthoDist = Mathf.Abs(m_StartOrthoSize - m_TargetOrthoSize);
    }
}
