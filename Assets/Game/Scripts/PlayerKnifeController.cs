using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnifeController : MonoBehaviour {

    [SerializeField] private Vector3 m_LocalPosition = new Vector3(0.123f, -0.04f);

	private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            transform.SetParent(collider.transform);
            transform.localScale = Vector3.one;
            transform.localPosition = m_LocalPosition;            
        }
    }
}
