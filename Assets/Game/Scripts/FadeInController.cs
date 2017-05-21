using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls the fading in at the start of the scene
public class FadeInController : MonoBehaviour {

    SpriteRenderer m_Sprite;        //foreground sprite
    private float t;                //time float
    private bool activate = false;  //bool to activate fading in

    void Awake()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), 1f); //move the foreground sprite to the correct position
    }

    void Start () {

        //<summary>
        //get components and start waiting coroutine
        //</summary>
        m_Sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Wait());
        m_Sprite.enabled = true;

	}

    void FixedUpdate()
    {
        if (activate) //start fading in
        {
            m_Sprite.color = Color.Lerp(Color.white, Color.clear, t);
            if (t < 1)
            {
                t += Time.deltaTime / 1f;
            }
        }
        if (m_Sprite.color == Color.clear) //destroy gameobject after sprite has faded out
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Wait() //wait a while before starting the fading in
    {
        yield return new WaitForSeconds(1f);
        activate = true;
    }
}
