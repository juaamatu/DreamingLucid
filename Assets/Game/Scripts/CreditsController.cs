using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls the movement of end credits
public class CreditsController : MonoBehaviour {

    public Transform endGoal;   //goal for the credits
    public Transform startGoal; //starting position

    void Update()
    {
         transform.position = Vector3.MoveTowards(startGoal.position, endGoal.position, .002f); //move the credits towards the goal position
    }

}
