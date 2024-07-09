using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_swordObject;
    [SerializeField] private GameObject m_bowObject;
    [SerializeField] private Rigidbody m_rb; // ���W�b�h�{�f�B
    [SerializeField] private Image m_hpGage;
    //[SerializeField] private string m_targetTag = "EnemyAttack"; // �G�Ƃ̓����蔻����s�����̃^�O���ݒ�
    //[SerializeField] private string m_bombTag = "BombAttack"; // ���e�Ƃ̓����蔻����s�����̃^�O���ݒ�
    [SerializeField] CharacterManager m_manager;
    //Sword m_swordClass;
    Bow m_bowClass;
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
    private const int m_playerCautionLife = 50;   // ��l���̗̑͂̒��Ӓl(�o�[���F)
    private const int m_playerDangerLife = 20;   // ��l���̗̑͂̊댯�l(�o�[�ԐF)
    private const int m_rollTiredCountMax = 5; // ����s���̈ړ������ʃJ�E���g�̏��

    [SerializeField] private int m_playerLife = m_playerMaxLife; // ��l���̗̑�
    [SerializeField] private int m_rollTiredCount = 0;           // ��l���̉���s����A�����Ďg���ƒi�X�ɖ��ɂȂ��Ă����J�E���g

    private const float m_leftRightSpeed = 4f;                    // �L�����N�^�[�̈ړ����x
    private const float m_rollCoolSetTime = 0.8f;                 // ����s���̎��s���ԌŒ�l
    private const float m_rollStiffnessSetTime = 0.5f;            // ����s���I�����̍d�����ԌŒ�l
    private const float m_rollAcceleration = 2.4f;                // ����s���̉����ʌŒ�l
    private const float m_rollTiredDecreaseBase = 0.25f;          // ����s���̌����ʐݒ�
    private const float m_rollTiredDecreaseTimeBase = 3f;         // ����s���̌����ʉ񕜎��ԌŒ�l
    private const float m_swordAttackCoolSetTime = 0.8f;          // ���ōU�������Ƃ��̍d�����ԌŒ�l
    private const float m_bowAttackCoolSetTime = 1.6f;            // �|�ōU�������Ƃ��̍d�����ԌŒ�l
    private const float m_subAttackCoolSetTime = 1f;              // �T�u�U�������Ƃ��̍d�����ԌŒ�l
    private const float m_weaponChangeCoolSetTime = 1f;           // ����`�F���W���̃N�[���^�C���Œ�l
    private const float m_damageCoolSetTime = 0.6f;               // �_���[�W���󂯂���̍d�����ԌŒ�l
    private const float m_downCoolSetTime = 1.7f;                 // �Ԃ��ꂽ��̍d�����ԌŒ�l
    private const float m_pushUpCoolSetTime = 2f;                 // �J�`�グ��ꂽ��̍d�����ԌŒ�l
    private const float m_invincibilitySetTime = 2f;              // �_���[�W���󂯂���̖��G���ԌŒ�l
    private const float m_blownAwayStiffnessSetTime = 0.8f;       // ������ԃ_���[�W���󂯂��Ƃ��̍d�����ԌŒ�l
    private const float m_swordMoveAcceleration = 4f;             // ���ōU�������Ƃ��̑O�i�Œ�l
    private const float m_swordMoveStiffnessSetTime = 0.4f;       // �U������O�̍d�����ԌŒ�l
    private const float m_swordMoveSetTime = 0.1f;                // �U������Ƃ��̈ړ����ԌŒ�l
    private const float m_swordSecondMoveStiffnessSetTime = 0.4f; // 2�i�U�������Ƃ��̍d�����ԌŒ�l
    private const float m_swordSecondMoveSetTime = 0.1f;          // 2�i�U�������Ƃ��̈ړ����ԌŒ�l
    private const float m_bowShotSetTime = 0.57f;                 // �|�U�����s���^�C�~���O���ߌŒ�l

    private float m_rollCoolTime = 0f;           // ����s���̎��s����
    private float m_rollStiffnessTime = 0f;      // ����s���I�����̍d������
    private float m_rollTiredDecrease = 0f;      // ����s���̌����ʐݒ�
    private float m_rollTiredDecreaseTime = 0f;  // ����s���̌����ʉ񕜎���
    private float m_weaponAttackCoolTime = 0f;   // �U�����[�V��������ړ��Ɉڂ��܂ł̎���
    private float m_weaponChangeCoolTime = 0f;   // ����̎�ނ�ς��鎞�̃N�[���^�C��
    private float m_damageCoolTime = 0f;         // �_���[�W���󂯂���̍d������
    private float m_invincibilityTime = 0f;      // �_���[�W���󂯂���̖��G����
    private float m_blownAwayStiffnessTime = 0f; // ������ԃ_���[�W���󂯂��Ƃ��̍d������
    private float m_swordMoveStiffnessTime = 0f; // ����U���ē����Ƃ��̍d������
    private float m_swordMoveTime = 0f;          // ����U���ē�������
    private float m_bowShotTime = 0f;            // �|�U�����s���^�C�~���O����

    private bool m_walkAnimeFlg = false;                 // �ړ����Ă��邩�̔���(AnimationMovement�ւ̈ڑ��p)
    private bool m_weaponFlg = false;                    // ���݌��Ƌ|�̂ǂ�����g�p���Ă��邩����(AnimationMovement�ւ̈ڑ��p)
    private bool m_attackAnimeFlg = false;               // �U���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_rollAnimeFlg = false;                 // ����s���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_damageAnimeFlg = false;               // �v���C���[��_���[�W���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_blownAwayAnimeFlg = false;            // ������ԍU�����󂯂��Ƃ��̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_downAnimeFlg = false;                 // �ׂ����U�����󂯂��Ƃ��̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_pushUpAnimeFlg = false;               // �J�`�グ�U�����󂯂��Ƃ��̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_deathFlg = false;                     // ���S���̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_secondSwordAttackAnimeFlg = false;    // 2�i�U�����s���ۂ̊Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_joyAnimeFlg = false;                  // ���œ����Ȃ��Ȃ�Ǘ�(AnimationMovement�ւ̈ڑ��p)
    private bool m_damageMotionFlg = false;              // �_���[�W���[�V�������̊Ǘ�
    private bool m_damageBlownAwayFlg = false;           // ������ԃ��[�V�������̊Ǘ�
    private bool m_damageBlownAwayStiffnessFlg = false;  // ������ԃ��[�V�����̍d�����Ԃ̊Ǘ�
    private bool m_invincibleFlg = false;                // ���G���Ԓ��̊Ǘ�
    private bool m_rollCoolTimeCheckFlg = false;         // ����s���̎��s���Ԓ����̊Ǘ�
    private bool m_rollStiffnessTimeCheckFlg = false;    // ����s���̍d�����Ԓ����̊Ǘ�
    private bool m_rollFinishCheckFlg = false;           // ����s�����I�����Ă��邩�̊Ǘ�
    private bool m_weaponAttackCoolTimeCheckFlg = false; // �U�����[�V��������ړ��Ɉڂ��܂ł̎��Ԃ��̊Ǘ�
    private bool m_weaponChangeCoolTimeCheckFlg = false; // ����̎�ނ�ς��鎞�̃N�[���^�C�����̊Ǘ�
    private bool m_swordMoveFlg = false;                 // ����U��ۂ̑O�ړ�
    private bool m_secondSwordAttackFlg = false;         // 2�i�U�����s���ۂ̊Ǘ�
    private bool m_bowShotFlg = false;                   // �|�U�����s���ۂ̊Ǘ�
    private bool m_swordMotionFlg = false;               // ���U�����[�V�������̊Ǘ�
    private bool m_secondSwordMotionFlg = false;         // 2�i�U�����[�V�������̊Ǘ�
    private bool m_bowMotionFlg = false;                 // �|�U�����[�V�������̊Ǘ�
    private bool m_subAttackMotionFlg = false;           // �T�u�U�����[�V�������̊Ǘ�
    private bool m_joyFlg = false;                       // ���œ����Ȃ��Ȃ�Ǘ�

    private Vector3 m_KnockBackVec = Vector3.zero; // �m�b�N�o�b�N�ʂ�������

    // Start is called before the first frame update
    void Start()
    {
        if(!m_manager)
        {
            Debug.Log("CharacterMovement:manager is Null");
        }

        if(!m_swordObject)
        {
            Debug.Log("CharacterMovement:Sword is Null");
        }
        else
        {
            m_swordObject.SetActive(true);
        }

        if (!m_bowObject)
        {
            Debug.Log("CharacterMovement:Bow is Null");
        }
        else
        {
            m_bowObject.SetActive(false);
            m_bowClass = m_bowObject.GetComponent<Bow>();
        }

        if (!m_rb)
        {
            Debug.Log("CharacterMovement:RigidBody is Null");
        }

        if(!m_hpGage)
        {
            Debug.Log("CharacterMovement:HP Gage is Null");
        }
        else
        {
            m_hpGage.color = Color.green;
        }

        //if(m_targetTag == "")
        //{
        //    Debug.Log("CharacterMovement:tag is Null");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_joyFlg && !m_deathFlg)
        {
            // ����`�F���W���̏���
            if (Input.GetKey(KeyCode.F))
            {
                if (!m_weaponChangeCoolTimeCheckFlg) // ����`�F���W���A���Ŕ������Ȃ��悤�t���O�ŊǗ�
                {
                    if (!m_swordMotionFlg && !m_secondSwordMotionFlg && !m_bowMotionFlg && !m_subAttackMotionFlg) // �U�����͕���`�F���W�o���Ȃ��悤�ɂ���
                    {
                        if (!m_weaponFlg) // �|�ɕύX
                        {
                            m_swordObject.SetActive(false);
                            m_bowObject.SetActive(true);
                            m_weaponFlg = true;
                            m_weaponChangeCoolTimeCheckFlg = true;
                            m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                        }
                        else // ���ɕύX
                        {
                            m_swordObject.SetActive(true);
                            m_bowObject.SetActive(false);
                            m_weaponFlg = false;
                            m_weaponChangeCoolTimeCheckFlg = true;
                            m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                        }
                    }
                }
            }

            // ���[�V�����d���̊Ǘ��n����
            MotionProcessing();

            // �}�E�X���N���b�N�ő������Ă��镐��ōU���B�g��������ɂ���čd�����Ԃ��قȂ�B
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg) // ����s���y�эU���̃��[�V�������͍U���ł��Ȃ�
                {
                    m_attackAnimeFlg = true;
                    if (!m_weaponFlg) // ���ōU��
                    {
                        m_weaponAttackCoolTime = m_swordAttackCoolSetTime;
                        m_weaponAttackCoolTimeCheckFlg = true;
                        m_swordMotionFlg = true;
                        m_swordMoveFlg = true;
                        m_swordMotionFlg = true;
                        m_swordMoveStiffnessTime = m_swordMoveStiffnessSetTime;
                        m_swordMoveTime = m_swordMoveSetTime;
                    }
                    else // �|�ōU��
                    {
                        m_weaponAttackCoolTime = m_bowAttackCoolSetTime;
                        m_bowShotTime = m_bowShotSetTime;
                        m_weaponAttackCoolTimeCheckFlg = true;
                        m_bowMotionFlg = true;
                        m_bowShotFlg = true;
                    }
                }
            }

            // ����U�������ɑO�ɐi�ޏ���
            if (m_swordMoveFlg)
            {
                if (m_swordMoveStiffnessTime < m_swordMoveStiffnessSetTime)
                {
                    // ���[�V�������ɂ�����x�N���b�N�����2�i�U��
                    if (Input.GetMouseButtonDown(0))
                    {
                        m_secondSwordAttackAnimeFlg = true;
                        m_secondSwordAttackFlg = true;
                        m_secondSwordMotionFlg = true;
                    }
                }

                m_swordMoveStiffnessTime -= Time.deltaTime;
                if (m_swordMoveStiffnessTime < 0)
                {
                    m_swordMoveTime -= Time.deltaTime;
                    if (m_swordMoveTime < 0 || m_manager.Getm_hitFlg)
                    {
                        m_swordMoveFlg = false;
                        if (m_secondSwordAttackFlg) // 2�i�U�����s���p�̕ϐ��ݒ�
                        {
                            m_swordMoveStiffnessTime = m_swordSecondMoveStiffnessSetTime;
                            m_swordMoveTime = m_swordSecondMoveSetTime;
                        }
                    }
                }
            }
            else
            {
                // 2�i�U�����s�����̂ݔh��
                if (m_secondSwordAttackFlg)
                {
                    m_swordMoveStiffnessTime -= Time.deltaTime;
                    if (m_swordMoveStiffnessTime < 0)
                    {
                        m_swordMoveTime -= Time.deltaTime;
                        if (m_swordMoveTime < 0 || m_manager.Getm_hitFlg)
                        {
                            m_secondSwordAttackFlg = false;
                        }
                    }
                }
            }

            // �T�u�U���B�}�E�X�̉E�N���b�N�Ŕ��e�𓊂���B
            if (m_manager.Getm_bombThrowCheckFlg)
            {
                if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
                {
                    m_weaponAttackCoolTime = m_subAttackCoolSetTime;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_subAttackMotionFlg = true;
                }
            }

            // ����s��
            if (Input.GetKey(KeyCode.Space))
            {
                if (!m_bowShotFlg && !m_deathFlg && !m_bowMotionFlg && !m_subAttackMotionFlg)
                {
                    if (!m_rollFinishCheckFlg)
                    {
                        m_rollCoolTime = m_rollCoolSetTime; // ����s�����̎��Ԑݒ�
                        m_rollStiffnessTime = m_rollStiffnessSetTime; // ����s����̍d�����Ԑݒ�
                        m_rollCoolTimeCheckFlg = true;
                        m_rollFinishCheckFlg = true;
                        m_rollAnimeFlg = true;
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
            }
        }
        else
        {
            m_walkAnimeFlg = false;
        }
        // �f�o�b�O�BY�L�[���������тɉ�
        //if(Input.GetKey(KeyCode.Y)) 
        //{
        //    Cure((int)Recovery.small);
        //}

        // ���Ԍo�߂Ŕ���������t���O��ύX���鏈��
        TimeProcessing();
    }

    private void FixedUpdate()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        //Vector3 position = transform.position;
        //if (position.y < 0)
        //{
        //    position.y = 0;
        //    transform.position = position;
        //}

        if (!m_joyFlg)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // �L�[�{�[�h�̍��E����
            float varticalInput = Input.GetAxis("Vertical"); // �L�[�{�[�h�̏㉺����

            bool moveFlg = false;

            Vector3 movement = new(0,0,0);
            Vector3 rotationDirection = new(0,0,0);
            Vector3 newPosition = new(0, 0, 0);
            RaycastHit hit;

            // �ړ��̏����B����s���A�U�����A�_���[�W���[�V�������A���S���͈ړ��s��
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg &&
                !m_damageMotionFlg && !m_damageBlownAwayStiffnessFlg && !m_deathFlg)
            {
                if (horizontalInput != 0f || varticalInput != 0f) // �L�[���͂�����Ă���Ƃ��͕������[�V�����ɂȂ�
                {
                    m_walkAnimeFlg = true;
                }
                else
                {
                    m_walkAnimeFlg = false;
                }

                moveFlg = true;

                // �ړ��ʌv�Z
                movement = new Vector3(horizontalInput, 0.0f, varticalInput) * m_leftRightSpeed;

                // �L�����N�^�[�̌�������͕����ɕύX
                if (movement != Vector3.zero)
                {
                    transform.forward = movement; // �L�����N�^�[���ړ������Ɍ�����
                }
            }
            else if (m_rollCoolTimeCheckFlg) // ����s�����̈ړ�����
            {
                moveFlg = true;

                // Y���̉�]�ɍ��킹�Ĉړ��������v�Z����
                rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
                movement = rotationDirection * (m_leftRightSpeed * (m_rollAcceleration - m_rollTiredDecrease));

                // �ړ�������0�łȂ��ꍇ�ɃI�u�W�F�N�g�̌�����ύX����
                if (movement != Vector3.zero)
                {
                    transform.forward = movement.normalized;
                }
            }
            else if (m_damageBlownAwayFlg) // �_���[�W���[�V�������̃m�b�N�o�b�N�̓����Ƃ�
            {
                moveFlg = true;

                // enum�ɏ����_��������Ȃ��̂Ōv�Z�p
                float knockbackPower = 10f;

                // �m�b�N�o�b�N�ʌv�Z
                movement = m_KnockBackVec.normalized * ((float)KnockBack.medium * knockbackPower);
            }

            // ����U���Ă���Ԃ̏���
            if (m_swordMoveFlg || (!m_swordMoveFlg && m_secondSwordAttackFlg))
            {
                if (m_swordMoveStiffnessTime < 0)
                {
                    if (m_swordMoveTime >= 0 && !m_manager.Getm_hitFlg)
                    {
                        moveFlg = true;

                        // Y���̉�]�ɍ��킹�Ĉړ��������v�Z����
                        rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
                        movement = rotationDirection * (m_leftRightSpeed * m_swordMoveAcceleration);
                        // �ړ�������0�łȂ��ꍇ�ɃI�u�W�F�N�g�̌�����ύX����
                        if (movement != Vector3.zero)
                        {
                            transform.forward = movement.normalized;
                        }
                    }
                }
            }

            if(moveFlg)
            {
                newPosition = transform.position + movement * Time.deltaTime;
                if (Physics.Raycast(transform.position, movement, out hit, movement.magnitude * Time.deltaTime))
                {
                    newPosition = hit.point;
                }
                // Rigidbody���g���ăL�����N�^�[���ړ�
                m_rb.MovePosition(newPosition);
                m_rb.interpolation = RigidbodyInterpolation.Interpolate;
            }

            // �|�������̎��Ԍo�ߊ֌W�B
            if (m_bowShotFlg)
            {
                m_bowShotTime -= Time.deltaTime;
                if (m_bowShotTime <= 0)
                {
                    m_bowClass.Shot();
                    m_bowShotFlg = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        // ������ԃ��[�V�����̏d���h�~
        if (m_blownAwayAnimeFlg)
        {
            m_blownAwayAnimeFlg = false;
        }

        if(m_downAnimeFlg)
        {
            m_downAnimeFlg = false;
        }

        if(m_pushUpAnimeFlg)
        {
            m_pushUpAnimeFlg = false;
        }

        // �_���[�W���[�V�����̏d���h�~
        if (m_damageAnimeFlg)
        {
            m_damageAnimeFlg = false;
        }
    }

    // �_���[�W���󂯂��Ƃ��̔ėp����
    public void Hit(int _damage, bool _knockBack, bool _down, bool _pushup)
    {
        // �_���[�W���[�V�������△�G���A�Q�[���N���A���̓_���[�W���󂯂Ȃ�
        if (!m_damageMotionFlg && !m_damageBlownAwayFlg && !m_invincibleFlg && !m_joyFlg)
        {
            m_playerLife -= _damage;
            float ratio = (float)m_playerLife / (float)m_playerMaxLife;
            m_hpGage.fillAmount = ratio;
            if (m_playerLife <= m_playerDangerLife)
            {
                m_hpGage.color = Color.red;
            }
            else if (m_playerLife <= m_playerCautionLife)
            {
                m_hpGage.color = Color.yellow;
            }

            if (m_playerLife > 0) // �_���[�W���󂯂đ̗͂�0�ȉ��ɂȂ�Ȃ���΃_���[�W���[�V���� + ���G���Ԕ���
            {
                // �_���[�W���󂯂����m�b�N�o�b�N����ꍇ
                if (_knockBack)
                {
                    m_blownAwayAnimeFlg = true;
                    m_damageBlownAwayFlg = true;
                    m_damageBlownAwayStiffnessFlg = true;
                    m_damageCoolTime = m_damageCoolSetTime;
                    m_blownAwayStiffnessTime = m_blownAwayStiffnessSetTime;
                }
                else if(_down) // �_���[�W���󂯂��Ƃ��ׂ����ꍇ
                {
                    m_downAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_downCoolSetTime;
                }
                else if(_pushup) // �_���[�W���󂯂����J�`�グ����ꍇ
                {
                    m_pushUpAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_pushUpCoolSetTime;
                }
                else // �_���[�W���󂯂����ɑ傫�ȃA�N�V���������Ȃ��ꍇ
                {
                    m_damageAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_damageCoolSetTime;
                }
                m_invincibilityTime = m_invincibilitySetTime;
            }
            else // �̗͂�0�ȉ��ɂȂ����ꍇ�Ɏ��S���ē������~�߂�
            {
                m_deathFlg = true;
                GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.PlayerDead);
                Debug.Log("��l�����S");
            }
        }
    }

    // Is Trigger���t���Ă���Collider�ɐڐG�����Ƃ��̏���
    void OnTriggerEnter(Collider _other)
    {
        // �_���[�W���[�V�������△�G���A�Q�[���N���A���̓_���[�W���󂯂Ȃ�
        if (!m_damageMotionFlg && !m_damageBlownAwayFlg && !m_invincibleFlg && !m_joyFlg)
        {
            if (_other.CompareTag("TreasureOpen"))
            {
                Setm_joyFlg();
            }
            else
            {
                // �G�ꂽ�I�u�W�F�N�g��Transform���擾
                Transform targetTransform = _other.transform;

                // �I�u�W�F�N�g�����݂��邩���m�F
                if (targetTransform != null)
                {
                    // �U�����s�����I�u�W�F�N�g�̈ʒu����U�����󂯂��I�u�W�F�N�g�̈ʒu�������āA�U�����󂯂������̃x�N�g�����v�Z
                    m_KnockBackVec = transform.position - targetTransform.position;
                }
                else
                {
                    // �I�u�W�F�N�g������
                    Debug.Log("���������I�u�W�F�N�g�������B");
                }
            }
        }
    }

    void Cure(int _recover)
    {
        m_playerLife += _recover;
        if(m_playerLife > m_playerMaxLife)
        {
            m_playerLife = m_playerMaxLife;
        }

        float ratio = (float)m_playerLife / (float)m_playerMaxLife;
        m_hpGage.fillAmount = ratio;
        if(m_playerLife > m_playerCautionLife)
        {
            m_hpGage.color = Color.green;
        }
        else if(m_playerLife > m_playerDangerLife)
        {
            m_hpGage.color = Color.yellow;
        }
    }

    // ���[�V�����d���h�~�p�̃t���O�Ǘ��n����
    void MotionProcessing()
    {
        // ����s�����[�V�����̏d���h�~
        if (m_rollAnimeFlg)
        {
            m_rollAnimeFlg = false;
        }

        // �U�����[�V�����̏d���h�~
        if (m_attackAnimeFlg)
        {
            m_attackAnimeFlg = false;
        }

        // 2�i�U�����[�V�����̏d���h�~
        if (m_secondSwordAttackAnimeFlg)
        {
            m_secondSwordAttackAnimeFlg = false;
        }

        // ��у��[�V�����̏d���h�~
        if (m_joyAnimeFlg) 
        {
            m_joyAnimeFlg = false;
        }
    }

    // ���Ԍo�߂Ŕ���������t���O��ύX���鏈��
    void TimeProcessing()
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
                m_swordMotionFlg = false;
                m_secondSwordMotionFlg = false;
                m_bowMotionFlg = false;
                m_subAttackMotionFlg = false;
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

    public bool Getm_walkAnimeFlg
    {
        get { return m_walkAnimeFlg; }
    }
    public bool Getm_rollAnimeFlg
    {
        get { return m_rollAnimeFlg; }
    }
    public bool Getm_weaponFlg
    {
        get { return m_weaponFlg; }
    }
    public bool Getm_attackAnimeFlg
    {
        get { return m_attackAnimeFlg; }
    }
    public bool Getm_secondSwordAttackAnimeFlg
    {
        get { return m_secondSwordAttackAnimeFlg; }
    }
    public bool Getm_damageAnimeFlg
    {
        get { return m_damageAnimeFlg; }
    }
    public bool Getm_blownAwayAnimeFlg
    {
        get { return m_blownAwayAnimeFlg; }
    }
    public bool Getm_downAnimeFlg
    {
        get { return m_downAnimeFlg; }
    }
    public bool Getm_pushUpAnimeFlg
    {
        get { return m_pushUpAnimeFlg; }
    }
    public bool Getm_deathFlg
    {
        get { return m_deathFlg; }
    }

    public bool Getm_swordMoveFlg
    {
        get { return m_swordMoveFlg; }
    }

    public bool Getm_secondSwordAttackFlg
    {
        get { return m_secondSwordAttackFlg; }
    }

    public bool Getm_joyFlg
    {
        get { return m_joyFlg; }
    }

    public bool Getm_joyAnimeFlg
    {
        get { return m_joyAnimeFlg; }
    }

    public int GetLife()
    {
        return m_playerLife;
    }

    void Setm_joyFlg()
    {
        m_joyFlg = true;
        m_joyAnimeFlg = true;
        GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.TreasureGet);
    }
}