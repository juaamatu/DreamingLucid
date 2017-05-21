using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to zoom triggers
/// </summary>
public class ZoomTriggerController : MonoBehaviour {

    [SerializeField] private float m_OrthoSize = 1f;
    public float OrthoSize { get { return m_OrthoSize; } }
}
