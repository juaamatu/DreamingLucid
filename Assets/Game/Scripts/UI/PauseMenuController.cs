using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Juho Turpeinen
/// Controls pause menu canvas
/// </summary>
public class PauseMenuController : MonoBehaviour {

    [SerializeField] private GameObject m_Container;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_ControlsMenu;
    GameObject m_Audio;

    /// <summary>
    /// Listens to cancel button and sets menu container active.
    /// Sets focus to first selectable and disables/enables player input
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Back") && m_Container.activeInHierarchy)
        {
            if (m_ControlsMenu.activeInHierarchy)
            {
                m_ControlsMenu.SetActive(false);
                m_MainMenu.SetActive(true);
            }
            else
            {
                m_Container.SetActive(false);
                PlayerEventHandler.EnablePlayerInput();                
                m_Audio.SetActive(true);
            }
        }
        if (Input.GetButtonDown("Menu"))
        {
            if (m_Container.activeInHierarchy && m_ControlsMenu.activeInHierarchy && DetectGamepad.Keyboard == 1)
            {
                m_ControlsMenu.SetActive(false);
                m_MainMenu.SetActive(true);
            }
            else
            {
                m_Container.SetActive(!m_Container.activeInHierarchy);
                if (m_Container.activeInHierarchy)
                {
                    m_ControlsMenu.SetActive(false);
                    m_MainMenu.SetActive(true);
                    Selectable selectable = GetComponentInChildren<Selectable>();
                    selectable.Select();
                    EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                    PlayerEventHandler.DisablePlayerInput();
                    m_Audio = GameObject.FindGameObjectWithTag("playerAudio");
                    m_Audio.SetActive(false);
                }
                else
                {
                    PlayerEventHandler.EnablePlayerInput();
                    m_Audio.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Callback from MenuSliderController OnSelected
    /// </summary>
    public void MusicSliderSelected()
    {
    }

    /// <summary>
    /// Callback from MenuSliderController OnDeselected
    /// </summary>
    public void MusicSliderDeselected()
    {
    }
}
