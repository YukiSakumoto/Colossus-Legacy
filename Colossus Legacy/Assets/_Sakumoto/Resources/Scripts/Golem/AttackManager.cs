using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


// -----------------------------
// AttackManager : �g����
// 1. �g�������X�N���v�g��AttackManager���Ăяo��
// 2. AddAttack() �ɒǉ��������U���� ID �������œn��
// 3. 
// -----------------------------

public struct AttackData
{
    public int m_id { get; set; }               // �U��ID
    public string m_name { get; set; }          // �U�����i�A�j���[�V�����̖��O�j
    public float m_dist { get; set; }           // �U���҂��N�_�Ƃ����U���̔�������
    public float m_coolDown { get; set; }       // �U����̍d������
}


public class AttackManager : MonoBehaviour
{
    public Animator m_animator;
    private bool m_isAttackAnimation = false;

    private int m_nowId = -1;
    private bool m_canAttack = false;   // �U���\��Ԃ�
    private float m_coolDown = 0.0f;                // ���̍U���܂ł̃N�[���_�E�����ԁi�b���j
    private List<AttackData> m_attackLists = new List<AttackData>();


    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    void Update()
    {
        // �A�j���[�V�������Ȃ�X�L�b�v
        if (m_isAttackAnimation) { return; }

        // �N�[���_�E�����Ԃ�����
        m_coolDown�@-= Time.deltaTime;
        if (m_coolDown < 0.0f)
        {
            m_coolDown = 0.0f;

            if (m_nowId == -1)
            {
                m_canAttack = true;
            }
        }
    }


    // �U���ǉ�����   �F   �U�������X�g�ɒǉ�����
    public void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // �U����������   ��   �����F�v���C���[�Ƃ̋���
    public int Action(float _dist)
    {
        if (!m_canAttack) { return -1; }      // �N�[���_�E�����܂��Ȃ瑁�����^�[��

        // �����\�ȍU�����擾
        List<int> attacks = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].m_dist >= _dist)
            {
                attacks.Add(m_attackLists[i].m_id);
            }
        }
        if (attacks.Count == 0) { Debug.Log("�����O"); return -1; }  // �������ɍU���Ώۂ����Ȃ������瑁�����^�[��

        // �擾�����ꗗ���烉���_���ōU��
        int randIndex = Random.Range(0, attacks.Count);
        m_nowId = m_attackLists[randIndex].m_id;
        m_coolDown = m_attackLists[randIndex].m_coolDown;

        m_isAttackAnimation = true;
        m_animator.SetBool(m_attackLists[m_nowId].m_name, m_isAttackAnimation);

        m_canAttack = false;

        return m_nowId;
    }


    // �A�j���[�V�����̏I���i���X�N���v�g�̃A�j���[�V��������Ăяo���j
    public void AnimationFin()
    {
        if (m_nowId < 0) { return; }
        m_isAttackAnimation = false;
        m_animator.SetBool(m_attackLists[m_nowId].m_name, m_isAttackAnimation);
        m_nowId = -1;
    }


    // ���X�g���̑Ώ�ID��T���āA�C���f�b�N�X�ԍ���Ԃ�
    private int SearchAttackId(int _attackId)
    {
        int result = 0;

        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].m_id == _attackId)
            {
                result = i;
                break;
            }
        }

        return result;
    }


    // _delta : true �Ōo�ߎ��Ԏ擾�@false �Ŏc�莞�Ԏ擾
    public float GetCoolDown(bool _delta = true)
    {
        if (_delta)
        {
            return m_attackLists[m_nowId].m_coolDown - m_coolDown;
        }
        else
        {
            return m_coolDown;
        }
    }


    //// �C���X�^���X��������
    //private void Awake()
    //{
    //    m_instance = new AttackData();   
    //}
}
