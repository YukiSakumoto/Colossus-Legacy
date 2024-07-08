using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField] SoundPlay m_soundPlay;

    float m_openTime = 0f;
    const float m_openingTime = 0.5f;

    bool m_openFlg = false;
    bool m_openingFlg = false;

    private Animator animator;
    void Start()
    {
        animator= GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetBool("Open", true);
        m_soundPlay.SoundTreasureOpenBegin();
        m_openFlg = true;
    }
    private void Update()
    {
        if (!m_openingFlg)
        {
            if (m_openFlg)
            {
                if (m_openTime < m_openingTime)
                {
                    m_openTime += Time.deltaTime;
                }
                else
                {
                    m_soundPlay.SoundTreasureOpening();
                    m_openingFlg = true;
                }
            }
        }
    }
}
