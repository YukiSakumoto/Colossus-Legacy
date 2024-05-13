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

public class AttackManager : MonoBehaviour
{
    public class AttackData
    {
        public int id = 0;                  // �U��ID
        public bool isAttack = false;       // �U�����Ă����Ԃ�
        public bool enable = false;         // �U������
        public float coolTime = 0.0f;       // �N�[���^�C��
    }

    private bool m_canAttack = false;     // �U���\��Ԃ��̔���p
    private float m_coolDown = 0.0f;      // ���̍U���܂ł̃N�[���_�E������
    private List<AttackData> m_attackLists = new List<AttackData>();
    
    // �U���ǉ�����   �F   �U�������X�g�ɒǉ�����
    void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // �U����������   �F   ������ ID �̍U���𔭐������ăN�[���_�E�����Ԃ�ݒ�
    void Action(int _attackId = 0)
    {
        int id = SearchAttackId(_attackId);

        m_coolDown = m_attackLists[id].coolTime;
    }


    // ���X�g���̑Ώ�ID��T���āA�v�f����Ԃ�
    int SearchAttackId(int _attackId)
    {
        int result = 0;

        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].id == _attackId)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
