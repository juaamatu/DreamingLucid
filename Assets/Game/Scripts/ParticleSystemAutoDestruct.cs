using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemAutoDestruct : MonoBehaviour {
	void Start () {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
	}
}