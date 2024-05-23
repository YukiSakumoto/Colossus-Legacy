using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public struct AttackData
{
    public int m_id { get; set; }               // �U��ID
    public string m_name { get; set; }          // �U�����i�A�j���[�V�����̖��O�j
    public float m_dist { get; set; }           // �U���҂��N�_�Ƃ����U���̔�������
    public float m_coolDown { get; set; }       // �U����̍d������
    public bool m_waitFlg { get; set; }         // �ҋ@�t���O�i���̍U����҂��ԂȂǂɎg�p�j
}


public class AttackManager : MonoBehaviour
{
    public Animator m_animator;
    private bool m_isAttackAnimation = false;

    private int m_nowId = -1;
    private bool m_canAttack = false;   // �U���\��Ԃ�
    [SerializeField] private float m_coolDown = 0.0f;    // ���̍U���܂ł̃N�[���_�E�����ԁi�b���j
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
        m_coolDown -= Time.deltaTime;
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
    public void AddAttack(int _id, string _name, float _dist, float _coolDown, bool _flg = false)
    {
        AttackData attack = new AttackData();
        attack.m_id = _id;
        attack.m_name = _name;
        attack.m_dist = _dist;
        attack.m_coolDown = _coolDown;
        attack.m_waitFlg = _flg;
        AddAttack(attack);
    }


    // �U����������   ��   �����F�v���C���[�Ƃ̋���
    public int Action(float _dist)
    {
        // �ҋ@��ԂȂ猻�݂̍U��ID��Ԃ��ă��^�[��
        if (m_attackLists[SearchAttackId(m_nowId)].m_waitFlg && m_nowId != -1) { return m_nowId; }

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
        if (attacks.Count == 0) { Debug.Log("�U���s��"); return -1; }  // �������ɍU���Ώۂ����Ȃ������瑁�����^�[��


        // �擾�����ꗗ���烉���_���ōU��
        int randIndex = Random.Range(0, attacks.Count);
        m_nowId = attacks[randIndex];
        m_coolDown = m_attackLists[SearchAttackId(m_nowId)].m_coolDown;

        m_canAttack = false;


        if (!m_attackLists[SearchAttackId(m_nowId)].m_waitFlg)
        {
            m_isAttackAnimation = true;
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
        }

        return m_nowId;
    }

    public int Action(float _dist, int _id)
    {

        // �ҋ@��ԂȂ猻�݂̍U��ID��Ԃ��ă��^�[��
        if (m_attackLists[SearchAttackId(m_nowId)].m_waitFlg) { return m_nowId; }

        // �N�[���_�E�����܂��Ȃ瑁�����^�[��
        if (!m_canAttack) { return -1; }

        // �w�肵���U�����͈͊O�Ȃ烊�^�[��
        if (m_attackLists[SearchAttackId(m_nowId)].m_dist < _dist) { return -1; }

        m_nowId = _id;
        m_coolDown = m_attackLists[SearchAttackId(m_nowId)].m_coolDown;
        m_canAttack = false;

        // �ҋ@��Ԃ��K�v�łȂ��Ȃ炻�̂܂܃A�j���[�V�������Đ�
        if (!m_attackLists[SearchAttackId(m_nowId)].m_waitFlg)
        {
            m_isAttackAnimation = true;
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
        }

        return m_nowId;
    }


    // �A�j���[�V�������X�^�[�g����
    public void AttackStart()
    {
        m_isAttackAnimation = true;
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
    }


    // �A�j���[�V�����̏I���i���X�N���v�g�̃A�j���[�V��������Ăяo���j
    public void AnimationFin()
    {
        if (m_nowId < 0) { return; }
        m_isAttackAnimation = false;
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
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
            return m_attackLists[SearchAttackId(m_nowId)].m_coolDown - m_coolDown;
        }
        else
        {
            return m_coolDown;
        }
    }
}
