using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesController : MonoBehaviour {

    [SerializeField] ParticleSystem m_DeathParticles;

    public void PlayBlood()
    {
        Instantiate(m_DeathParticles, transform.position, Quaternion.identity);
    }
}
