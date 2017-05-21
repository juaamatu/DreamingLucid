using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Juho Turpeinen
// Controls sparks that consists of two particle system.
// One is only spark particles and other light area behind them
public class SparkParticlesController : MonoBehaviour {

    [SerializeField] private float m_MinForce = 50;             // Min force for emit
    [SerializeField] private float m_MaxForce = 200;            // Max force for emit
    [SerializeField] private float m_MinAliveTime = 1;          // Min life time
    [SerializeField] private float m_MaxAliveTime = 3;          // Max life time
    [SerializeField] private Sprite[] m_SparkSprites;           // All spark sprites
    [SerializeField] private Sprite[] m_SparkLightSprites;      // All spark light sprites
    [SerializeField] private SpriteRenderer m_Spark;            // Spark sprite renderer
    [SerializeField] private SpriteRenderer m_SparkLight;       // Spark light area renderer
    private Rigidbody2D m_Rigidbody;                            // Rigidbody for spark
    private float m_AccumulatedTime = 0;                        // Timer for alive time
    private float m_Force;                                      // Force between min and max
    private float m_AliveTime;                                  // Time between min and max alive time
    private Vector3 m_StartScale;                               // Start scale of spark light area

	private void Awake () {
        // get references
        m_Rigidbody = GetComponent<Rigidbody2D>();
        // choose random sprites for renderers
        m_Spark.sprite = m_SparkSprites[Random.Range(0, m_SparkSprites.Length)];
        m_SparkLight.sprite = m_SparkLightSprites[Random.Range(0, m_SparkLightSprites.Length)];
        m_Force = Random.Range(m_MinForce, m_MaxForce + 1); // force to add
        m_AliveTime = Random.Range(m_MinAliveTime, m_MaxAliveTime + 1); // how long is spark alive
        m_StartScale = m_SparkLight.transform.localScale; // start scale of light area
    }

    private void Start()
    {
        // apply start force
        m_Rigidbody.AddForce(new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(0, 2)) * m_Force);
    }

    private void Update()
    {
        m_AccumulatedTime += Time.deltaTime;
        // lerp light scale over time
        m_SparkLight.transform.localScale = Vector3.Lerp(m_StartScale, Vector3.zero, m_AliveTime - (m_AliveTime / m_AccumulatedTime));
        if (m_AccumulatedTime >= m_AliveTime) // destroy spark when it has lived its time
        {
            Destroy(gameObject);
        }
    }
}
