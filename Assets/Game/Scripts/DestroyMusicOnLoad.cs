using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Destroys the music if found
public class DestroyMusicOnLoad : MonoBehaviour {

    public bool m_DestroyMusic1 = true;
    public bool m_DestroyMusic2 = true;

    void Start() //destroy music object if found
    {
        if (m_DestroyMusic1)
        {
            Destroy(GameObject.FindGameObjectWithTag("Music"));
        }
        if (m_DestroyMusic2)
        {
            Destroy(GameObject.FindGameObjectWithTag("Music2"));
        }
    }
}
