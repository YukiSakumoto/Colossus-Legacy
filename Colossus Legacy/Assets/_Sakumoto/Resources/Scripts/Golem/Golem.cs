using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

public class Golem : MonoBehaviour
{
    protected AttackManager attackManager;
    [SerializeField] private List<Collider> attackColliders;

    [SerializeField] private GolemLeft m_golemLeft;     // Unity�ŃA�^�b�`�ς�
    [SerializeField] private GolemRight m_golemRight;   // Unity�ŃA�^�b�`�ς�
    [SerializeField] private GolemMain m_golemMain;

    [SerializeField] private GameObject m_myself;
    [SerializeField] private GameObject m_target;

    [SerializeField] private TMPro.TMP_Text m_text;

    protected bool m_stop = false;          // ���r�𓯎��ɍ��킹�邽�߂̃t���O

    [SerializeField] protected int m_hp = 100;               // �S�[�����̗̑�

    protected bool m_damageFlg = false;     // �U�����󂯂��ۂ̃t���O
    [SerializeField] private float m_damageTime = 0.0f;
    [SerializeField] private int m_damagePoint = 0;

    private float m_time = 0.0f;

    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        m_golemLeft = GameObject.Find("Golem_Left").GetComponent<GolemLeft>();
        m_golemRight = GameObject.Find("Golem_Right").GetComponent<GolemRight>();
        m_golemMain = GameObject.Find("Golem_Main").GetComponent<GolemMain>();
    }

    void Update()
    {
        // Null �`�F�b�N
        if (!m_golemLeft || !m_golemRight || !m_golemMain)
        {
            if (!m_golemLeft) { Debug.Log("LNull!!"); }
            if (!m_golemRight) { Debug.Log("RNull!!"); }
            if (!m_golemMain) { Debug.Log("MNull!!"); }

            return;
        }

        // �_���[�W�̏������Ɏ����Ă���i�������^�[���j
        if (!m_damageFlg)
        {
            // �ǂ��炩�̕��ʂ��_���[�W���󂯂���ԂȂ�S�Ă̕��ʂ��_���[�W��Ԃɂ���
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                // HP�����炷��
                m_hp -= m_damagePoint;

                m_damageFlg = true;

                // �_���[�W�A�j���[�V�����̍Đ�
                m_golemLeft.HitDamage();
                m_golemRight.HitDamage();
                m_golemMain.HitDamage();

                m_time = m_damageTime;
                if (m_hp <= 0)
                {
                    m_time = 3.0f;
                }

                return;
            }
        }
        else
        {
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                m_time -= Time.deltaTime;

                if (m_hp <= 0)
                {
                    if (m_time <= 0.0f)
                    {
                        m_golemMain.ArmorDestroy();
                        m_golemMain.WakeUp();
                        m_golemMain.SpecialAttack();
                    }
                    return;
                }

                // �_���[�W�A�j���[�V�������I�����鏈��
                if (m_time <= 0.0f)
                {
                    m_damageFlg = false;

                    m_golemLeft.WakeUp();
                    m_golemRight.WakeUp();
                    m_golemMain.WakeUp();
                }
            }
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
        m_stop = false;
        attackManager.AnimationFin();

        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
        }
    }


    // �U�����󂯂��ۂ̏���
    public void HitDamage()
    {
        m_stop = true;
        m_damageFlg = true;
        attackManager.ChangeAnimation("Damage", m_damageFlg);
    }


    // �U������N���オ�鏈��
    public void WakeUp()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ChangeAnimation("Damage", m_damageFlg);
    }


    // �A�j���[�V�����I�����ɓǂݍ���
    private void ResetAnimation()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ResetAnimation();
    }


    // ���S����
    public void Death()
    {
        Destroy(this.gameObject);
    }


    public bool GetStop() { return m_stop; }
}
