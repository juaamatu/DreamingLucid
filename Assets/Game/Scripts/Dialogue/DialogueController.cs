using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Juho Turpeinen
/// <summary>
/// Adds dialogue subtitles and audio to game
/// </summary>
public class DialogueController : MonoBehaviour {

    [SerializeField] private CanvasGroup m_CanvasGroup;         // Canvas group reference
    [SerializeField] private Text m_SubtitleText;               // Text reference
    [SerializeField] private GameObject m_DialogueLine;         // Dialogue line object
    private static DialogueController m_Controller;             // Singleton reference
    private List<DialogueLineController> m_Lines;               // All lines showing in screen
    private AudioSource m_AudioSource;                          // AudioSource component

    /// <summary>
    /// Set singleton
    /// </summary>
    void Awake() 
    {
        if (m_Controller == null)
            m_Controller = this;
        else
            Destroy(gameObject);
        m_Lines = new List<DialogueLineController>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays subtitles for a duration
    /// </summary>
    /// <param name="subtitle">subtitle string to show on screen</param>
    /// <param name="duration">how long to show subtitles</param>
    public static void PlayDialog(string subtitle, float duration)
    {
        m_Controller.PlayDialogSingleton(subtitle, duration, null);   // Call method from singleton object
    }

    /// <summary>
    /// Plays subtitles for a duration and also play audio
    /// </summary>
    /// <param name="subtitle">subtitle string to show on screen</param>
    /// <param name="duration">how long to show subtitles</param>
    /// <param name="clip">audio clip to play</param>
    public static void PlayDialog(string subtitle, float duration, AudioClip clip)
    {
        m_Controller.PlayDialogSingleton(subtitle, duration, clip);   // Call method from singleton object
    }

    // Bad naming... 
    /// <summary>
    /// Instansiates the dialogue line
    /// </summary>
    /// <param name="subtitle">subtitle to create subtitle line with</param>
    /// <param name="duration">duration for line</param>
    /// <param name="clip">audio clip to play</param>
    private void PlayDialogSingleton(string subtitle, float duration, AudioClip clip)
    {
        // Instansiate dialogueLine
        GameObject dialogueLine = Instantiate(m_DialogueLine, m_DialogueLine.transform.position, m_DialogueLine.transform.rotation) as GameObject;
        dialogueLine.transform.SetParent(transform, false);     // Set parent so rect transform values wont be fucked up
        DialogueLineController controller = dialogueLine.GetComponent<DialogueLineController>();    // Get controller
        controller.Init(duration, subtitle);    // Init controller with desired alivetime and subtitle text
        DialogueLineController[] lines = m_Lines.ToArray(); // add to tmp array
        m_Lines = new List<DialogueLineController>();   // Reset line list
        for (int i = 0; i < lines.Length; i++)  // Remove empty entries and call scroll up with else
        {
            if (lines[i] != null)
            {
                m_Lines.Add(lines[i]);
                lines[i].ScrollUp();
            }
        }
        m_Lines.Add(controller);    // Add controller to list

        if (clip != null)
        {
            m_AudioSource.PlayOneShot(clip);
        }
    }
}
