using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Juho Turpeinen
/// Controls the opening of locked doors
/// </summary>
public class LockedDoorController : MonoBehaviour, IItemInteractable, IButtonInteractable {

    [SerializeField] private GameObject[] m_DoorTiles;      // Tiles that make the door
    public string m_DialogOnTriggerEnter1;  // Dialogue message to play when entering trigger area
    public string m_DialogOnTriggerEnter2;
    public string m_DialogOnTriggerEnter3;
    public AudioSource m_Dialogue1; //audio dialogue to play when entering trigger area
    public AudioSource m_Dialogue2;
    public AudioSource m_Dialogue3;
    private bool m_CanOpen;     // flag if key has been obtained
    [SerializeField] private AudioSource lockedSound; // Sound to play when entering trigger but door is locked
    [SerializeField] private AudioSource openSound;     // Sound to play when opening door
    private int m_HudIndex = -1;    // hud index of the key so we know where to remove it from... Prolly should be done elsewhere....

    /// <summary>
    /// Called when collider (player) enters trigger area
    /// </summary>
    /// <param name="collider">collider that entered</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_CanOpen && collider.gameObject.tag.Equals("Player"))  // Can open the door and player is the collider
        {
            Open(); // Open the door
        }
        else if (!m_CanOpen && collider.gameObject.tag.Equals("Player"))    // If player enters trigger but does not have a key
        {
            int x = Random.Range(1, 4);
            if (x == 1)
            {
                DialogueController.PlayDialog(m_DialogOnTriggerEnter1, 4);   // Play dialog
                m_Dialogue1.Play();
            }
            if (x == 2)
            {
                DialogueController.PlayDialog(m_DialogOnTriggerEnter2, 4);   // Play dialog
                m_Dialogue2.Play();
            }
            if (x == 3)
            {
                DialogueController.PlayDialog(m_DialogOnTriggerEnter3, 4);   // Play dialog
                m_Dialogue3.Play();
            }

            lockedSound.Play(); //play locked door sound
        }
    }

    /// <summary>
    /// Open the motherfucking door
    /// </summary>
    void Open()
    {
        foreach (GameObject go in m_DoorTiles)  // Go through all the tiles
        {
            if (go != null) // Sometimes tile does not exist or has not been assigned...
            {
                // Tile plays animation and has script attached to it so it destroys itself after animation has been finished
                go.GetComponent<SpriteAnimator>().enabled = true;   // Start spriteanimator
                go.SetActive(true); // enable gameobject
                openSound.Play(); //play opening lock sound
            }
        }
        // Remove self colliders, sprites, etc...
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ItemHUDController.RemoveImage(m_HudIndex);  // Remove key from hud
        m_HudIndex = -1;    // Reset hud index
    }

    /// <summary>
    /// Pick up key
    /// </summary>
    /// <param name="hudIndex">index where the key is in HUD</param>
    public void ItemPickedUp(int hudIndex)
    {
        m_CanOpen = true;       // set flag 
        m_HudIndex = hudIndex;  // set index
    }

    /// <summary>
    /// Callback from button indicating it has been pressed
    /// </summary>
    public void ButtonPressed()
    {
        Open();     // Open the door
    }
}
