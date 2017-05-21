using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Juho Turpeinen
// Controls emitter for sparks
public class SparkSourceController : MonoBehaviour {

    [SerializeField] private GameObject m_Spark;        // Spark prefab
    [SerializeField] private float m_SpawnFreq = 1;     // Spawn frequency
    private float m_AccumulatedTime = 0;                // Total accumulated time this object has been active
	
	private void Update () {
        m_AccumulatedTime += Time.deltaTime;
        if (m_AccumulatedTime > m_SpawnFreq) // Spawn spark if timer is over spawn freq
        {
            GameObject spark = Instantiate(m_Spark, transform.position, transform.rotation);
            m_AccumulatedTime = 0;
        }
	}
}
