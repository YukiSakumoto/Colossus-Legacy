using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Effekseer;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rb; // ���W�b�h�{�f�B

    [SerializeField] private string m_targetParentTag = "EnemyAttack"; // �G�Ƃ̓����蔻����s�����̃^�O���ݒ�

    // �_���[�W��
    enum Damage
    {
        small = 20,  // ���_���[�W
        medium = 50, // ���_���[�W
        big = 70,    // ��_���[�W
        death = 100  // �����U��
    }
    // �_���[�W���̃m�b�N�o�b�N��
    enum KnockBack
    {
        none = 0,    // �m�b�N�o�b�N���Ȃ�
        small = 2,   // ���m�b�N�o�b�N
        medium = 4,  // ���m�b�N�o�b�N
        big = 7,    // ��m�b�N�o�b�N
    }
    // �񕜗�
    enum Recovery
    {
        small = 20,  // ����
        medium = 50, // ����
        big = 70,    // ���
        full = 100   // ���S��
    }

    private const int m_playerMaxLife = 100;   // ��l���̗̑͂̏���l
    private const int m_rollTiredCountMax = 5; // ����s���̈ړ������ʃJ�E���g�̏��

    [SerializeField] private int m_playerLife = m_playerMaxLife; // ��l���̗̑�
    [SerializeField] private int m_rollTiredCount = 0;           // ��l���̉���s����A�����Ďg���ƒi�X�ɖ��ɂȂ��Ă����J�E���g

    private const float m_leftRightSpeed = 4f;           // �L�����N�^�[�̈ړ����x
    private const float m_rollCoolSetTime = 0.8f;        // ����s���̎��s���ԌŒ�l
    private const float m_rollStiffnessSetTime = 0.5f;   // ����s���I�����̍d�����ԌŒ�l
    private const float m_rollAcceleration = 2.4f;       // ����s���̉����ʌŒ�l
    private const float m_rollTiredDecreaseBase = 0.25f; // ����s���̌����ʐݒ�
    private const float m_rollTiredDecreaseTimeBase = 3f;// ����s���̌����ʉ񕜎��ԌŒ�l
    private const float m_swordAttackCoolSetTime = 0.9f; // ���ōU�������Ƃ��̍d�����ԌŒ�l
    private const float m_bowAttackCoolSetTime = 1.3f;   // �|�ōU�������Ƃ��̍d�����ԌŒ�l
    private const float m_subAttackCoolSetTime = 1f;   // �T�u�U�������Ƃ��̍d�����ԌŒ�l
    private const float m_weaponChangeCoolSetTime = 1f;  // ����`�F���W���̃N�[���^�C���Œ�l
    private const float m_damageCoolSetTime = 0.6f;      // �_���[�W���󂯂���̍d�����ԌŒ�l
    private const float m_invincibilitySetTime = 2f;     // �_���[�W���󂯂���̖��G���ԌŒ�l
    private const float m_blownAwayStiffnessSetTime = 0.8f; // ������ԃ_���[�W���󂯂��Ƃ��̍d�����ԌŒ�l

    private float m_rollCoolTime = 0f;         // ����s���̎��s����
    private float m_rollStiffnessTime = 0f;    // ����s���I�����̍d������
    private float m_rollTiredDecrease = 0f;    // ����s���̌����ʐݒ�
    private float m_rollTiredDecreaseTime = 0f;// ����s���̌����ʉ񕜎���
    private float m_weaponAttackCoolTime = 0f; // �U�����[�V��������ړ��Ɉڂ��܂ł̎���
    private float m_weaponChangeCoolTime = 0f; // ����̎�ނ�ς��鎞�̃N�[���^�C��
    private float m_damageCoolTime = 0f;       // �_���[�W���󂯂���̍d������
    private float m_invincibilityTime = 0f;    // �_���[�W���󂯂���̖��G����
    private float m_blownAwayStiffnessTime = 0f;// ������ԃ_���[�W���󂯂��Ƃ��̍d������

    private bool m_walkFlg = false;                      // �ړ����Ă��邩�̔���(AnimationMovement�ւ̈ڑ��p)
    private bool m_weaponFlg = false;                    // ���݌��Ƌ|�̂ǂ�����g�p���Ă��邩����(AnimationMovement�ւ̈ڑ��p)
    private bool m_attackFlg = false;                    // �U���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_subAttackFlg = false;                 // �T�u�U���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_rollFlg = false;                      // ����s���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_damageFlg = false;                    // �v���C���[��_���[�W���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_blownAwayFlg = false;                 // ������ԍU�����󂯂��Ƃ��̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_deathFlg = false;                     // ���S���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_damageMotionFlg = false;              // �_���[�W���[�V�������̊Ǘ�
    private bool m_damageBlownAwayFlg = false;           // ������ԃ��[�V�������̊Ǘ�
    private bool m_damageBlownAwayStiffnessFlg = false;  // ������ԃ��[�V�����̍d�����Ԃ̊Ǘ�
    private bool m_invincibleFlg = false;                // ���G���Ԓ��̊Ǘ�
    private bool m_rollCoolTimeCheckFlg = false;         // ����s���̎��s���Ԓ����̊Ǘ�
    private bool m_rollStiffnessTimeCheckFlg = false;    // ����s���̍d�����Ԓ����̊Ǘ�
    private bool m_rollFinishCheckFlg = false;           // ����s�����I�����Ă��邩�̊Ǘ�
    private bool m_weaponAttackCoolTimeCheckFlg = false; // �U�����[�V��������ړ��Ɉڂ��܂ł̎��Ԃ��̊Ǘ�
    private bool m_weaponChangeCoolTimeCheckFlg = false; // ����̎�ނ�ς��鎞�̃N�[���^�C�����̊Ǘ�

    private Vector3 m_KnockBackVec = Vector3.zero; // �m�b�N�o�b�N�ʂ�������

    // Start is called before the first frame update
    void Start()
    {
        if(!m_rb)
        {
            Debug.Log("RigidBody is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // �L�[�{�[�h�̍��E����
        float varticalInput = Input.GetAxis("Vertical"); // �L�[�{�[�h�̏㉺����

        // �ړ��̏����B����s���A�U�����A�_���[�W���[�V�������A���S���͈ړ��s��
        if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg &&
            !m_damageMotionFlg && !m_damageBlownAwayStiffnessFlg && !m_deathFlg)
        {
            if (horizontalInput != 0f || varticalInput != 0f) // �L�[���͂�����Ă���Ƃ��͕������[�V�����ɂȂ�
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
        else if (m_rollCoolTimeCheckFlg) // ����s�����̈ړ�����
        {
            // Y���̉�]�ɍ��킹�Ĉړ��������v�Z����
            Vector3 rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
            Vector3 movement = rotationDirection * (m_leftRightSpeed * (m_rollAcceleration - m_rollTiredDecrease));

            // Rigidbody���g���ăI�u�W�F�N�g���ړ�
            m_rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);

            // �ړ�������0�łȂ��ꍇ�ɃI�u�W�F�N�g�̌�����ύX����
            if (movement != Vector3.zero)
            {
                transform.forward = movement.normalized;
            }
        }
        else if(m_damageBlownAwayFlg) // �_���[�W���[�V�������̃m�b�N�o�b�N�̓����Ƃ�
        {
            // enum�ɏ����_��������Ȃ��̂Ōv�Z�p
            float knockbackPower = 10f;

            // �m�b�N�o�b�N�ʌv�Z
            Vector3 movement = m_KnockBackVec.normalized * ((float)KnockBack.medium / knockbackPower);

            // �m�b�N�o�b�N�����ɗ͂������ăL�����N�^�[���ړ�������
            m_rb.MovePosition(transform.position + movement);
        }

        // ����`�F���W���̏���
        if (Input.GetKey(KeyCode.F))
        {
            if (!m_weaponChangeCoolTimeCheckFlg) // ����`�F���W���A���Ŕ������Ȃ��悤�t���O�ŊǗ�
            {
                if (!m_weaponFlg) // �|�ɕύX
                {
                    m_weaponFlg = true;
                    m_weaponChangeCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                }
                else // ���ɕύX
                {
                    m_weaponFlg = false;
                    m_weaponChangeCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                }
            }
        }

        // ���[�V�����d���̊Ǘ��n����
        motionProcessing();

        // �}�E�X���N���b�N�ő������Ă��镐��ōU���B�g��������ɂ���čd�����Ԃ��قȂ�B
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg) // ����s���y�эU���̃��[�V�������͍U���ł��Ȃ�
            {
                m_attackFlg = true;
                if (!m_weaponFlg) // ���ōU��
                {
                    m_weaponAttackCoolTime = m_swordAttackCoolSetTime;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
                else // �|�ōU��
                {
                    m_weaponAttackCoolTime = m_bowAttackCoolSetTime;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
            }
        }

        // �T�u�U���B�}�E�X�̉E�N���b�N�Ŕ��e�𓊂���B
        if (Input.GetMouseButtonDown(1))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg) // ����s���y�эU���̃��[�V�������͍U���ł��Ȃ�
            {
                m_subAttackFlg = true;
                m_weaponAttackCoolTime = m_subAttackCoolSetTime;
                m_weaponAttackCoolTimeCheckFlg = true;
            }
        }

        // ����s��
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_rollFinishCheckFlg)
            {
                m_rollCoolTime = m_rollCoolSetTime; // ����s�����̎��Ԑݒ�
                m_rollStiffnessTime = m_rollStiffnessSetTime; // ����s����̍d�����Ԑݒ�
                m_rollCoolTimeCheckFlg = true;
                m_rollFinishCheckFlg = true;
                m_rollFlg = true;
                // ����s�������邽�тɒi�X�X�s�[�h��������
                if (m_rollTiredCount < m_rollTiredCountMax)
                {
                    m_rollTiredCount++; // �����̗ʂ̑���
                }
                else
                {
                    // ����s���̌����񐔂̌��E
                    m_rollTiredCount = m_rollTiredCountMax;
                }
                m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase; // �����̉񕜂ɂ����鎞�Ԑݒ� 
                m_rollTiredDecrease = m_rollTiredDecreaseBase * m_rollTiredCount; // �����ʌv�Z
            }
        }

        // ���Ԍo�߂Ŕ���������t���O��ύX���鏈��
        timeProcessing();
    }

    private void LateUpdate()
    {
        // ������ԃ��[�V�����̏d���h�~
        if (m_blownAwayFlg)
        {
            m_blownAwayFlg = false;
        }
    }

    // Is Trigger���t���Ă���Collider�ɐڐG�����Ƃ��̏���
    void OnTriggerEnter(Collider _other)
    {
        // �_���[�W���[�V�������△�G���̓_���[�W���󂯂Ȃ�
        if (!m_damageMotionFlg && !m_damageBlownAwayFlg && !m_invincibleFlg)
        {
            // �G�ꂽ�I�u�W�F�N�g�̐e�I�u�W�F�N�g���擾
            Transform parentTransform = _other.transform.parent?.parent;

            // �e�I�u�W�F�N�g�����݂��邩���m�F
            if (parentTransform != null)
            {
                if (parentTransform.gameObject.CompareTag(m_targetParentTag)) // �e�I�u�W�F�N�g���w��̃^�O�������Ă��邩�m�F
                {
                    // ������������̐e�I�u�W�F�N�g�̖��O���R���\�[���ɕ\������
                    Debug.Log("hit at " + parentTransform.name + " Tag");

                    // �U�����s�����I�u�W�F�N�g�̈ʒu����U�����󂯂��I�u�W�F�N�g�̈ʒu�������āA�U�����󂯂������̃x�N�g�����v�Z
                    m_KnockBackVec = transform.position - parentTransform.position;

                    int damage = (int)Damage.small;
                    hit(damage);
                }
                else
                {
                    // ���������I�u�W�F�N�g�̐e�I�u�W�F�N�g�̐e�I�u�W�F�N�g�Ƀ^�O���ݒ肳��Ă��Ȃ��ꍇ�ɃR���\�[���ɕ\��
                    Debug.Log(parentTransform.name + " is does not have the " + m_targetParentTag + " Tag");
                }
            }
            else
            {
                // �e�I�u�W�F�N�g�̐e�I�u�W�F�N�g�ɓ�����I�u�W�F�N�g������
                Debug.Log("No parent object found");
            }
        }
    }

    // �_���[�W���󂯂��Ƃ��̔ėp����
    void hit(int _damage)
    {
        m_playerLife -= _damage;
        if (m_playerLife > 0) // �_���[�W���󂯂đ̗͂�0�ȉ��ɂȂ�Ȃ���΃_���[�W���[�V���� + ���G���Ԕ���
        {
            // ���@�_���[�W���󂯂����m�b�N�o�b�N���Ȃ��ꍇ
            if(!m_weaponFlg)
            {
                m_damageFlg = true;
                m_damageMotionFlg = true;
            }
            else // ���@�_���[�W���󂯂����m�b�N�o�b�N����ꍇ
            {
                m_blownAwayFlg = true;
                m_damageBlownAwayFlg = true;
                m_damageBlownAwayStiffnessFlg = true;
                m_blownAwayStiffnessTime = m_blownAwayStiffnessSetTime;
            }
            m_damageCoolTime = m_damageCoolSetTime;
            m_invincibilityTime = m_invincibilitySetTime;
 
        }
        else // �̗͂�0�ȉ��ɂȂ����ꍇ�Ɏ��S���ē������~�߂�
        {
            m_deathFlg = true;
            Debug.Log("Player Life is Empty");
        }
    }

    // ���[�V�����d���h�~�p�̃t���O�Ǘ��n����
    void motionProcessing()
    {
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

        // �_���[�W���[�V�����̏d���h�~
        if (m_damageFlg)
        {
            m_damageFlg = false;
        }
    }

    // ���Ԍo�߂Ŕ���������t���O��ύX���鏈��
    void timeProcessing()
    {
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

        // �_���[�W���[�V�������̏���
        if (m_damageMotionFlg)
        {
            m_damageCoolTime -= Time.deltaTime;
            if (m_damageCoolTime <= 0)
            {
                m_damageMotionFlg = false;
                m_invincibleFlg = true;
            }
        }

        // ������ԃ��[�V�������̏���
        if (m_damageBlownAwayFlg)
        {
            m_damageCoolTime -= Time.deltaTime;
            if (m_damageCoolTime <= 0)
            {
                m_damageBlownAwayFlg = false;
                m_invincibleFlg = true;
            }
        }
        else if (m_damageBlownAwayStiffnessFlg) // ������ԃ��[�V������̍d������
        {
            m_blownAwayStiffnessTime -= Time.deltaTime;
            if (m_blownAwayStiffnessTime < 0)
            {
                m_damageBlownAwayStiffnessFlg = false;
            }
        }
        else if (m_invincibleFlg) // ���G���Ԓ��̏���
        {
            m_invincibilityTime -= Time.deltaTime;
            if (m_invincibilityTime <= 0)
            {
                m_invincibleFlg = false;
            }
        }

        // ����s���ړ��ʌ������Ԃ̉񕜂̏���
        if (m_rollTiredCount > 0)
        {
            m_rollTiredDecreaseTime -= Time.deltaTime;
            if (m_rollTiredDecreaseTime <= 0)
            {
                m_rollTiredCount--;
                if (m_rollTiredCount > 0)
                {
                    m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase;
                }
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
    public bool Getm_damageFlg
    {
        get { return m_damageFlg; }
    }
    public bool Getm_blownAwayFlg
    {
        get { return m_blownAwayFlg; }
    }
    public bool Getm_deathFlg
    {
        get { return m_deathFlg; }
    }
}