using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    [SerializeField] private LockedDoorController m_LockedDoorController;
    [SerializeField] private GameObject m_ItemInteractable;
    [SerializeField] private Sprite m_HudSprite;
    int m_HudIndex;

    void OnTriggerEnter2D(Collider2D collider)
    {
        IItemInteractable interactable = (IItemInteractable)m_ItemInteractable.GetComponent(typeof(IItemInteractable));
        if (interactable != null)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                m_HudIndex = ItemHUDController.AddImage(m_HudSprite);
                if (m_HudIndex != -1)
                {
                    interactable.ItemPickedUp(m_HudIndex);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.LogWarning("Did not find interactable item in KeyController field " + m_ItemInteractable.name);
        }        
    }
}
