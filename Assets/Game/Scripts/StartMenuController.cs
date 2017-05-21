using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour {

    public Transform m_TargetCamera;
    private float t;
    private Vector3 targetPosition;
    private bool startFade = false;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public string m_LevelName;
	private float startSize;
	private float endSize;
	private Camera cam;
    private Image[] m_Images;
    private Text[] m_Texts;
    private Vector3 m_StartPos;

	// Use this for initialization
	void Start () {
        m_StartPos = transform.position;
        Selectable selectable = GetComponentInChildren<Selectable>();
        selectable.Select();
        m_Images = GetComponentsInChildren<Image>();
        m_Texts = GetComponentsInChildren<Text>();
        cam = GetComponentInParent<Camera>();
		startSize = cam.orthographicSize;
		endSize = 0.7802451f;
    }
	
	// Update is called once per frame
	void Update () {
        if (startFade)
        {
            for (int i = 0; i < m_Images.Length; i++)
            {
                m_Images[i].color = Color.Lerp(Color.white, Color.clear, t);
            }
            for (int i = 0; i < m_Texts.Length; i++)
            {
                m_Texts[i].color = Color.Lerp(Color.white, Color.clear, t);
            }
            cam.orthographicSize = Mathf.Lerp (startSize, endSize, t);
            if (t < 1)
            {
                t += Time.deltaTime / 3f;
            }
            transform.position = Vector3.Lerp(m_StartPos, m_TargetCamera.position, t);
        }
        if (t > 1)
        {
            SceneManager.LoadScene(m_LevelName);
        }
	}

    private void OnApplicationFocus(bool focus)
    {
        Selectable selectable = GetComponentInChildren<Selectable>();
        selectable.Select();
    }

    public void StartFade()
    {
        startFade = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
