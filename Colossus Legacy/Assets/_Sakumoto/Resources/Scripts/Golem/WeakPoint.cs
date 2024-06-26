using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    GameStatusManager m_gameManager;
    public bool m_weakHit { get; set; }


    private void Start()
    {
        m_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameStatusManager>();
        if (!m_gameManager) { Debug.LogError("ゲームマネージャーが見つかりません"); }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SwordAttack"))
        {
            m_gameManager.DamageGolemSword();
            m_weakHit = true;
        }
        else if (other.gameObject.CompareTag("ArrowAttack"))
        {
            m_gameManager.DamageGolemArrow();
            m_weakHit = true;
        }
        else if (other.gameObject.CompareTag("BombAttack"))
        {
            m_gameManager.DamageGolemBomb();
            m_weakHit = true;
        }
    }
}
