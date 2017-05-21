using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerController : MonoBehaviour {

    Collider2D m_Collider;
    public FloatingKnifeEnemyController m_EnemyScript;
    public GameObject[] m_TargetInteractables;
    private IButtonInteractable[] m_ButtonInteractables;
    public bool collided = false;

    // Use this for initialization
    void Start () {

        m_Collider = GetComponent<BoxCollider2D>();
        m_Collider.isTrigger = true;
        m_ButtonInteractables = new IButtonInteractable[m_TargetInteractables.Length];
        for (int i = 0; i < m_TargetInteractables.Length; i++)
        {
            m_ButtonInteractables[i] = (IButtonInteractable)m_TargetInteractables[i].GetComponent(typeof(IButtonInteractable));
        }
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            foreach (IButtonInteractable interactable in m_ButtonInteractables)
            {
                interactable.ButtonPressed();
            }
            collided = true;
        }
        
    }
}
