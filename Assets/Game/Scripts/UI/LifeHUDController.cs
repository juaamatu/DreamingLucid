using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeHUDController : MonoBehaviour {

    static LifeHUDController controller;
    Image[] m_LifeImages;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else
        {
            Destroy(this);
        }
    }

	void Start ()
    {
        m_LifeImages = GetComponentsInChildren<Image>();
        PlayerLifeController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLifeController>();
        for (int i = controller.LifeAmount; i < controller.MaxLifeAmount; i++)
        {
            RemoveLifeImage();
        }
	}

    public static void AddLifeImage()
    {
        if (controller == null)
        {
            Debug.LogWarning("Tried to add life when LifeHUDCOntroller singleton is still null");
            return;
        }

        for (int i = controller.m_LifeImages.Length - 1; i > -1; i--)
        {
            if (!controller.m_LifeImages[i].enabled)
            {
                controller.m_LifeImages[i].enabled = true;
                break;
            }
        }
    }

    public static void RemoveLifeImage()
    {
        for (int i = 0; i < controller.m_LifeImages.Length; i++)
        {
            if (controller.m_LifeImages[i].enabled)
            {
                controller.m_LifeImages[i].enabled = false;
                break;
            }
        }
    }
}
