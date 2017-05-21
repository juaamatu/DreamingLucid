using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls animation on checkpoints
public class CheckpointAnimator : MonoBehaviour {

    SpriteAnimator animate; //spriteanimator script
    private bool dialoguePlayed = false; //check if dialogue has been played

	void Start ()
    {
        //get component and disable it
        animate = GetComponent<SpriteAnimator>();
        animate.enabled = false;
	}


    void OnTriggerEnter2D(Collider2D collider) //starts animation when the player enters the checkpoint
    {
        if (collider.gameObject.tag.Equals("Player") )
        {
            animate.enabled = true;
            if (!dialoguePlayed)
            {
                DialogueController.PlayDialog("Checkpoint", 4);
                dialoguePlayed = true;
            }
        }
    }
}
