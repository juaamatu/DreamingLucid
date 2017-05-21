using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Petro Pitkänen
// Controls reverb zone
public class ReverbZoneController : MonoBehaviour {

    Collider2D m_Collider;      //collider of the zone
    public AudioMixer m_Mixer;  //audiomixer reference
    public bool inTrigger = false; //check if the player is in the trigger
    public float m_ReverbAmount = 0f; //reverb amount

	void Start () {

        m_Collider = GetComponent<Collider2D>(); //get component

	}

    void OnTriggerEnter2D(Collider2D collider) //enable reverb if the player is in the trigger
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            EnableReverb();
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) //disable reverb if the player leaves the trigger
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            DisableReverb();
            inTrigger = false;
        }
    }

    void EnableReverb() //enable reverb
    {
        m_Mixer.SetFloat("reverb", m_ReverbAmount);
    }

    void DisableReverb() //disable reverb
    {
        m_Mixer.SetFloat("reverb", -10000f);
    }
}
