using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraRigController : MonoBehaviour {

    static CameraRigController controller;
    static bool vignetteOn = false;
    [SerializeField] private GameObject m_StaticCamera;
    [SerializeField] private GameObject m_FollowCamera;
    [SerializeField] private KeyCode m_ColorCorrectionToggle;
    [SerializeField] private KeyCode m_ContrastEnchanceToggle;

    private void Awake()
    {
        if (controller == null)
            controller = this;
        else
            Destroy(gameObject);
    }

    void Update () {
        if (Input.GetKeyDown(m_ColorCorrectionToggle))
        {
            ToggleColorCorrection();
        }
        if (Input.GetKeyDown(m_ContrastEnchanceToggle))
        {
            ToggleContrastEnhance();
        }

    }

    public static void SetVignette(bool active)
    {
        LensAberrations[] effects = controller.gameObject.GetComponentsInChildren<LensAberrations>();
        foreach (LensAberrations effect in effects)
        {
            effect.enabled = active;
        }
        vignetteOn = active;
    }

    static void ToggleColorCorrection()
    {
        ColorCorrectionCurves[] effects = controller.gameObject.GetComponentsInChildren<ColorCorrectionCurves>();
        foreach (ColorCorrectionCurves effect in effects)
        {
            effect.enabled = !effect.enabled;
        }
    }

    static void ToggleContrastEnhance()
    {
        ContrastEnhance[] effects = controller.gameObject.GetComponentsInChildren<ContrastEnhance>();
        foreach (ContrastEnhance effect in effects)
        {
            effect.enabled = !effect.enabled;
        }
    }
}
