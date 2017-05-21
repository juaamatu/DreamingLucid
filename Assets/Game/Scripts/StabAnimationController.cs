using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Petro Pitkänen
// Controls the stabbing victim animations in storyroom
public class StabAnimationController : MonoBehaviour {

    //<summary>
    //get spriteanimator script and spriterenderer references
    //</summary>
    public SpriteAnimator Idle;
    public SpriteAnimator Stab;
    public SpriteAnimator Death;
    SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer idleRenderer;
    public SpriteRenderer stabRenderer;
    public SpriteRenderer deathRenderer;

	void Start () {

        //<summary>
        //get component and enable/disable components
        //</summary>
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Idle.enabled = false;
        Stab.enabled = false;
        Death.enabled = false;
        m_SpriteRenderer.enabled = true;
        idleRenderer.enabled = true;
        stabRenderer.enabled = false;
        deathRenderer.enabled = false;
	}
	
    public void StabEnabled() //start first stabbing animation
    {
        StartCoroutine(WaitFrame());
        Stab.enabled = true;
        Idle.enabled = false;
        Death.enabled = false;
        idleRenderer.enabled = false;
        stabRenderer.enabled = true;
        deathRenderer.enabled = false;
    }

    public void DeathEnabled() //start dying animation
    {
        StartCoroutine(WaitFrameAgain());
        Death.enabled = true;
        Idle.enabled = false;
        Stab.enabled = false;
        m_SpriteRenderer.enabled = false;
        idleRenderer.enabled = false;
        deathRenderer.enabled = true;
    }

    IEnumerator WaitFrame() //wait for a few frames before continuing
    {
        yield return new WaitForSeconds(.2f);
        m_SpriteRenderer.enabled = false;
    }
    IEnumerator WaitFrameAgain() //wait for a few frames before continuing
    {
        yield return new WaitForSeconds(.1f);
        stabRenderer.enabled = false;
    }

}
