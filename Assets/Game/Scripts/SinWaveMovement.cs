using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls sin wave movement
public class SinWaveMovement : MonoBehaviour
{
    //<summary>
    //establish amplitude and omega on x and y axis
    //</summary>
    public float amplitudeX = .5f;
    public float amplitudeY = .5f;
    public float omegaX = 1.0f;
    public float omegaY = 5.0f;

    public float index; //index float
    public Vector2 startPos;    //starting position of sin wave
    
    void Start()
    {
        startPos = transform.position; //set starting position
    }

    public void Update() //makes the sin wave
    {
        index += Time.deltaTime;
        float x = amplitudeX * Mathf.Cos(omegaX * index);
        float y = Mathf.Sin(amplitudeY * Mathf.Sin(omegaY * index));
        transform.position = startPos + new Vector2(x, y);
    }

}
