using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour {

    [SerializeField] private int m_MaxLife = 3;
    [SerializeField] private bool m_ResetOnStart = true;
    private static int m_LifeAmount = 3;
    public int LifeAmount { get { return m_LifeAmount; } }
    public int MaxLifeAmount { get { return m_MaxLife; } }

	void Start () {
        m_LifeAmount = (m_ResetOnStart) ? m_MaxLife : m_LifeAmount;
        PlayerEventHandler.OnDeath += OnDeath;
        PlayerEventHandler.OnLifeRedeemed += OnLifeRedeemed;
        if (m_ResetOnStart)
        {
            ResetLifes();
        }
    }
	
    private void OnDeath()
    {
        m_LifeAmount--;
        LifeHUDController.RemoveLifeImage();
    }

    private void OnLifeRedeemed()
    {
        if (m_LifeAmount < m_MaxLife)
        {
            m_LifeAmount++;
            LifeHUDController.AddLifeImage();
        }
    }
    
    public void AddLife()
    {
        if (m_LifeAmount < m_MaxLife)
        {
            m_LifeAmount++;
            LifeHUDController.AddLifeImage();
        }
    }

    public static void ResetLifes()
    {
        m_LifeAmount = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLifeController>().m_MaxLife;
        LifeHUDController.AddLifeImage();
        LifeHUDController.AddLifeImage();
        LifeHUDController.AddLifeImage();
    }
}
