using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen, Juho Turpeinen
// Adds Developer controls to game
public class TeleportPlayer : MonoBehaviour {

    [SerializeField] KeyCode RespawnKey = KeyCode.R;         //Key to reset the curren scene
    [SerializeField] KeyCode ClearCorpsesKey = KeyCode.T;    //Key to clear all corpses in scene
    [SerializeField] KeyCode AddLivesKey = KeyCode.L;        //Key to reset lives
    [SerializeField] KeyCode NextSceneKey = KeyCode.N;       //Key to load the next scene
    public bool developerKeysActive = false; //bool to check if developerkeys are active
    //teleport coordinates
    public float x1;
    public float y1;
    public float x2;
    public float y2;
    public float x3;
    public float y3;
    public float x4;
    public float y4;

    private bool toggle;    //toggle keys active/not active
    private bool activateToggle = false;
    PlayerLifeController m_PlayerLifeController;    //reference to playerlifecontroller script


    void Update () {

        if (developerKeysActive) //checks if developer keys have been activated
        {
            if (Input.GetKey("y") && Input.GetKeyDown("1")) //teleports the player to position 1
            {
                transform.position = new Vector2(x1, y1);
            }
            if (Input.GetKey("y") && Input.GetKeyDown("2")) //teleports the player to position 2
            {
                transform.position = new Vector2(x2, y2);
            }
            if (Input.GetKey("y") && Input.GetKeyDown("3")) //teleports the player to position 3
            {
                transform.position = new Vector2(x3, y3);
            }
            if (Input.GetKey("y") && Input.GetKeyDown("4")) //teleports the player to position 4
            {
                transform.position = new Vector2(x4, y4);
            }
            if (Input.GetKeyDown(RespawnKey)) //reloads the current scene
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                PlayerLifeController.ResetLifes();
            }
            if (Input.GetKeyDown(ClearCorpsesKey)) //clears all corpses on scene
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Corpse");
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObjects[i]);
                }
            }
            if (Input.GetKeyDown(AddLivesKey)) //adds a life to the player
            {
                PlayerLifeController.ResetLifes();
            }
            if (Input.GetKeyDown(NextSceneKey)) //loads the next scene
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab)) //checks if shift and tab have been pressed and toggles developer keys
        {
            toggle = !toggle;
        }
        if (toggle && !activateToggle) //activates developer keys
        {
            Active();
        }
        else if (!toggle && activateToggle) //deactivates developer keys
        {
            Deactive();
        }
    }
    void Active() //plays dialogue for activating developer keys
    {
        developerKeysActive = true;
        DialogueController.PlayDialog("Developer keys activated", 3);
        activateToggle = true;
    }
    void Deactive() //plays dialogue for deactivating developer keys
    {
        developerKeysActive = false;
        DialogueController.PlayDialog("Developer keys deactivated", 3);
        activateToggle = false;
    }
}
