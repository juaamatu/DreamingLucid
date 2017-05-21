using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.parent = transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.parent = transform.root.parent;
    }
}
