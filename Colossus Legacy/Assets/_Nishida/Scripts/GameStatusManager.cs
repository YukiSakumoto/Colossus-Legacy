using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    [SerializeField] Golem m_golem;
    [SerializeField] GameObject m_character;
    [SerializeField] CharacterManager m_characterManager;

    int m_golemDamage = 0;
    int m_characterDamage = 0;

    bool m_golemDamageFlg = false;

    bool m_characterDamageFlg = false;
    bool m_characterKnockBackFlg = false;


    enum PlayerDamage // ��l�����󂯂�_���[�W��
    {
        small = 30,  // ���_���[�W
        medium = 50, // ���_���[�W
        big = 80,    // ��_���[�W
        death = 100  // �����U��
    }

    enum GolemDamage // �S�[�������󂯂�_���[�W��
    {
        Sword = 50, // ���_���[�W
        Arrow = 20, // ��_���[�W
        Bomb = 50,  // ���e�_���[�W
    }

    enum Recovery // �񕜗�
    {
        small = 20,  // ����
        medium = 50, // ����
        big = 70,    // ���
        full = 100   // ���S��
    }

    void Start()
    {
        if(!m_golem)
        {
            Debug.Log("GameStatusManager: golem is Null");
        }

        if (!m_character)
        {
            Debug.Log("GameStatusManager: character is Null");
        }

        if (!m_characterManager)
        {
            Debug.Log("GameStatusManager: characterManger is Null");
        }

    }

    void Update()
    {
        if(m_golemDamageFlg)
        {
            m_golem.SetHit(m_golemDamage);
            m_golemDamageFlg = false;
            m_golemDamage = 0;
        }
        
        if(m_characterDamageFlg)
        {
            m_characterManager.SetHit(m_characterDamage,m_characterKnockBackFlg);
            m_characterDamageFlg = false;
            m_characterDamage = 0;
        }
    }

    public void DamageGolemSword()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Sword;
        Debug.Log("GameStatusManager: DamageGolemSword");
    }
    public void DamageGolemArrow()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Arrow;
        Debug.Log("GameStatusManager: DamageGolemArrow");
    }

    public void DamageGolemBomb()
    {
        m_golemDamageFlg = true;
        m_golemDamage = (int)GolemDamage.Bomb;
        Debug.Log("GameStatusManager: DamageGolemBomb");
    }

    public void DamagePlayerPushUP() // ������˂��グ��U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDamage = (int)PlayerDamage.small;
        Debug.Log("GameStatusManager: DamagePlayerPushUP");
    }

    public void DamagePlayerDown() // �ォ�牟���ׂ��n�̍U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDamage = (int)PlayerDamage.medium;
        Debug.Log("GameStatusManager: DamagePlayerDown");
    }

    public void DamagePlayerPressHand() // �����U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = false;
        m_characterDamage = (int)PlayerDamage.big;
        Debug.Log("GameStatusManager: DamagePlayerPressHand");
    }

    public void DamagePlayerBeam() // �r�[���U��
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDamage = (int)PlayerDamage.death;
        Debug.Log("GameStatusManager: DamagePlayerBeam");
    }

    public void DamagePlayerBomb() // ���e�U��(����)
    {
        m_characterDamageFlg = true;
        m_characterKnockBackFlg = true;
        m_characterDamage = (int)PlayerDamage.medium;
        Debug.Log("GameStatusManager: DamagePlayerBomb");
    }
}
