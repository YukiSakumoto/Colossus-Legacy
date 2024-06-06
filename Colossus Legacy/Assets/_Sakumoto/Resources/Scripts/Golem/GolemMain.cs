using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMain : Golem
{
    EffekseerEffectAsset m_effect;
    EffekseerHandle m_effectHandle;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // �G�t�F�N�g�̐��i�����x�������������߁A�G�t�F�N�g���ʂɕ����ƂŃf�J���[�U�[�ɂ��Ă݂�j
    public int m_effectNum = 1;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;

    [SerializeField] private Collider m_laserCollider;

    // ================================
    // �^�[�Q�b�g�ւ̉�]�␳�n
    // ================================
    // ��
    [SerializeField] private Transform m_headTrans;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 m_forward = Vector3.forward;

    // ���������x�N�g��
    private Vector3 m_initVec;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        // �G�t�F�N�g���擾����B
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_initVec = Vector3.back;
    }


    void Update()
    {
        if (!m_alive) { return; }

        WeakHit();

        // ====================================
        // �v���C���[��Ǐ]���鏈��
        // ====================================
        if (!m_target || !m_headTrans) { return; }

        // �^�[�Q�b�g�ւ̌����x�N�g���v�Z
        Vector3 dir = m_target.transform.position - m_headTrans.position;
        // �^�[�Q�b�g�̕����ւ̉�]
        Quaternion lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
        // ��]�␳
        Quaternion offsetRotation = Quaternion.FromToRotation(m_forward, Vector3.forward);

        // �^�[�Q�b�g�����ւ̉�]�̏��Ɏ��g�̌����𑀍�
        // �����ۂ̃v���C���[�ւ̌���������
        Quaternion rot = lookAtRotation * offsetRotation;
        // ���f���̌�������
        rot.eulerAngles += new Vector3(-90.0f, 0.0f, 0.0f);

        //// �����p�x�ƃ��f���p�x�̍���
        //Vector3 deltaDir = m_initVec - rot.eulerAngles;

        //m_headTrans.eulerAngles += deltaDir;
        m_headTrans.rotation = rot;
    }


    public void BigLaserEffect()
    {
        // �G�t�F�N�g�̈ʒu�ݒ�
        m_effectPos = transform.position;
        m_effectPos.y += 4.0f;
        m_effectPos.x -= 0.5f;

        // �G�t�F�N�g�̉�]�ݒ�
        m_effectRot = transform.rotation;
        m_effectRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_effectHandle = EffekseerSystem.PlayEffect(m_effect, m_effectPos);
        m_effectHandle.SetRotation(m_effectRot);
    }


    private bool EndEffect()
    {
        bool result = false;

        m_effectScale *= 0.95f;
        m_effectHandle.SetScale(m_effectScale);

        m_laserCollider.enabled = false;

        if (m_effectScale.x <= 0.01f)
        {
            m_effectScale = Vector3.one;
            m_effectHandle.Stop();

            WeakOn();

            result = true;
        }

        return result;
    }


    public void SpecialAttack()
    {
        m_laserTime -= Time.deltaTime;
        if (m_isLaser)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                m_laserCollider.enabled = true;
            }
        }


        if (m_laserTime <= 0.0f)
        {
            // �N�[���_�E�����c
            if (m_isLaser)
            {
                bool fadeOut = false;
                
                fadeOut = EndEffect();
                if (fadeOut)
                {
                    m_laserTime = m_coolTime;
                    m_isLaser = false;
                }
            }
            // �r�[������I
            else
            {
                if (m_stop) return;

                BigLaserEffect();

                m_laserTime = m_shotTime;
                m_isLaser = true;
            }
        }
    }


    // �Z�j��
    public void ArmorDestroy()
    {
        // �ł���΃f�B�]���u�ł���Ă݂����I
        foreach (Transform child in transform)
        {
            if (child.name == "Armors")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
