using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Destroys music, if another music is found
public class DestroyMusicController : MonoBehaviour {

	void Awake () {

		if (GameObject.FindGameObjectWithTag("Music"))
        {
            Destroy(gameObject);
        }
	}

}
