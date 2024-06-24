using Effekseer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GolemMain : Golem
{
    [SerializeField] private GameObject m_armors;
    private Dissolve m_armorDissolves;
    private SkinMesh m_armorSkinMesh;

    [SerializeField] private float m_armorDissolveSpeed = 0.2f;
    private float m_armorDissolveRatio = 0.0f;

    EffekseerEffectAsset m_effect;
    EffekseerHandle m_effectHandle;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // �G�t�F�N�g�̐�
    public int m_effectNum = 1;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;

    [SerializeField] private float m_shrinkSpeed = 0.1f;


    [SerializeField] private List<Collider> m_laserCollider;
    [SerializeField] private Transform m_laserTransform;
    

    // ================================
    // �^�[�Q�b�g�ւ̉�]�␳�n
    // ================================
    bool m_trackingFlg = true;

    // ��
    [SerializeField] private Transform m_headTrans;

    // �O���̊�ƂȂ郍�[�J����ԃx�N�g��
    [SerializeField] private Vector3 m_forward = Vector3.back;

    Quaternion m_nowRot;

    // ���������x�N�g��
    private Vector3 m_initVec;
    private Quaternion m_initRot;

    // �T�E���h
    GolemSounds m_sound;


    void Start()
    {
        m_skinMesh = GetComponent<SkinMesh>();
        m_dissolve = GetComponent<Dissolve>();

        if (m_armors)
        {
            m_armorDissolves = m_armors.GetComponent<Dissolve>();
            m_armorSkinMesh = m_armors.GetComponent<SkinMesh>();
        }

        attackManager = GetComponent<AttackManager>();

        // �G�t�F�N�g���擾����B
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_initVec = m_forward;
        m_initRot.eulerAngles = m_initVec;
        m_initRot.eulerAngles += new Vector3(-90.0f, 180.0f, 0.0f);

        m_nowRot = m_initRot;
        m_headTrans.rotation = m_nowRot;
        //m_initVec += new Vector3(-90.0f, 0.0f, 0.0f);

        m_sound = GetComponent<GolemSounds>();
    }


    void Update()
    {
        if (!m_alive) { return; }

        WeakHit();


        // ====================================
        // �v���C���[��Ǐ]���鏈��
        // ====================================
        if (m_trackingFlg)
        {
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

            // �����p�x�ƃ��f���p�x�̍���
            Quaternion deltaDir = new Quaternion();
            deltaDir.eulerAngles = rot.eulerAngles - m_nowRot.eulerAngles;

            m_headTrans.eulerAngles += deltaDir.eulerAngles;
            m_nowRot = m_headTrans.rotation;
        }
        else
        {
            m_headTrans.rotation = m_initRot;
        }
    }


    public void BigLaserEffect()
    {
        // �G�t�F�N�g�̈ʒu�ݒ�
        m_effectPos = m_laserTransform.position;

        // �G�t�F�N�g�̉�]�ݒ�
        m_effectRot = transform.rotation;
        m_effectRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_effectHandle = EffekseerSystem.PlayEffect(m_effect, m_effectPos);
        m_effectHandle.SetRotation(m_effectRot);
    }


    private bool EndEffect()
    {
        bool result = false;

        m_effectScale -= Vector3.one * m_shrinkSpeed * Time.deltaTime;
        m_effectHandle.SetScale(m_effectScale);

        for (int i = 0; i < m_laserCollider.Count; i++)
        {
            m_laserCollider[i].enabled = false;
        }

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
        ArmorDestroy();
        m_laserTime -= Time.deltaTime;
        if (m_isLaser)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                for (int i = 0; i < m_laserCollider.Count; i++)
                {
                    m_laserCollider[i].enabled = true;
                }
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
                m_sound.PlayLaserCharge();
                Invoke("LaserShotSound", 2.0f);

                WeakOff();

                m_laserTime = m_shotTime;
                m_isLaser = true;
            }
        }
    }


    // �Z�j��
    public void ArmorDestroy()
    {
        if (!m_armors) { return; }
        if (!m_armorDissolves) { return; }

        m_armorDissolveRatio += m_armorDissolveSpeed * Time.deltaTime;
        m_armorDissolves.SetDissolveAmount(m_armorDissolveRatio);

        if (m_armorDissolveRatio >= 1.0f)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Armors")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        if (!m_armorSkinMesh) { return; }
        m_armorSkinMesh.SetSkinMeshShadow(false);
    }


    // ���𐳖ʂɌ�����
    private void TrackingOff()
    {
        m_trackingFlg = false;
        m_headTrans.rotation = m_initRot;
    }


    // �����^�[�Q�b�g�Ɍ�����
    private void TrackingOn()
    {
        m_trackingFlg = true;
    }


    private void LaserShotSound()
    {
        m_sound.PlayLaserShot();
        m_sound.PlayLaserKeep();
    }
}
