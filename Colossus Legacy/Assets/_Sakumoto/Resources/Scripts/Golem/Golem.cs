using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditorInternal;
#endif
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Golem : MonoBehaviour
{
    ~Golem() { }

    // �U���E��_�X�N���v�g�̃A�^�b�`
    protected AttackManager attackManager;                      // �U���Ǘ��X�N���v�g
    [SerializeField] private List<Collider> attackColliders;    // �U������
    [SerializeField] protected WeakPoint m_weakCollider;        // ��_����X�N���v�g
    [SerializeField] protected LightWeak m_weakLight;

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
    protected GameObject m_attackAreaIns;

    // �^�[�Q�b�g�ւ̊p�x�E�����w��p
    [SerializeField] protected GameObject m_myself;     // ����
    [SerializeField] protected GameObject m_target;     // �^�[�Q�b�g
    [SerializeField] protected float m_dist = 0.0f;     // �^�[�Q�b�g�܂ł̋���
    // �f�o�b�O�p
    [SerializeField] private TMPro.TMP_Text m_text;

    // �U���Ǘ��p
    [SerializeField] protected bool m_stop = false;          // �e�p�[�c�̏������~�߂�t���O
    [SerializeField] protected bool m_attackWait = false;    // �U���ҋ@��ԃt���O
    [SerializeField] protected bool m_palmsFlg = false;

    // �U���̉�
    private int m_damageCnt = 0;
    [SerializeField] protected int m_attackCnt = 0;
    [SerializeField] protected int m_palmsMinCnt = 2;


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


    // ��Փx�֘A
    protected float m_attackSpeed = 1.0f;
    private float m_firstSpeed = 1.0f;
    private float m_secondSpeed = 1.0f;
    private float m_ranpageSpeed = 1.0f;


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
        m_damageCnt = 0;


        // ��Փx
        if (GameManager.Instance.m_difficulty == GameManager.Difficulty.Easy)
        {
            m_firstSpeed = 1.1f;
            m_secondSpeed = 1.2f;
            m_ranpageSpeed = 1.3f;
        }
        else if (GameManager.Instance.m_difficulty == GameManager.Difficulty.Hard)
        {
            m_firstSpeed = 1.2f;
            m_secondSpeed = 1.4f;
            m_ranpageSpeed = 1.5f;
        }
        else if (GameManager.Instance.m_difficulty == GameManager.Difficulty.SuperHard)
        {
            m_firstSpeed = 1.3f;
            m_secondSpeed = 1.5f;
            m_ranpageSpeed = 2.0f;
        }
    }

    void Update()
    {
        if (GameStatusManager.Instance.m_debugFlg)
        {
            if (Input.GetKeyDown(KeyCode.F1)) { DamageAction(50); m_golemLeft.m_alive = false; m_golemRight.m_damageFlg = true; m_golemMain.m_damageFlg = true; }
            if (Input.GetKeyDown(KeyCode.F2)) { DamageAction(50); m_golemRight.m_alive = false; m_golemLeft.m_damageFlg = true; m_golemMain.m_damageFlg = true; }
            if (Input.GetKeyDown(KeyCode.F3)) { DamageAction(100); m_hp = 0; m_golemMain.m_alive = false; m_golemRight.m_alive = false; m_golemLeft.m_alive = false; }
        }

        // HP ��������
        HpDown();
        if (m_hpState == HpState.Dead)
        {
            if (!m_golemLeft && !m_golemRight && !m_golemMain)
            {
                foreach (Transform child in this.transform)
                {
                    // Debug.Log("�I���");
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
                    if (m_hpState != HpState.Max)
                    {
                        if (m_hpState == HpState.Half)
                        {
                            if (m_golemRight) { m_golemLeft.m_alive = false; }
                        }
                        else if (m_hpState == HpState.Crisis)
                        {
                            m_golemLeft.m_alive = false;
                        }
                    }
                    // �U����ǉ�
                    else
                    {
                        ChangeAttackState();
                    }
                    return;
                }
            }
            if (m_golemRight)
            {
                if (m_golemRight.m_damageFlg)
                {
                    DamageAction(m_golemRight.m_damagePoint);
                    if (m_hpState != HpState.Max)
                    {
                        if (m_hpState == HpState.Half)
                        {
                            if (m_golemLeft) { m_golemRight.m_alive = false; }
                            else { ChangeAttackState(); }
                        }
                        else if (m_hpState == HpState.Crisis)
                        {
                            m_golemRight.m_alive = false;
                        }
                    }
                    else
                    {
                        ChangeAttackState();
                    }
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
            if (!m_palmsFlg)
            {
                if (m_golemLeft.m_nowAttackId == 0 && m_golemRight.m_nowAttackId != 0)
                {
                    m_palmsFlg = true;
                    m_golemRight.SetNextAttackId(0);
                    m_golemRight.m_palmsFlg = true;
                    // Debug.Log("�݂��Ă�" + m_golemRight.m_palmsFlg);
                }
                else if (m_golemRight.m_nowAttackId == 0 && m_golemLeft.m_nowAttackId != 0)
                {
                    m_palmsFlg = true;
                    m_golemLeft.SetNextAttackId(0);
                    m_golemLeft.m_palmsFlg = true;
                    // Debug.Log("�Ђ���Ă�" + m_golemRight.m_palmsFlg);
                }
                else
                {
                    m_golemLeft.m_palmsFlg = false;
                    m_golemRight.m_palmsFlg = false;
                }
            }
            else if (m_palmsFlg && m_golemLeft.m_attackWait && m_golemRight.m_attackWait)
            {
                m_palmsFlg = false;
                m_golemLeft.m_palmsFlg = false;
                m_golemRight.m_palmsFlg = false;

                Invoke(nameof(PalmsStart), 0.75f);
            }
        }
    }


    private void PalmsStart()
    {
        m_golemLeft.AttackStart();
        m_golemRight.AttackStart();
    }


    private void ChangeAttackState()
    {
        // �U���񐔂ɉ����čU���p�^�[����ω�������
        if (m_hpState == HpState.Max)
        {
            if (m_damageCnt == 1)
            {
                m_golemLeft.attackManager.AddAttack(3, "SwingDown", new Vector2(0.0f, 22.0f), 1.5f);
                m_golemLeft.m_attackCnt = 0;
                m_golemLeft.m_palmsMinCnt = 3;
                m_golemLeft.m_attackSpeed = m_firstSpeed;
                m_golemLeft.attackManager.SetAttackSpeed(m_golemLeft.m_attackSpeed);

                m_golemRight.attackManager.AddAttack(3, "SwingDown", new Vector2(0.0f, 22.0f), 1.5f);
                m_golemRight.m_attackCnt = 0;
                m_golemRight.m_palmsMinCnt = 3;
                m_golemRight.m_attackSpeed = m_firstSpeed;
                m_golemRight.attackManager.SetAttackSpeed(m_golemRight.m_attackSpeed);
            }
            else if (m_damageCnt == 2)
            {
                m_golemLeft.attackManager.DeleteAttack(1);
                m_golemLeft.attackManager.DeleteAttack(3);
                m_golemLeft.attackManager.AddAttack(1, "SwingDown", new Vector2(0.0f, 22.0f), 0.5f);
                m_golemLeft.attackManager.AddAttack(3, "SwingDown", new Vector2(0.0f, 22.0f), 0.5f);
                m_golemLeft.m_attackCnt = 0;
                m_golemLeft.m_palmsMinCnt = 5;
                m_golemLeft.m_attackSpeed = m_secondSpeed;
                m_golemLeft.attackManager.SetAttackSpeed(m_golemLeft.m_attackSpeed);

                m_golemRight.attackManager.DeleteAttack(1);
                m_golemRight.attackManager.DeleteAttack(2);
                m_golemRight.attackManager.AddAttack(1, "Protrusion", new Vector2(0.0f, 55.0f), 0.2f);
                m_golemRight.attackManager.AddAttack(2, "Protrusion", new Vector2(0.0f, 55.0f), 0.2f);
                m_golemRight.m_attackCnt = 0;
                m_golemRight.m_palmsMinCnt = 5;
                m_golemRight.m_attackSpeed = m_secondSpeed;
                m_golemRight.attackManager.SetAttackSpeed(m_golemRight.m_attackSpeed);
            }
        }
        else if (m_hpState == HpState.Half)
        {
            if (m_damageCnt == 0)
            {
                if (m_golemLeft)
                {
                    m_golemLeft.m_attackSpeed = 1.0f;
                    m_golemLeft.attackManager.SetAttackSpeed(m_attackSpeed);
                }
                if (m_golemRight)
                {
                    m_golemRight.m_attackSpeed = 1.0f;
                    m_golemRight.attackManager.SetAttackSpeed(m_attackSpeed);
                }
            }
            else if (m_damageCnt == 1)
            {
                if (m_golemLeft)
                {
                    m_golemLeft.m_attackSpeed = m_ranpageSpeed;
                    m_golemLeft.attackManager.SetAttackSpeed(m_attackSpeed);
                }
                if (m_golemRight)
                {
                    m_golemRight.m_attackSpeed = m_ranpageSpeed;
                    m_golemRight.attackManager.SetAttackSpeed(m_attackSpeed);
                }
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
                m_damageCnt = 0;
                m_attackSpeed = 1.0f;
                if (attackManager) { attackManager.SetAttackSpeed(m_attackSpeed); }
            }
            else
            {
                m_damageCnt++;
            }
        }
        else if (m_hpState == HpState.Half)
        {
            if (m_hp <= 1)
            {
                m_hp = 1;
                m_hpState = HpState.Crisis;
                m_time = 3.0f;
                m_damageCnt = 0;
                m_attackSpeed = 1.0f;
                if (attackManager) { attackManager.SetAttackSpeed(m_attackSpeed); }
            }
            else
            {
                m_damageCnt++;
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
        if (m_golemLeft)
        {
            m_golemLeft.HitDamage();
            m_golemLeft.ResetAttackSpeed();
            if (m_golemRight) m_golemRight.ResetAttackSpeed();
            m_golemLeft.DestroyAttackArea();
        }
        if (m_golemRight)
        {
            m_golemRight.HitDamage();
            m_golemRight.ResetAttackSpeed();
            if (m_golemLeft) m_golemLeft.ResetAttackSpeed();
            m_golemRight.DestroyAttackArea();
        }
        if (m_golemMain)
        {
            m_golemMain.HitDamage();
            m_golemMain.DestroyAttackArea();
        }
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
        m_attackCnt++;
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
        m_weakLight.enabled = true;
    }


    // ��_�������
    protected void WeakOff()
    {
        if (!m_weakCollider) { return; }
        Collider col = m_weakCollider.GetComponent<Collider>();
        col.enabled = false;
        m_weakLight.enabled = false;
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

    public int GetHp()
    {
        return m_hp;
    }


    protected void DebugAttackId(string _name = "")
    {
        for (int i = 0; i < attackManager.GetAttackIdList().Count; i++)
        {
            Debug.Log(_name + " : " + attackManager.GetAttackIdList()[i]);
        }
    }


    protected void DestroyAttackArea()
    {
        if (m_attackAreaIns)
        {
            Destroy(m_attackAreaIns);
        }
    }


    protected void ResetAttackSpeed()
    {
        m_attackSpeed = 1.0f;
    }
}