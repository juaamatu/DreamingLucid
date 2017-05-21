using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls showing different instructions depending if a controller is connected or not
public class ButtonInstructionController : MonoBehaviour {

    public GameObject m_Keyboard;       //keyboard instructions
    public GameObject m_Controller;     //controller instructions

	void Update () {
		
        if (DetectGamepad.Keyboard == 1 && DetectGamepad.Controller == 0) //detect if controller is not connected
        {
            m_Keyboard.SetActive(true);     //change to keyboard instructions
            m_Controller.SetActive(false);
        }
        if (DetectGamepad.Controller == 1 && DetectGamepad.Keyboard == 0)   //detect if controller is connected
        {
            m_Keyboard.SetActive(false);    //change to controller instructions
            m_Controller.SetActive(true);
        }

	}
}
