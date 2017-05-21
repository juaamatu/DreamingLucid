using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Petro Pitkänen
// Makes Pause menu not destroy on loading another scene
public class UIDontDestroyController : MonoBehaviour {

	void Awake () {
        DontDestroyOnLoad(transform.gameObject);	
	}

}
