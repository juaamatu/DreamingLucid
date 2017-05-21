using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    public delegate void CollisionEnterAction(Collision2D collision);
    public event CollisionEnterAction OnCollisionEnter;
    public delegate void TriggerEnterAction(Collider2D collider);
    public event TriggerEnterAction OnTriggerEnter;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnCollisionEnter != null)
            OnCollisionEnter(collision);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("moi");
        if (OnTriggerEnter != null)
            OnTriggerEnter(collider);
    }
}
