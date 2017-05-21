using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls the movement of eyes on Spike Enemy
public class EnemyEyeMovementController : MonoBehaviour {

    Transform m_PlayerTransform;        //player transform 
    public Transform m_ParentTransform; //transform of the parent object
    public float minX;  //min X, where eyes can move
    public float maxX;  //max X, -||-
    public float minY;  //min Y, -||-
    public float maxY;  //max Y, -||-
    public float speed; //how fast the eyes can move
    private Vector3 m_velocity; //smoothdamp velocity
    public float m_SmoothTime = 0.3f;   //smoothness of the movement
    public bool m_PlayerIsNear;     //check if player is near
    public float m_AggroDistance;   //how far away will the eyes start to track

    void Start()
    {
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform; //find the player transform
    }

    void Update()
    {
        m_PlayerIsNear = ((m_PlayerTransform.position - m_ParentTransform.position).magnitude <= m_AggroDistance); //check when player is in range
        if (m_PlayerIsNear) //start the movement of the eyes, if the player is near
        {
            Vector3 targetPosition = m_PlayerTransform.position;    //target position the eyes are trying to go
            targetPosition.x = Mathf.Clamp(targetPosition.x, m_ParentTransform.position.x + minX, m_ParentTransform.position.x + maxX); //clamp the movement on x-axis
            targetPosition.y = Mathf.Clamp(targetPosition.y, m_ParentTransform.position.y + minY, m_ParentTransform.position.y + maxY); //clamp the movement on y-axis
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, m_SmoothTime); //move the eyes towards the player
        }
        if (!m_PlayerIsNear) //move the eyes back to the center if the player is not near 
        {
            transform.position = Vector3.SmoothDamp(transform.position, m_ParentTransform.position, ref m_velocity, m_SmoothTime); //move eyes to center
        }
    }
}
