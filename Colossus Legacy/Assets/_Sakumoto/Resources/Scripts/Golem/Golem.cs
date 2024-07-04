using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditorInternal;

public class Golem : MonoBehaviour
{
    ~Golem() { }

    // �U���E��_�X�N���v�g�̃A�^�b�`
    protected AttackManager attackManager;                      // �U���Ǘ��X�N���v�g
    [SerializeField] private List<Collider> attackColliders;    // �U������
    [SerializeField] protected WeakPoint m_weakCollider;        // ��_����X�N���v�g

    // �����Ǘ�
    private enum HpState
    {
        Max,        // ���C�I
        Half,       // �c�蔼��
        Crisis,     // �s���`�I
        Dead        // ���S�c
    }
    [SerializeField] HpState m_hpState = HpState.Max;                        // �̗͏��
    protected bool m_alive = true;                          // �e�p�[�c�̐����t���O
    [SerializeField] protected bool m_enable = true;        // �q�G�����L�[�L���Ǘ��t���O

    // �e�p�[�c���
    private GolemLeft m_golemLeft;      // ���r
    private GolemRight m_golemRight;    // �E�r
    private GolemMain m_golemMain;      // �{��
    protected string m_nowAttackName = "";   // �e�p�[�c�̌��݂̍U����

    // �^�[�Q�b�g�ւ̊p�x�E�����w��p
    [SerializeField] protected GameObject m_myself;     // ����
    [SerializeField] protected GameObject m_target;     // �^�[�Q�b�g
    [SerializeField] protected float m_dist = 0.0f;     // �^�[�Q�b�g�܂ł̋���
    // �f�o�b�O�p
    [SerializeField] private TMPro.TMP_Text m_text;

    // �U���Ǘ��p
    [SerializeField] protected bool m_stop = false;          // �e�p�[�c�̏������~�߂�t���O
    protected bool m_attackWait = false;    // �U���ҋ@��ԃt���O

    // HP ����
    [SerializeField] private int m_maxHp = 100;     // �ő�̗�
    [SerializeField] private int m_hp;              // ���݂̗̑�
    [SerializeField] private Image m_hpGage;        // HPUI
    private float m_damageRatio = 1.0f;             // �_���[�W�̌����l


    // ��_���[�W�֘A
    [SerializeField] protected bool m_damageFlg = false;                     // �U�����󂯂��ۂ̃t���O
    [SerializeField] private float m_damageTime = 0.0f;     // �����܂ł̎���
    [SerializeField] public int m_damagePoint = 0;          // �_���[�W��
    [SerializeField] private float m_time = 0.0f;           // �o�ߎ��ԊǗ�


    // ======================
    // �f�B�]���u�����p
    // ======================
    protected SkinMesh m_skinMesh;                          // �e
    protected Dissolve m_dissolve;                          // �f�B�]���u
    [SerializeField] private float m_dissolveSpeed = 0.1f;  // �f�B�]���u�����������鎞��
    private float m_dissolveRatio = 0.0f;                   // �f�B�]���u�����̐i�s����


    // ======================
    // �J������h�炷��
    // ======================
    protected CameraQuake m_camera;     // �J������h�炷�X�N���v�g


    void Start()
    {
        GameEvent.Instance.Reset();

        m_skinMesh = GetComponent<SkinMesh>();
        m_dissolve = GetComponent<Dissolve>();

        m_camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraQuake>();

        attackManager = GetComponent<AttackManager>();

        m_golemLeft = GameObject.Find("Golem_Left").GetComponent<GolemLeft>();
        m_golemRight = GameObject.Find("Golem_Right").GetComponent<GolemRight>();
        m_golemMain = GameObject.Find("Golem_Main").GetComponent<GolemMain>();

        m_hp = m_maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { DamageAction(50); m_golemLeft.m_alive = false; m_golemRight.m_damageFlg = true; m_golemMain.m_damageFlg = true; }
        if (Input.GetKeyDown(KeyCode.F2)) { DamageAction(50); m_golemRight.m_alive = false; m_golemLeft.m_damageFlg = true; m_golemMain.m_damageFlg = true; }
        if (Input.GetKeyDown(KeyCode.F3)) { DamageAction(50); m_golemMain.m_alive = false; m_golemRight.m_alive = false; m_golemMain.m_alive = false; }

        // HP ��������
        HpDown();
        if (m_hpState == HpState.Dead)
        {
            if (!m_golemLeft && !m_golemRight && !m_golemMain)
            {
                foreach (Transform child in this.transform)
                {
                    Debug.Log("�I���");
                    //�����̎q����Destroy����
                    Destroy(child.gameObject);
                }
                return;
            }
        }

        if (m_golemLeft)
        {
            if (!m_golemLeft.m_enable)
            {
                Destroy(m_golemLeft.gameObject);
                if (m_golemRight)
                {
                    m_golemRight.attackManager.DeleteAll();
                    m_golemRight.attackManager.AddAttack(9, "Rampage", new Vector2(0.1f, 55.0f), 0.0f);
                }
            }
        }
        if (m_golemRight)
        {
            if (!m_golemRight.m_enable)
            {
                Destroy(m_golemRight.gameObject);
                if (m_golemLeft)
                {
                    m_golemLeft.attackManager.DeleteAll();
                    m_golemLeft.attackManager.AddAttack(9, "Rampage", new Vector2(0.1f, 55.0f), 0.0f);
                }
            }
        }
        if (m_golemMain)
        {
            if (!m_golemMain.m_enable)
            {
                Destroy(m_golemMain.gameObject);
            }
        }

        // =======================================================
        // �_���[�W�̏������Ɏ����Ă���i�������^�[���j
        // =======================================================
        if (!m_damageFlg)
        {
            // �ǂ��炩�̕��ʂ��_���[�W���󂯂���ԂȂ�S�Ă̕��ʂ��_���[�W��Ԃɂ���
            if (m_golemLeft)
            {
                if (m_golemLeft.m_damageFlg)
                {
                    DamageAction(m_golemLeft.m_damagePoint);
                    if (m_hpState != HpState.Max) { m_golemLeft.m_alive = false; }
                    return;
                }
            }
            if (m_golemRight)
            {
                if (m_golemRight.m_damageFlg)
                {
                    DamageAction(m_golemRight.m_damagePoint);
                    if (m_hpState != HpState.Max) { m_golemRight.m_alive = false; }
                    return;
                }
            }
            if (m_golemMain)
            {
                if (m_golemMain.m_damageFlg)
                {
                    DamageAction(m_golemMain.m_damagePoint);
                    if (m_hpState == HpState.Dead) { m_golemMain.m_alive = false; }
                    return;
                }
            }
        }
        // �_���[�W��Ԃ��畜������܂ł̏���
        else
        {
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                m_time -= Time.deltaTime;

                // �S�[�������s���`��ԂȂ��Z�����
                if (m_hpState == HpState.Crisis)
                {
                    if (m_time <= 0.0f)
                    {
                        m_time = 0.0f;
                        m_golemMain.SpecialAttack();
                        if (m_golemMain.m_damageFlg)
                        {
                            DamageAction(1);
                        }
                    }
                    else
                    {
                        m_golemMain.WakeUp();
                    }
                    return;
                }

                // �_���[�W�A�j���[�V�������I�����鏈��
                if (m_time <= 0.0f)
                {
                    m_time = 0.0f;

                    m_damageFlg = false;

                    if (m_golemLeft) m_golemLeft.WakeUp();
                    if (m_golemRight) m_golemRight.WakeUp();
                    if (m_golemMain) m_golemMain.WakeUp();
                }
            }
        }


        // ===============================================
        // �����̘r�����鎞�̍U�������i�����U���j
        // ===============================================
        if (m_golemLeft && m_golemRight)
        {
            if (m_golemLeft.AttackWait())
            {
                if (m_golemLeft.m_nowAttackId == 2)
                {
                    m_golemRight.AttackWait();
                    m_golemRight.SetNextAttackId(2);
                }
            }
            if (m_golemRight.AttackWait())
            {
                if (m_golemRight.m_nowAttackId == 2)
                {
                    m_golemLeft.AttackWait();
                    m_golemLeft.SetNextAttackId(2);
                }
            }

            if (m_golemLeft.m_attackWait && m_golemRight.m_attackWait)
            {
                m_golemLeft.AttackStart();
                m_golemRight.AttackStart();
            }
        }
    }


    private void HpDown()
    {
        if (m_hpGage.fillAmount >= m_damageRatio)
        {
            m_hpGage.fillAmount -= 0.2f * Time.deltaTime;
        }
    }


    private void DamageAction(int _damage)
    {
        // HP�����炷��
        m_hp -= _damage;
        if (m_hpState == HpState.Max)
        {
            m_time = m_damageTime;
            if (m_hp <= m_maxHp / 2.0f)
            {
                m_hpState = HpState.Half;
            }
        }
        else if (m_hpState == HpState.Half)
        {
            if (m_hp <= 1)
            {
                m_hp = 1;
                m_hpState = HpState.Crisis;
                m_time = 3.0f;
            }
        }
        else if (m_hpState == HpState.Crisis)
        {
            m_hp = 0;
            m_hpState = HpState.Dead;
            m_golemMain.m_alive = false;
        }
        m_damageRatio = (float)m_hp / (float)m_maxHp;

        m_damageFlg = true;

        // �_���[�W�A�j���[�V�����̍Đ�
        if (m_golemLeft) m_golemLeft.HitDamage();
        if (m_golemRight) m_golemRight.HitDamage();
        if (m_golemMain) m_golemMain.HitDamage();
    }


    // �S�[�����̎�_���U�����ꂽ���̏���
    protected void WeakHit(int _damage)
    {
        if (!m_weakCollider) { return; }

        if (m_weakCollider.m_weakHit)
        {
            m_damageFlg = true;
            m_damagePoint = _damage;
        }
    }


    public void SetHit(int _damage)
    {
        if (m_golemLeft) m_golemLeft.WeakHit(_damage);
        if (m_golemRight) m_golemRight.WeakHit(_damage);
        if (m_golemMain) m_golemMain.WeakHit(_damage);
    }


    // �^�[�Q�b�g�Ƃ̋������擾���ĕԂ�
    public float DistanceToTarget()
    {
        if (m_target || m_myself)
        {
            Vector3 targetPos = m_target.transform.position;
            Vector3 myPos = m_myself.transform.position;

            m_dist = Vector3.Distance(targetPos, myPos);
        }

        // �f�o�b�O�p
        if (m_text) m_text.text = "Dist : " + m_dist.ToString("#.###");

        return m_dist;
    }


    protected int AttackSet(float _dist, int _id = -1)
    {
        int resultId = -1;

        if (_id == -1)
        {
            resultId = attackManager.Action(_dist);
        }
        else
        {
            resultId = attackManager.Action(_dist, _id);
        }
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
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
        }
    }

    private void AttackAnimFin()
    {
        m_stop = false;
        m_attackWait = false;
        attackManager.AnimationFin();
    }


    // ��_���蔭��
    protected void WeakOn()
    {
        if (!m_weakCollider) { return; }
        Collider col = m_weakCollider.GetComponent<Collider>();
        col.enabled = true;
    }


    // ��_�������
    protected void WeakOff()
    {
        if (!m_weakCollider) { return; }
        Collider col = m_weakCollider.GetComponent<Collider>();
        col.enabled = false;
    }


    // �U�����󂯂��ۂ̏���
    protected void HitDamage()
    {
        attackManager.ResetAttackAnimation();
        attackManager.ChangeAnimation("Damage", true);

        if (m_weakCollider)
        {
            m_weakCollider.m_weakHit = false;
        }
    }


    // �U������N���オ�鏈��
    protected void WakeUp()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ResetAnimation();
    }


    private void ResetAttack()
    {
        attackManager.ResetAttackAnimation();
    }


    // �A�j���[�V�����I�����ɓǂݍ���
    private void ResetAnimation()
    {
        m_stop = false;
        m_attackWait = false;
        attackManager.ResetAnimation();
    }


    // ���S����
    public void Death()
    {
        Destroy(this.gameObject);
    }


    public bool GetStop() { return m_stop; }


    protected bool PartsDestroy()
    {
        if (!m_dissolve) { return false; }

        m_dissolveRatio += m_dissolveSpeed * Time.deltaTime;
        m_dissolve.SetDissolveAmount(m_dissolveRatio);

        if (m_dissolveRatio >= 0.6f)
        {
            m_enable = false;
        }

        if (!m_skinMesh) { return false; }
        m_skinMesh.SetSkinMeshShadow(false);

        return true;
    }


    protected void CameraQuakingShort()
    {
        m_camera.StartShake(0.5f, 0.3f, 10.0f);
    }


    protected void CameraQuakingLong()
    {
        m_camera.StartShake(1.0f, 0.75f, 10.0f);
    }


    public string GetNowAttackName()
    {
        return m_nowAttackName;
    }
}
