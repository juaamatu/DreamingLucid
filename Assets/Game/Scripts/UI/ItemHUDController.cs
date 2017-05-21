using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUDController : MonoBehaviour {

    private static ItemHUDController m_Controller;
    private Image[] m_ItemImages;

    void Awake()
    {
        if (m_Controller == null)
        {
            m_Controller = this;
        }
        else
        {
            Destroy(this);
        }
    }

	void Start () {
        m_ItemImages = GetComponentsInChildren<Image>();
        foreach (Image image in m_ItemImages)
        {
            image.gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Adds image to screen and returns index which image occupies
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns>index for image</returns>
    public static int AddImage(Sprite sprite)
    {
        for (int i = 0; i < m_Controller.m_ItemImages.Length; i++)
        {
            if (!m_Controller.m_ItemImages[i].gameObject.activeInHierarchy)
            {
                m_Controller.m_ItemImages[i].sprite = sprite;
                m_Controller.m_ItemImages[i].gameObject.SetActive(true);
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Removes image from screen
    /// </summary>
    /// <param name="index">image index</param>
    public static void RemoveImage(int index)
    {
        if (index >= 0 && index < m_Controller.m_ItemImages.Length)
        {
            m_Controller.m_ItemImages[index].gameObject.SetActive(false);
        }
    }
}
