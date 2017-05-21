using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Juho Turpeinen
// Controls camera that moves with target
// TODO Proper map clamping and focus shift
public class FollowCameraController : MonoBehaviour {

    public enum CameraMoveMode { Pan, TargetSwitch }
    [SerializeField] private float m_DistanceToPlayer = 3f;     // How far away in x-axis are we from player
    [SerializeField] private float m_SmoothTime = 0.3F;         // How smoothly camera follows target
    [SerializeField] private CameraMoveMode m_CameraMoveMode = CameraMoveMode.Pan;  // How do we want to focus camera on another object
    [SerializeField] private bool m_UseCustomBounds = false;    // How smoothly camera follows target
    [SerializeField] private Collider2D m_CustomBoundsCollider; // Custom bounds offered by collider
    public CameraMoveMode MoveMode { get { return m_CameraMoveMode; } } // Property so we can see the value from pan script
    private Camera m_Camera;                                    // Camera component reference
    private Transform m_PlayerTransform;                        // Player transform reference
    private Transform m_Target;                                 // Target to follow
    private Vector3 m_Velocity;                                 // Reference value for current follow velocity
    private float m_MinY;                                       // Map min y
    private float m_MinX;                                       // Map min x
    private float m_MaxX;                                       // Map max x
    private float m_MaxY;                                       // Map max y
    private float m_PanTime = 0;                                // How long have we panned in current pan
    private float m_PanDuration = 0;                            // Total time for pan
    public float DistanceToPlayer { get { return m_DistanceToPlayer; } }
    public Transform Target { get { return m_Target; } }
    private bool m_IsPanning = false;

    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    void Start () {        

        // Set references
        m_Camera = GetComponent<Camera>();
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_Target = m_PlayerTransform;

        SetBounds(m_Camera.orthographicSize);
    }
	
	void LateUpdate () {
        Vector3 targetPosition = m_Target.position + (Vector3.back * m_DistanceToPlayer); // position we are trying to reach
        // clamp values to fit map
        targetPosition.x = Mathf.Clamp(targetPosition.x, m_MinX, m_MaxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, m_MinY, m_MaxY);
        // calculate position for this frame
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_Velocity, m_SmoothTime);
    }   

    // calculate fps if we want to see it
    private void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        
    }

    // Calculate map bounds
    Bounds OrthographicBounds(float orthographicSize, Vector3 mapCenter, float aspect)
    {
        float screenAspect = aspect;
        float cameraHeight = orthographicSize * 2;
        Bounds bounds = new Bounds(
            mapCenter,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    /// <summary>
    /// Pan to target
    /// </summary>
    /// <param name="target">target we are panning</param>
    /// <param name="duration">pan duration</param>
    public void PanToTarget(Transform target, float duration)
    {
        m_PanDuration = duration;
        m_PanTime = 0;
        m_Target = target;
        m_IsPanning = true;
    }

    public void MoveCamera(Transform target)
    {
        m_Target = target;
    }
    public void MoveCameraBack()
    {
        m_Target = m_PlayerTransform;
    }

    /// <summary>
    /// Sets camera bounds
    /// </summary>
    /// <param name="size"></param>
    public void SetBounds(float size)
    {
        Bounds staticBounds = OrthographicBounds(2.4f, Vector3.zero, 1.3333f);  // whole map bounds
        Bounds dynamicBounds = OrthographicBounds(size, Vector3.zero, (float)Screen.width / (float)Screen.height);  // bounds for current camera
        // Calculate map bounds
        if (m_UseCustomBounds)
        {
            m_Camera.orthographicSize = Mathf.Clamp(size, 0, (m_CustomBoundsCollider.bounds.max.y - m_CustomBoundsCollider.bounds.min.y) / 2);
            float screenAspect = (float)Screen.width / (float)Screen.height;
            m_MinX = m_CustomBoundsCollider.bounds.min.x + m_Camera.orthographicSize * screenAspect;
            m_MinY = m_CustomBoundsCollider.bounds.min.y + m_Camera.orthographicSize;
            m_MaxX = m_CustomBoundsCollider.bounds.max.x - m_Camera.orthographicSize * screenAspect;
            m_MaxY = m_CustomBoundsCollider.bounds.max.y - m_Camera.orthographicSize;
        }
        else
        {
            m_MinX = staticBounds.min.x + dynamicBounds.extents.x;
            m_MinY = staticBounds.min.y + dynamicBounds.extents.y;
            m_MaxX = staticBounds.max.x - dynamicBounds.extents.x;
            m_MaxY = staticBounds.max.y - dynamicBounds.extents.y;
        }
    }

    /// <summary>
    /// Sets camera target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    /// <summary>
    /// Resets camera target to player
    /// </summary>
    public void SetTargetToPlayer()
    {
        m_Target = m_PlayerTransform.transform;
    }
}
