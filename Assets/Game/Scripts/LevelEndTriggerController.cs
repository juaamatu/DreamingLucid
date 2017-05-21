using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelEndTriggerController : MonoBehaviour {

    Collider2D m_Collider;
    [SerializeField] string m_LevelName;
    public SpriteRenderer m_FadeOut;
    private bool activateFade = false;
    private float t;
    private bool isGhost;

	void Start () {
        m_Collider = GetComponent<Collider2D>();
        m_Collider.isTrigger = true;
        m_FadeOut.color = (Color.clear);
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Player") && !isGhost)
        {
            activateFade = true;
        }
    }

    private void FixedUpdate()
    {
        if (activateFade)
        {
            m_FadeOut.transform.position = Vector3.Lerp(m_FadeOut.transform.position, new Vector3(0f, 0f, 0f), 1f);
            m_FadeOut.color = Color.Lerp(Color.clear, Color.white, t);
            if (t < 1)
            {
                t += Time.deltaTime / 1f;
            }
        }
        if (m_FadeOut.color == (Color.white))
        {
            SceneManager.LoadScene(m_LevelName);
        }
        if (PlayerEventHandler.IsGhost)
        {
            isGhost = true;
        }
        if (!PlayerEventHandler.IsGhost)
        {
            isGhost = false;
        }
    }
}
