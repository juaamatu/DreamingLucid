using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Juho Turpeinen
/// Controls UI Button
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler {

    private Button m_Button;    // Button component reference
    private Vector3 m_Scale;    // Base scale

    /// <summary>
    /// Get references
    /// </summary>
    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Scale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = m_Scale;
    }

    /// <summary>
    /// OnSelect doesnt fire on enable so do the selection here
    /// </summary>
    private void OnEnable()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.Equals(gameObject))
        {
            transform.localScale = m_Scale * 1.1f;
        }
    }

    /// <summary>
    /// Implements ISelectHandler
    /// </summary>
    /// <param name="e"></param>
    public void OnSelect(BaseEventData e)
    {
        transform.localScale = m_Scale * 1.1f;
    }

    /// <summary>
    /// Implements IDeselectHandler
    /// </summary>
    /// <param name="e"></param>
    public void OnDeselect(BaseEventData e)
    {
        transform.localScale = m_Scale;
    }

    /// <summary>
    /// Enables container
    /// </summary>
	public void SetContainerActive(GameObject container, bool active)
    {
        container.SetActive(active);
    }

    /// <summary>
    /// Toggles container
    /// </summary>
    public void ToggleContainerActive(GameObject container)
    {
        container.SetActive(!container.activeInHierarchy);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
