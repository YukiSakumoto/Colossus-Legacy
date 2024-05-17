using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Golem : MonoBehaviour
{
    protected AttackManager attackManager;

    [SerializeField] private GolemLeft m_golemLeft;     // Unity�ŃA�^�b�`�ς�
    [SerializeField] private GolemRight m_golemRight;   // Unity�ŃA�^�b�`�ς�

    public bool m_stop = false;     // ���r�𓯎��ɍ��킹�邽�߂̃t���O

    public int hp = 100;

    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        m_golemLeft = GameObject.Find("Golem_Left").GetComponent<GolemLeft>();
        m_golemRight = GameObject.Find("Golem_Right").GetComponent<GolemRight>();
    }

    void Update()
    {
        // Null �`�F�b�N
        if (!m_golemLeft || !m_golemRight)
        {
            if (!m_golemLeft) { Debug.Log("LNull!!"); }
            if (!m_golemRight) { Debug.Log("RNull!!"); }
            
            return;
        }

        if (m_golemLeft.AttackWait())
        {
            m_golemRight.AttackSet(2);
        }
        m_golemRight.AttackWait();
        {
            m_golemLeft.AttackSet(2);
        }

        if (m_golemLeft.GetStop() && m_golemRight.GetStop())
        {
            Debug.Log("OK!");
            m_golemLeft.AttackStart();
            m_golemRight.AttackStart();
        }
    }
}
