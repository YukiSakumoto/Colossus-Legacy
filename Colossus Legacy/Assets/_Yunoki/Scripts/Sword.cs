using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    //private CharacterMovement characterMovement;

    [SerializeField] private string m_targetWeakTag = "EnemyWeak";
    [SerializeField] private string m_targetBodyTag = "Enemy";

    [SerializeField] private MeshCollider m_meshCollider;

    [SerializeField] CharacterManager m_manager;
    [SerializeField] GameStatusManager m_gameStatusManager;

    private bool m_hitFlg = false;
    private bool m_deflectedFlg = false;
    private bool m_judgeEndFlg = false;

    void Start()
    {
        // CharacterMovement������GameObject������
        GameObject characterObject = GameObject.Find("HumanMale_Character");

        // CharacterMovement���A�^�b�`����Ă���GameObject����CharacterMovement�R���|�[�l���g���擾
        //characterMovement = characterObject.GetComponent<CharacterMovement>();

        m_meshCollider.enabled = false;

        if (!m_manager)
        {
            Debug.Log("Sword:manager is Null");
        }

        if (!m_gameStatusManager)
        {
            Debug.Log("Sword:GameStatusManager is Null");
        }
    }

    private void Update()
    {
        if (m_manager.Getm_swordMoveFlg || m_manager.Getm_secondSwordAttackFlg)
        {
            SwordAttackOn();
        }
        else
        {
            SwordAttackOff();
            m_judgeEndFlg = false;
        }
    }

    void LateUpdate()
    {
        m_hitFlg = false;
        m_deflectedFlg = false;
    }

    void OnTriggerEnter(Collider _other)
    {
        if (!m_judgeEndFlg)
        {
            if (!m_hitFlg && !m_deflectedFlg)
            {
                // �U�����[�V�������̂ݔ�����s���悤�ɂ���
                if (_other.gameObject.CompareTag(m_targetWeakTag))
                {
                    m_gameStatusManager.DamageGolemSword();
                    //Destroy(_other.gameObject);
                    Debug.Log("�q�b�g�I�����G�̎�_�ɓ�����������");
                    m_hitFlg = true;
                    SwordAttackOff();
                    m_judgeEndFlg = true;
                }
                else if (_other.gameObject.CompareTag(m_targetBodyTag))
                {
                    //Destroy(_other.gameObject);
                    Debug.Log("�e����I�����e����ă_���[�W������ւ�");
                    m_deflectedFlg = true;
                    SwordAttackOff();
                    m_judgeEndFlg = true;
                }
            }
        }
    }

    private void SwordAttackOn()
    {
        m_meshCollider.enabled = true;
    }

    private void SwordAttackOff()
    {
        m_meshCollider.enabled = false;
    }

    public bool Getm_hitFlg
    {
        get { return m_hitFlg; }
    }

    public bool Getm_deflectedFlg
    {
        get { return m_deflectedFlg; }
    }
}