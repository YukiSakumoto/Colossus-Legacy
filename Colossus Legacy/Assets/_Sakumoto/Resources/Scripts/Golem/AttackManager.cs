using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public struct AttackData
{
    public int m_id { get; set; }               // �U��ID
    public string m_name { get; set; }          // �U�����i�A�j���[�V�����̖��O�j
    public Vector2 m_dist { get; set; }         // �U���҂��N�_�Ƃ����U���̔������� (x : ���m�Œ�����   y : ���m�ŒZ����)
    public float m_coolDown { get; set; }       // �U����̍d������
    public bool m_waitFlg { get; set; }         // �ҋ@�t���O�i���̍U����҂��ԂȂǂɎg�p�j
}


public class AttackManager : MonoBehaviour
{
    public Animator m_animator;
    private bool m_isAttackAnimation = false;

    [SerializeField] private int m_nowId = -1;
    [SerializeField] private bool m_canAttack = false;   // �U���\��Ԃ�
    [SerializeField] private float m_coolDown = 0.0f;    // ���̍U���܂ł̃N�[���_�E�����ԁi�b���j
    [SerializeField] private List<AttackData> m_attackLists = new List<AttackData>();


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
    public void AddAttack(int _id, string _name, Vector2 _dist, float _coolDown, bool _flg = false)
    {
        AttackData attack = new();
        attack.m_id = _id;
        attack.m_name = _name;
        attack.m_coolDown = _coolDown;
        attack.m_waitFlg = _flg;

        // �ǉ����ꂽ Vector2 �̋������~���ɕ��ёւ�
        attack.m_dist = _dist;
        if (attack.m_dist.x < attack.m_dist.y)
        {
            Vector2 tmp = new(attack.m_dist.y, attack.m_dist.x);
            attack.m_dist = tmp;
        }

        AddAttack(attack);
    }


    // �U���̍폜����
    public void DeleteAttack(int _id)
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            AttackData attack = m_attackLists[i];
            if (attack.m_id == _id)
            {
                int idx = m_attackLists[i].m_id;
                m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
                m_attackLists.Remove(attack);
            }
        }
    }


    // �U���̍폜����
    public void DeleteAttack(string _name)
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            AttackData attack = m_attackLists[i];
            if (attack.m_name == _name)
            {
                int idx = m_attackLists[i].m_id;
                m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
                m_attackLists.Remove(attack);
            }
        }
    }


    public void DeleteAll()
    {
        ResetAnimation();
        m_attackLists.Clear();
    }


    // �U����������   ��   �����F�v���C���[�Ƃ̋���
    public int Action(float _dist)
    {
        // �ҋ@��ԂȂ猻�݂̍U��ID��Ԃ��ă��^�[��
        if (m_attackLists[SearchAttackId(m_nowId)].m_waitFlg && m_nowId != -1) { return m_nowId; }

        if (!m_canAttack) { return m_nowId; }      // �N�[���_�E�����܂��Ȃ瑁�����^�[��

        // �����\�ȍU�����擾
        List<int> attacks = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            // ���m�Œ� > �^�[�Q�b�g�Ƃ̋��� > ���m�ŒZ �Ȃ�
            if (m_attackLists[i].m_dist.x >= _dist && m_attackLists[i].m_dist.y <= _dist)
            {
                attacks.Add(m_attackLists[i].m_id);
            }
        }
        if (attacks.Count == 0) { return -1; }  // �������ɍU���Ώۂ����Ȃ������瑁�����^�[��


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
        //if (!_startFlg) { return m_nowId; }

        // �N�[���_�E�����܂��Ȃ瑁�����^�[��
        if (!m_canAttack) { return m_nowId; }

        // �w�肵���U�����͈͊O�Ȃ烊�^�[��
        if (m_attackLists[SearchAttackId(_id)].m_dist.x < _dist ||
            m_attackLists[SearchAttackId(_id)].m_dist.y > _dist) { Debug.Log("���ɂ�ɂ�"); return -1; }

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
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, true);
    }


    // �A�j���[�V�����̏I���i���X�N���v�g�̃A�j���[�V��������Ăяo���j
    public void AnimationFin()
    {
        if (m_nowId < -1) { return; }
        m_isAttackAnimation = false;
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, false);
        m_nowId = -1;
    }


    // ���݂̃A�j���[�V�����𒆒f���ĕʂ̃A�j���[�V�����̍Đ�
    public void ChangeAnimation(string _animationName, bool _flg, int _id = -1)
    {
        if (m_nowId != -1)
        {
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, false);
        }
        m_animator.SetBool(_animationName, _flg);

        m_nowId = _id;
        if (m_nowId == -1)
        {
            m_isAttackAnimation = false;
        }
    }


    public void ResetAttackAnimation()
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            int idx = m_attackLists[i].m_id;
            m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
        }
        m_nowId = -1;
        m_isAttackAnimation = false;
    }


    // �S�A�j���[�V�������~
    public void ResetAnimation()
    {
        ResetAttackAnimation();
        m_animator.SetBool("Damage", false);
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


    public string GetAttackName()
    {
        if (m_nowId != -1)
        {
            return m_attackLists[SearchAttackId(m_nowId)].m_name;
        }
        return "";
    }

    public string GetAttackName(int _id)
    {
        return m_attackLists[SearchAttackId(_id)].m_name;
    }


    public bool IsAttackRange(int _id, float _targetDist)
    {
        if (m_attackLists[SearchAttackId(_id)].m_dist.x < _targetDist ||
            m_attackLists[SearchAttackId(_id)].m_dist.y > _targetDist) { return false; }

        return true;
    }


    public List<int> GetAttackIdList()
    {
        List<int> idList = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            idList.Add(m_attackLists[SearchAttackId(i)].m_id);
        }

        return idList;
    }


    public Vector2 GetAttackDist(int _id)
    {
        return m_attackLists[SearchAttackId(_id)].m_dist;
    }



    public void SetAttackSpeed(float _val)
    {
        m_animator.SetFloat("AttackSpeed", _val);
    }
}
