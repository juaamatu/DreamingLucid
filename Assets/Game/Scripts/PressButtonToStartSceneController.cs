using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressButtonToStartSceneController : MonoBehaviour {

    [SerializeField] private KeyCode m_StartKey = KeyCode.Space;
    [SerializeField] private string m_LevelName;

    void Update () {
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Suicide"))
        {
            SceneManager.LoadScene(m_LevelName);
        }
	}
}
