using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightConeController : MonoBehaviour {

    public Vector3[] waypoints;
    public float speed = 1;
    float accumulatedTime = 0;

	void Start () {
	}
	
	void Update () {
        accumulatedTime += Time.deltaTime * speed;
        transform.position = Vector2.Lerp(waypoints[0], waypoints[1], accumulatedTime);
        if (accumulatedTime >= 1)
        {
            accumulatedTime = 0;
            Array.Reverse(waypoints);
        }
	}
}
