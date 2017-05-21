using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Juho Turpeinen
/// Controls canvas that is attached to player
/// </summary>
public class PlayerCanvasController : MonoBehaviour {

    [SerializeField] private Image m_SpawnUpImage;  // Image for spawn up
    private Transform m_Player;         // Parent player transform
    private CanvasGroup m_CanvasGroup;  // Canvas group reference
    private float m_TargetAlpha = 0;    // Target alpha for lerp
    private PlayerLifeController m_PlayerLifeController;

    /// <summary>
    /// Get references
    /// </summary>
	void Start () {
        m_Player = GetComponentInParent<Character2DUserController>().transform;
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_PlayerLifeController = GetComponentInParent<PlayerLifeController>();
	}
	
    /// <summary>
    /// Run late update so it can use information done in parent scripts
    /// </summary>
	void LateUpdate () {
        if (PlayerEventHandler.IsGhost)
        {
            // Reset scale because otherwise it follows parent scale
            transform.localScale = new Vector3((m_Player.localScale.x * transform.localScale.x < 0) ? -transform.localScale.x : transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (PlayerEventHandler.GameObjectUnderPlayer != null && PlayerEventHandler.GameObjectUnderPlayer.tag.Equals("Corpse"))
            {
                m_CanvasGroup.alpha = 1;
                m_SpawnUpImage.enabled = PlayerEventHandler.GameObjectUnderPlayer.Equals(PlayerEventHandler.Corpses[PlayerEventHandler.Corpses.Count - 1]);
            }
            else
            {
                m_CanvasGroup.alpha = 0;
            }
        }
        else
        {
            m_CanvasGroup.alpha = 0;
        }
	}
}
