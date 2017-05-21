using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Destroys Pause Menu if found
public class DestroyPauseMenuController : MonoBehaviour {

	void Start () {

        Destroy(GameObject.FindGameObjectWithTag("PauseMenu"));

	}

}
