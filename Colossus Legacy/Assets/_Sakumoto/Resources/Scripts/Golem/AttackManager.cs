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
        public int m_id { get; set; }             // �U��ID
        public bool m_enable { get; set; }        // �U�����[�V���������Ă��邩
        public bool m_isAttack { get; set; }      // �U������o����Ԃ�
        public float m_coolTime { get; set; }     // �N�[���^�C��
        public float m_startTime {  get; set; }   // �U������J�n�t���[��
        public float m_endTime { get; set; }      // �U������I���t���[��
    }
    public AttackData m_instance { get; set; }

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

        m_coolDown = m_attackLists[id].m_coolTime;
    }


    // ���X�g���̑Ώ�ID��T���āA�C���f�b�N�X�ԍ���Ԃ�
    int SearchAttackId(int _attackId)
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


    // �C���X�^���X��������
    private void Awake()
    {
        m_instance = new AttackData();   
    }
}
