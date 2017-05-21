using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvasActive : MonoBehaviour {

    [SerializeField] private KeyCode m_ToggleButton;
    [SerializeField] private Canvas m_Canvas;

    void Update () {
		if (Input.GetKeyDown(m_ToggleButton))
        {
            m_Canvas.gameObject.SetActive(!m_Canvas.gameObject.activeInHierarchy);
        }
	}
}
