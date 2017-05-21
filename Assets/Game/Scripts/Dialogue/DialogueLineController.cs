using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Juho Turpeinen
/// <summary>
/// Controls one line of dialogue
/// </summary>
[RequireComponent(typeof(Text))]
public class DialogueLineController : MonoBehaviour {

    private Text m_Text;                    // Text component containing line
    private RectTransform m_RectTransform;  // Rect transform component
    private float m_AliveTime = 5;          // How long to display text
    private float m_AliveTimer = 0;         // Timer for how long this has been alive
    private float m_AliveStartY;            // We start to lerp text from under the screen (-RectTransform.heigth)
    private float m_ScrollAmount = 60f;     // How up we want to scroll text
    private float m_ScrollStartY;           // Start scroll pos, used with lerping
    private float m_ScrollEndY;             // End scoll pos, used with lerping
    private float m_ScrollTime = 0;         // Scroll lerp time

    /// <summary>
    /// Called when object has been initialized. Used to create component references
    /// </summary>
	void Awake () {
        m_Text = GetComponent<Text>();
        m_RectTransform = GetComponent<RectTransform>();
        m_AliveStartY = -m_RectTransform.sizeDelta.y;   // Just under the screen
        m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, m_AliveStartY);
        m_Text.text = "DefaultText -> text has not been set in DialogueLineController";
	}
	
    /// <summary>
    /// Called every frame
    /// </summary>
	void Update () {
        m_AliveTimer += Time.deltaTime;     // Add time to timer
        if (m_AliveTimer <= 1)  // If alive time is less then one -> fade subtitle in
        {
            float y = Mathf.Lerp(m_AliveStartY, 0, m_AliveTimer);
            m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, y);
        }
        if (m_AliveTime - m_AliveTimer <= 1)    // Fade out
        {
            m_Text.color = Color.Lerp(Color.white, Color.clear, 1 - (m_AliveTime - m_AliveTimer));
        }
        if (m_ScrollStartY != m_ScrollEndY)   // Scroll up if needed
        {
            m_ScrollTime += Time.deltaTime;
            float y = Mathf.Lerp(m_ScrollStartY, m_ScrollEndY, m_ScrollTime);
            m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, y);
        }
        if (m_AliveTimer > m_AliveTime) // Destroy gameobject when finished
        {
            Destroy(gameObject);
        }
	}

    /// <summary>
    /// Init this dialogue line
    /// </summary>
    /// <param name="aliveTime">How long do we show this dialogue</param>
    /// <param name="text">Text to display as subtitles</param>
    public void Init(float aliveTime, string text)
    {
        m_AliveTime = aliveTime;
        m_Text.text = text;
    }

    /// <summary>
    /// Sets the text for text component
    /// </summary>
    /// <param name="text">Text to be set</param>
    public void SetText(string text)
    {
        m_Text.text = text;
    }
    
    /// <summary>
    /// Starts to scroll text up
    /// </summary>
    public void ScrollUp()
    {
        m_ScrollStartY = m_RectTransform.anchoredPosition.y;
        m_ScrollEndY = m_ScrollEndY + m_ScrollAmount;
        m_ScrollTime = 0;
    }
}
