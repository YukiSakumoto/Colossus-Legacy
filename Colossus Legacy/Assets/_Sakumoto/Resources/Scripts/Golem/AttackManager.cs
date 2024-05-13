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
    // �U���̃f�[�^�n
    public class AttackData
    {
        public int id { get; set; }             // �U��ID
        public bool enable { get; set; }        // �U�����������Ă��邩
        public bool isAttack { get; set; }      // �U�����肪���邩
        public float coolTime { get; set; }     // �N�[���^�C��
    }
    public AttackData intstance { get; private set; }

    private bool m_canAttack = false;     // �U���\��Ԃ��̔���p
    private float m_coolDown = 0.0f;      // ���̍U���܂ł̃N�[���_�E������
    private List<AttackData> m_attackLists = new List<AttackData>();


    
    // �U���ǉ�����   �F   �U�������X�g�ɒǉ�����
    public void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // �U����������   �F   ������ ID �̍U���𔭐������ăN�[���_�E�����Ԃ�ݒ�
    public void Action(int _attackId = 0)
    {
        int id = SearchAttackId(_attackId);

        m_attackLists[id].enable = true;
        m_coolDown = m_attackLists[id].coolTime;
    }


    // ���X�g���������ID��T���āA�C���f�b�N�X�ԍ���Ԃ�
    private int SearchAttackId(int _attackId)
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


    // AttackManager �̃C���X�^���X��������
    private void Awake()
    {
        intstance = new AttackData();
    }
}
