using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Juho Turpeinen
/// Controls sliders that control audio
/// </summary>
[ExecuteInEditMode]
public class MenuSliderController : MonoBehaviour, ISelectHandler, IDeselectHandler {

    [SerializeField] private UnityEngine.Audio.AudioMixer m_AudioMixer;     // AudioSystem to control
    [SerializeField] private Slider m_Slider;   // Slider reference where we get values
    [SerializeField] private string m_AudioParameterName;   // Parameter name -> value we want to change
    [SerializeField] float m_MinValue = -80;    // Min value
    [SerializeField] float m_MaxValue = 0;      // Max value
    private Vector3 m_Scale;    // Start scale -> used to highlight selection
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    /// <summary>
    /// Called before first frame.
    /// Set refeenced and start values
    /// </summary>
    void Start()
    {
        float value;
        m_AudioMixer.GetFloat(m_AudioParameterName, out value);
        float fixedValue = (value - m_MinValue) / Mathf.Abs(m_MaxValue - m_MinValue);
        m_Slider.value = fixedValue;
        m_Scale = transform.parent.localScale;

        // float fixedValue = 1f * Mathf.Pow(1.55f, ((m_Slider.value * -1) / 8));
        float vol;
        m_AudioMixer.GetFloat(m_AudioParameterName, out vol);
        m_Slider.value = Mathf.Pow(10f, vol / 20f);
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
    /// Called when gameobject is disabled
    /// Reset scale
    /// </summary>
    private void OnDisable()
    {
        transform.parent.localScale = m_Scale;
        OnDeselected.Invoke();
    }
 
    /// <summary>
    /// Called when slider is updated.
    /// TODO: Change from logarithm to linear
    /// </summary>
    public void UpdateSliderValue () {
        float fixedValue = 1f * Mathf.Pow(1.55f, ((m_Slider.value * -1) / 8));
        m_AudioMixer.SetFloat(m_AudioParameterName, fixedValue * -1);
        float vol;
        m_AudioMixer.GetFloat(m_AudioParameterName, out vol);
    }

    /// <summary>
    /// Called when object is selected by event system
    /// </summary>
    /// <param name="e"></param>
    public void OnSelect(BaseEventData e)
    {
        transform.parent.localScale = m_Scale * 1.1f;
        OnSelected.Invoke();
    }

    /// <summary>
    /// Called when gameobject is deselected by event system
    /// </summary>
    /// <param name="e"></param>
    public void OnDeselect(BaseEventData e)
    {
        transform.parent.localScale = m_Scale;
        OnDeselected.Invoke();
    }

    private void Update()
    {

    }
}
