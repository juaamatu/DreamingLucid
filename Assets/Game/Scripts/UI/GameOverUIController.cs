using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour {

    [SerializeField] private string m_GameOverLevel = "";
    private static GameOverUIController m_GameOverUIController;
    private Animator m_Animator;
    private bool m_GameOver = false;
    public AudioSource m_Music;
    private bool m_PlayOnce = true;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        if (m_GameOverUIController == null)
        {
            m_GameOverUIController = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (m_GameOver && Input.GetButtonDown("Jump"))
        {
            if (m_GameOverLevel.Equals(""))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                m_Music.Stop();
            }
            else
            {
                SceneManager.LoadScene(m_GameOverLevel);
                m_Music.Stop();
            }
            m_GameOverUIController.m_Animator.SetTrigger("Exit");
        }
        if (m_GameOver && !m_Music.isPlaying && m_PlayOnce)
        {
            m_Music.Play();
            m_PlayOnce = false;
            Destroy(GameObject.FindGameObjectWithTag("Music"));
            Destroy(GameObject.FindGameObjectWithTag("Music2"));
        }
    }

    public static void FadeIn()
    {
        m_GameOverUIController.m_Animator.SetTrigger("FadeIn");
        m_GameOverUIController.m_GameOver = true;
    }
}
