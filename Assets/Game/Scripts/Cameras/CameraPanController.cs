using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Pans camera between object and player
/// </summary>
[RequireComponent(typeof(FollowCameraController))]
public class CameraPanController : MonoBehaviour {

    private FollowCameraController m_FollowCameraController;    // Main follow script reference
    private Camera m_Camera;                                    // Camera reference
    private Transform m_Target;                                 // Target transform
    private IButtonCallbackable m_IButtonCallbackable;          // Callback when camera has panned
    private Vector3 m_PanCenterPoint;                           // Center point for pan (lerp target)
    private float m_PanTargetOrthoSize;                         // Ortho size for pan (lerp target)
    private float m_PanStartOrthoSize;                          // Start ortho size (lerp start)
    private Vector3 m_PanStartPosition;                         // Start point (lerp start)
    private float m_PanTime;                                    // Pan timer
    private Transform m_PlayerTransform;                        // Reference to player transform
    public enum PanPhase { In, Stay, Out, None }                // Pan phases
    private PanPhase m_PanPhase = PanPhase.None;                // Current pan phase

    /// <summary>
    /// Setup references
    /// </summary>
    void Awake () {
        m_FollowCameraController = GetComponent<FollowCameraController>();      
        m_Camera = GetComponent<Camera>();
        m_PanStartOrthoSize = m_Camera.orthographicSize;
        enabled = false;
    }
	
    /// <summary>
    /// Pan camera in late update after player has moved
    /// </summary>
	void LateUpdate () {
        m_PanTime += Time.deltaTime;    // Add to pan timer
        switch (m_PanPhase)             // Go through pan phases
        {
            case PanPhase.In:           // Pan in
                m_PanStartPosition = m_PlayerTransform.position;
                m_PanStartPosition.z = transform.position.z;
                LerpCamera();           // Lerp in to pan point
                if (m_PanTime >= 1)     // ---> Next pan phase
                {
                    m_PanTime = 0;      // Reset timer
                    m_PanPhase = PanPhase.Stay;         // Next we want to stay in pan position for a while
                    if (m_IButtonCallbackable != null)  // Make callback if possible
                    {
                        m_IButtonCallbackable.OnActionReady();
                    }
                }
                break;
            case PanPhase.Stay:         // Dont do anything, just wait
                if (m_PanTime >= 1)     // Prepare out phase
                {
                    m_PanStartPosition = transform.position;    // Switch target positions
                    m_PanCenterPoint = m_PlayerTransform.position + (Vector3.back * m_FollowCameraController.DistanceToPlayer);
                    float tmpOrtho = m_PanStartOrthoSize;       // Switch target ortho values
                    m_PanStartOrthoSize = m_PanTargetOrthoSize;
                    m_PanTargetOrthoSize = tmpOrtho;
                    m_PanTime = 0;                              // Reset timer
                    m_PanPhase = PanPhase.Out;                  // Next we want to move out
                }
                break;
            case PanPhase.Out:          // Pan out
                LerpCamera();           // Lerp in to start position
                if (m_PanTime >= 1)     // ---> Move away from panning
                {
                    m_FollowCameraController.enabled = true;    // Enable main follow camera monobehaviour
                    enabled = false;                            // disable self monobehaviour
                    m_PanTime = 0;                              // Reset timer
                    m_PanPhase = PanPhase.None;                 // Reset pan
                }
                break;
        }
    }

    /// <summary>
    /// Lerps camera to target
    /// Lerp x is pan timer, so lerp time is always 1 second
    /// </summary>
    private void LerpCamera()
    {
        m_Camera.orthographicSize = Mathf.Lerp(m_PanStartOrthoSize, m_PanTargetOrthoSize, m_PanTime); // Lerp camera ortho size
        Vector3 targetPosition = m_PanCenterPoint;  // Set target position
        targetPosition = Vector3.Lerp(m_PanStartPosition, m_PanCenterPoint, m_PanTime); // Lerp position
        Vector4 bounds = CalculateBounds(m_PanTargetOrthoSize);    // Get position map bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.x, bounds.y);   // Clamp position x to fit map
        targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.z, bounds.w);   // Clamp position y to fit map
        transform.position = targetPosition;
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
    /// Start to pan camera towards object
    /// </summary>
    /// <param name="target">Transform to pan towards</param>
    /// <param name="callback">Callback for action</param>
    public void PanToTarget(Transform target, IButtonCallbackable callback)
    {
        switch (m_FollowCameraController.MoveMode)
        {
            case FollowCameraController.CameraMoveMode.Pan:
                if (enabled)
                {
                    return;
                }
                m_FollowCameraController.enabled = false;   // Disable main script for a duration of pan
                this.enabled = true;                        // Enable self for a duration of pan
                m_IButtonCallbackable = callback;           // Set callback reference
                m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;       // Update player transform reference
                m_Target = target;
                m_PanStartPosition = transform.position;    // Start position for pan
                m_PanCenterPoint = m_PlayerTransform.position + (target.position - m_PlayerTransform.position) / 2; // Target point for pan
                m_PanCenterPoint.z = transform.position.z;  // Reset target point depth
                m_PanStartOrthoSize = m_Camera.orthographicSize;    // Current ortho size
                m_PanTargetOrthoSize = (target.position - m_PlayerTransform.position).magnitude / 2 * (Screen.width / Screen.height); // Target ortho size
                m_PanTargetOrthoSize = (m_PanTargetOrthoSize < m_PanStartOrthoSize) ? m_PanStartOrthoSize : m_PanTargetOrthoSize;    // Clamp min orthi size
                m_PanTargetOrthoSize = Mathf.Clamp(m_PanTargetOrthoSize, 0, 2.4f);
                m_PanPhase = PanPhase.In;   // Start pan
                m_PanTime = 0;  // Reset timer
                break;
            case FollowCameraController.CameraMoveMode.TargetSwitch:
                m_FollowCameraController.PanToTarget(target, 2);
                StartCoroutine(SetCallBack(2 / 2, callback));
                break;
        }
    }

    IEnumerator SetCallBack(float duration, IButtonCallbackable callback)
    {
        yield return new WaitForSeconds(duration);
        callback.OnActionReady();
    }
}
