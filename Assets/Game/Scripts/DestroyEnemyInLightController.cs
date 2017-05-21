using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen, Juho Turpeinen
// Controls destroying enemy when colliding with light
public class DestroyEnemyInLightController : MonoBehaviour
{
    public bool collided = false;           //check if is colliding with light
    [SerializeField] private Sprite[] m_Sprites;    //animation sprites
    [SerializeField] private float m_FrameTime = 0.1f;  //animation speed
    [SerializeField] private bool m_PlayOnce = false;   //bool to check that animation plays only once
    [SerializeField] private bool m_DeleteOnEnd = false;    //bool to destroy object after animation is done
    SpriteRenderer m_SpriteRenderer;    //reference to sprite renderer
    float m_AccumulatedTime = 0;        //float for animator
    int m_CurrentIndex = 0;             //float for animator
    SpriteAnimator m_Animator;          //reference to sprite animator script
    public DestroyEnemyInLightController m_Trigger; //trigger script in child object to start
    public GameObject m_Spike;          //reference to spike object
    AudioSource m_Audio;                //audiosource for dying sound
    private bool soundPlayed = false;   //bool to check if sound has played

    void Start()
    {  
        //<summary>
        //get components
        //</summary>
        m_Animator = GetComponent<SpriteAnimator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider) //check for collision with light
    {
        if (collider.gameObject.tag.Equals("Light"))
        {
            StartCoroutine(WaitSome());
           
        }
    }

    IEnumerator WaitSome()  //wait for the end of the frame to start the animation
    {
        yield return new WaitForEndOfFrame();
            collided = true;
    }

    void Update()
    {
        if (collided)
        {
            if (m_Animator != null) //enable animation
            {
                m_Animator.enabled = false;
            }
            if (m_Trigger != null)  //enable trigger for script in child
            {
                m_Trigger.Trigger();
            }
            if (m_Spike != null)    //destroy the spike object
            {
                Destroy(m_Spike);
            }
            //copy-paste from the SpriteAnimationController script
            if (m_Sprites.Length == 0)  //start animation
                return;
            m_AccumulatedTime += Time.deltaTime;
            if (m_AccumulatedTime >= m_FrameTime)
            {
                if (m_CurrentIndex == m_Sprites.Length - 1 && (m_PlayOnce || m_DeleteOnEnd))
                {
                    enabled = false;
                    if (m_DeleteOnEnd)
                        Destroy(gameObject);
                    return;
                }
                m_CurrentIndex = (m_CurrentIndex != m_Sprites.Length - 1) ? m_CurrentIndex + 1 : 0;
                m_SpriteRenderer.sprite = m_Sprites[m_CurrentIndex];
                m_AccumulatedTime = 0;
            }
            if (!m_Audio.isPlaying && !soundPlayed) //play death sound
            {
                m_Audio.Play();
                soundPlayed = true;
            }
        }
    }
    
    public void Trigger() //trigger to start
    {
        collided = true;
    }
}
