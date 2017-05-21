using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Petro Pitkänen
// Detects if there are any controllers connected
public class DetectGamepad : MonoBehaviour {

    public static int Controller = 0;   //public int for controller
    public static int Keyboard = 0;     //public int for keyboard


    void Update()
    {
        string[] names = Input.GetJoystickNames();

        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19) //detect PS4 controller
            {
                Controller = 1;
                Keyboard = 0;
            }

            if (names[x].Length == 33) //detect Xbox One controller
            {
                Controller = 1;
                Keyboard = 0;
            }

            if (names[x].Length == 22) //detect USB Snes Gamepad
            {
                Controller = 1;
                Keyboard = 0;
            }

            if (names[x].Length == 0) //detect that no controllers are connected
            {
                Controller = 0;
                Keyboard = 1;
            }

        }

    }
}
