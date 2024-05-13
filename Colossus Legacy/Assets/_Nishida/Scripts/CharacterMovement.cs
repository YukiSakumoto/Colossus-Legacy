using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public float m_leftRightSpeed = 4f;  // �L�����N�^�[�̈ړ����x
    public float m_rotationSpeed = 200f; // �L�����N�^�[�̉�]���x

    private Rigidbody m_rb; // ���W�b�h�{�f�B�B

    private float m_rollCoolTime = 0f;         // ����s���̎��s����
    private float m_rollStiffnessTime = 0f;    // ����s���I�����̍d������
    private float m_weaponAttackCoolTime = 0f; // �U�����[�V��������ړ��Ɉڂ��܂ł̎���
    private float m_weaponChangeCoolTime = 0f; // ����̎�ނ�ς��鎞�̃N�[���^�C��

    private bool m_walkFlg = false;                      // �ړ����Ă��邩�̔���(AnimationMovement�ւ̈ڑ��p)
    private bool m_weaponFlg = false;                    // ���݌��Ƌ|�̂ǂ�����g�p���Ă��邩����(AnimationMovement�ւ̈ڑ��p)
    private bool m_attackFlg = false;                    // �U���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_subAttackFlg = false;                 // �T�u�U���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_rollFlg = false;                      // ����s���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_rollCoolTimeCheckFlg = false;         // ����s���̎��s���Ԓ����̊Ǘ�
    private bool m_rollStiffnessTimeCheckFlg = false;    // ����s���̍d�����Ԓ����̊Ǘ�
    private bool m_rollFinishCheckFlg = false;           // ����s�����I�����Ă��邩�̊Ǘ�
    private bool m_weaponAttackCoolTimeCheckFlg = false; // �U�����[�V��������ړ��Ɉڂ��܂ł̎��Ԃ��̊Ǘ�
    private bool m_weaponChangeCoolTimeCheckFlg = false; // ����̎�ނ�ς��鎞�̃N�[���^�C�����̊Ǘ�

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // �L�[�{�[�h�̍��E����
        float varticalInput = Input.GetAxis("Vertical"); // �L�[�{�[�h�̏㉺����

        // �ړ��̏����B����s���A����`�F���W���A�U�����͈ړ��s��
        if (!m_rollFinishCheckFlg && !m_weaponChangeCoolTimeCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
        {
            if (horizontalInput != 0f || varticalInput != 0f)
            {
                m_walkFlg = true;
            }
            else
            {
                m_walkFlg = false;
            }

            // �ړ��ʌv�Z
            Vector3 movement = new Vector3(horizontalInput, 0.0f, varticalInput) * m_leftRightSpeed;

            // Rigidbody���g���ăL�����N�^�[���ړ�
            m_rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);

            // �L�����N�^�[�̌�������͕����ɕύX
            if (movement != Vector3.zero)
            {
                transform.forward = movement; // �L�����N�^�[���ړ������Ɍ�����
            }
        }

        // ����s�����̈ړ�����
        if (m_rollCoolTimeCheckFlg)
        {
            // Y���̉�]�ɍ��킹�Ĉړ��������v�Z����
            Vector3 rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
            Vector3 movement = rotationDirection * (m_leftRightSpeed * 2.3f);

            // Rigidbody���g���ăI�u�W�F�N�g���ړ�
            m_rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);

            // �ړ�������0�łȂ��ꍇ�ɃI�u�W�F�N�g�̌�����ύX����
            if (movement != Vector3.zero)
            {
                transform.forward = movement.normalized;
            }
        }

        // ����`�F���W���̏���
        if (Input.GetKey(KeyCode.F))
        {
            if (!m_weaponChangeCoolTimeCheckFlg)
            {
                if (!m_weaponFlg) // �|�ɕύX
                {
                    m_weaponFlg = true;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = 2f;
                }
                else // ���ɕύX
                {
                    m_weaponFlg = false;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = 2f;
                }
            }
        }

        // ����s�����[�V�����̏d���h�~
        if (m_rollFlg)
        {
            m_rollFlg = false;
        }

        // �U�����[�V�����̏d���h�~
        if (m_attackFlg)
        {
            m_attackFlg = false;
        }

        // �T�u�U�����[�V�����̏d���h�~
        if (m_subAttackFlg)
        {
            m_subAttackFlg = false;
        }

        // �������Ă��镐��ōU���B�g��������ɂ���čd�����Ԃ��قȂ�B
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
            {
                m_attackFlg = true;
                if (!m_weaponFlg) // ���ōU��
                {
                    m_weaponAttackCoolTime = 0.9f;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
                else // �|�ōU��
                {
                    m_weaponAttackCoolTime = 2.0f;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
            }
        }

        // �T�u�U���B���e�𓊂���B
        if (Input.GetMouseButtonDown(1))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
            {
                m_subAttackFlg = true;
                m_weaponAttackCoolTime = 1.2f;
                m_weaponAttackCoolTimeCheckFlg = true;
            }
        }

        // ����s��
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_rollFinishCheckFlg)
            {
                m_rollCoolTime = 0.9f;
                m_rollStiffnessTime = 0.5f;
                m_rollCoolTimeCheckFlg = true;
                m_rollFinishCheckFlg = true;
                m_rollFlg = true;
            }
        }

        // ����s�����̃��[�V�������ԏ���
        if (m_rollCoolTimeCheckFlg)
        {
            m_rollCoolTime -= Time.deltaTime;
            if (m_rollCoolTime <= 0)
            {
                m_rollStiffnessTimeCheckFlg = true;
                m_rollCoolTimeCheckFlg = false;
            }
        }

        // ����s�����̍d�����ԏ���
        if (m_rollStiffnessTimeCheckFlg)
        {
            m_rollStiffnessTime -= Time.deltaTime;
            if (m_rollStiffnessTime <= 0)
            {
                m_rollStiffnessTimeCheckFlg = false;
                m_rollFinishCheckFlg = false;
            }
        }

        // ����`�F���W���̍d�����ԏ���
        if (m_weaponChangeCoolTimeCheckFlg)
        {
            m_weaponChangeCoolTime -= Time.deltaTime;
            if (m_weaponChangeCoolTime <= 0)
            {
                m_weaponChangeCoolTimeCheckFlg = false;
            }
        }

        // �U�����̍d�����ԏ���
        if (m_weaponAttackCoolTimeCheckFlg)
        {
            m_weaponAttackCoolTime -= Time.deltaTime;
            if (m_weaponAttackCoolTime <= 0)
            {
                m_weaponAttackCoolTimeCheckFlg = false;
            }
        }
    }

    public bool Getm_walkFlg
    {
        get { return m_walkFlg; }
    }

    public bool Getm_rollFlg
    {
        get { return m_rollFlg; }
    }

    public bool Getm_weaponFlg
    {
        get { return m_weaponFlg; }
    }
    public bool Getm_attackFlg
    {
        get { return m_attackFlg; }
    }

    public bool Getm_subAttackFlg
    {
        get { return m_subAttackFlg; }
    }
}