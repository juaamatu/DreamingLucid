using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen, Juho Turpeinen
// Controls dialogue triggers
public class MonologueTriggerController : MonoBehaviour {

    [SerializeField] public string dialogueOnTrigger;       //dialogue subtitle to play
    [SerializeField] private GameObject[] m_TargetInteractables;    //array of interactables to trigger
    private IButtonInteractable[] m_ButtonInteractables;    //array of interactables to trigger
    public bool triggerButtonPress = false;     //bool to check if you want to trigger button press
    AudioSource line;       //audiosource for the dialogue
    SpriteRenderer sprite;  //debug spriterenderer
    private bool lineActive = false;    //check if the line has been delivered
    public bool keyActivated = false;   //bool to check if you want the dialogue to trigger on a keypress
    private bool inTrigger = false;     //check if the player is in the trigger
    private bool isGhost = false;       //check if the player is a ghost
    public bool onlyGhostTriggerable = false;   //bool to check if you want the dialogue to trigger only as a ghost

	void Start () {

        //<summary>
        //get components, turn off debugsprite, find interactable objects
        //</summary>
        sprite = GetComponent<SpriteRenderer>();
        line = GetComponent<AudioSource>();
        sprite.enabled = false;
        //copy paste from ButtonController script
        m_ButtonInteractables = new IButtonInteractable[m_TargetInteractables.Length];
        for (int i = 0; i < m_TargetInteractables.Length; i++)
        {
            m_ButtonInteractables[i] = (IButtonInteractable)m_TargetInteractables[i].GetComponent(typeof(IButtonInteractable));
            // Log error if iterated object does not have script that impelements IButtonInteractable
            if (m_ButtonInteractables[i] == null)
            {
                Debug.LogError("Could not find component of type " + m_ButtonInteractables.GetType().ToString() + " in " + m_TargetInteractables[i].name);
            }
        }
    }

    void Update()
    {
        if (!line.isPlaying && lineActive) //destroys the object after line has been delivered
        {
            Destroy(gameObject);
        }
        if (keyActivated && Input.anyKeyDown && inTrigger && !lineActive && !line.isPlaying && !isGhost) //plays the line, if player is in desired trigger and presses a desired button
        {
            line.Play();
            lineActive = true;
            DialogueController.PlayDialog(dialogueOnTrigger, 4);
            if (triggerButtonPress) //triggers button press
            {
                foreach (IButtonInteractable interactable in m_ButtonInteractables)
                {
                    interactable.ButtonPressed();
                }
            }

        }
        if (PlayerEventHandler.IsGhost) //checks if player is a ghost
        {
            isGhost = true;
        }
        if (!PlayerEventHandler.IsGhost) //checks if player is not a ghost
        {
            isGhost = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collider) //check collisions
    {
        //plays the line when the player enters the trigger, if it isn't keyactivated and if the player is not a ghost
        if (collider.gameObject.tag.Equals("Player") && !line.isPlaying && !lineActive && !keyActivated && !isGhost && !onlyGhostTriggerable)
        {
            line.Play();
            lineActive = true;
            DialogueController.PlayDialog(dialogueOnTrigger, 4);
            foreach (IButtonInteractable interactable in m_ButtonInteractables)
            {
                interactable.ButtonPressed();
            }
        }
        //plays the line when the player enters the trigger, if it isn't keyactivated and if the player is a ghost
        if (collider.gameObject.tag.Equals("Player") && !line.isPlaying && !lineActive && !keyActivated && isGhost && onlyGhostTriggerable) 
        {
            line.Play();
            lineActive = true;
            DialogueController.PlayDialog(dialogueOnTrigger, 4);
            foreach (IButtonInteractable interactable in m_ButtonInteractables)
            {
                interactable.ButtonPressed();
            }
        }
        inTrigger = true; //checks when the player is in the trigger
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        inTrigger = false; //checks when the player leaves the triggers
    }

    
}
