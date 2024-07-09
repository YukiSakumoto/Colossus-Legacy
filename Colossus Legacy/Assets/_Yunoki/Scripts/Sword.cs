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
    private GameStatusManager m_gameStatusManager;

    private float m_notHitTime = 0f;
    private const float m_notHitSetTime = 0.2f;
    private bool m_swordEnableFlg = false;
    private bool m_swordFirstFlg = false;
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

        m_gameStatusManager = GameObject.FindWithTag("GameManager").GetComponent<GameStatusManager>();
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

        if(m_swordEnableFlg)
        {
            if (m_notHitTime <= 0f)
            {
                m_meshCollider.enabled = true;
            }
        }
        else
        {
            m_meshCollider.enabled = false;
        }

        if (m_notHitTime > 0f) 
        {
            m_notHitTime -= Time.deltaTime;
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
                    Debug.Log("�q�b�g�I�����G�̎�_�ɓ�����������");
                    m_hitFlg = true;
                    SwordAttackOff();
                    m_judgeEndFlg = true;
                }
                else if (_other.gameObject.CompareTag(m_targetBodyTag))
                {
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
        m_swordEnableFlg = true;
        if (!m_swordFirstFlg)
        {
            m_swordFirstFlg = true;
            m_notHitTime = m_notHitSetTime;
        }
    }

    private void SwordAttackOff()
    {
        m_swordEnableFlg = false;
        m_swordFirstFlg = false;
        m_notHitTime = 0f;
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