using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

public class Golem : MonoBehaviour
{
    protected AttackManager attackManager;
    public List<Collider> attackColliders;

    [SerializeField] private GolemLeft m_golemLeft;     // Unity�ŃA�^�b�`�ς�
    [SerializeField] private GolemRight m_golemRight;   // Unity�ŃA�^�b�`�ς�

    [SerializeField] private GameObject m_myself;
    [SerializeField] private GameObject m_target;

    [SerializeField] private TMPro.TMP_Text m_text;

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
            m_golemRight.AttackWait();
            m_golemRight.SetNextAttackId(2);
        }
        if (m_golemRight.AttackWait())
        {
            m_golemLeft.AttackWait();
            m_golemLeft.SetNextAttackId(2);
        }

        if (m_golemLeft.GetStop() && m_golemRight.GetStop())
        {
            m_golemLeft.AttackStart();
            m_golemRight.AttackStart();
        }
    }


    // �^�[�Q�b�g�Ƃ̋������擾���ĕԂ�
    public float DistanceToTarget()
    {
        float dist = 0.0f;

        if (m_target || m_myself)
        {
            Vector3 targetPos = m_target.transform.position;
            Vector3 myPos = m_myself.transform.position;

            dist = Vector3.Distance(targetPos, myPos);
        }

        // �f�o�b�O�p
        if (m_text) m_text.text = "Dist : " + dist.ToString("#.###");

        return dist;
    }


    public int AttackSet(float _dist, int _id = -1)
    {
        int resultId = -1;

        if (_id == -1) resultId = attackManager.Action(_dist);
        else resultId = attackManager.Action(_dist, _id);

        return resultId;
    }


    // �U�����萶��
    private void AttackOn()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = true;
        }
    }


    // �U���������
    private void AttackOff()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
            attackManager.AnimationFin();
        }
    }
}
