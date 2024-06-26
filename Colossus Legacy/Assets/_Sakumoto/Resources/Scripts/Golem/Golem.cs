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
    //bool m_skimEnable = true;

    protected AttackManager attackManager;
    [SerializeField] private List<Collider> attackColliders;
    [SerializeField] protected WeakPoint m_weakCollider;

    private GolemLeft m_golemLeft;
    private GolemRight m_golemRight;
    private GolemMain m_golemMain;

    [SerializeField] protected GameObject m_myself;
    [SerializeField] protected GameObject m_target;

    [SerializeField] private TMPro.TMP_Text m_text;

    protected bool m_stop = false;          // ���r�𓯎��ɍ��킹�邽�߂̃t���O
    protected bool m_attackWait = false;

    [SerializeField] private int m_maxHp = 100;
    public int m_hp;               // �S�[�����̗̑�
    [SerializeField] private Image m_hpGage;

    protected bool m_damageFlg = false;     // �U�����󂯂��ۂ̃t���O
    [SerializeField] private float m_damageTime = 0.0f;
    [SerializeField] public int m_damagePoint = 0;

    [SerializeField] private float m_time = 0.0f;
    [SerializeField] protected float m_dist = 0.0f;

    private bool m_lastAttack = false;
    [SerializeField] protected bool m_alive = true;
    [SerializeField] protected bool m_enable = true;

    // ======================
    // �f�B�]���u�����p
    // ======================
    protected SkinMesh m_skinMesh;
    protected Dissolve m_dissolve;
    [SerializeField] private float m_dissolveSpeed = 0.1f;
    private float m_dissolveRatio = 0.0f;


    // ======================
    // �J������h�炷��
    // ======================
    protected CameraQuake m_camera;


    void Start()
    {
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
        if (!m_alive)
        {
            if (m_golemLeft) m_golemLeft.PartsDestroy();
            if (m_golemRight) m_golemRight.PartsDestroy();
            if (m_golemMain) 
                if (m_golemMain.PartsDestroy())
                {
                    foreach (Transform child in this.transform)
                    {
                        //�����̎q����Destroy����
                        Destroy(child.gameObject);
                    }
                }

            return;
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

        // �_���[�W�̏������Ɏ����Ă���i�������^�[���j
        if (!m_damageFlg)
        {
            // �ǂ��炩�̕��ʂ��_���[�W���󂯂���ԂȂ�S�Ă̕��ʂ��_���[�W��Ԃɂ���
            if (m_golemLeft)
            {
                if (m_golemLeft.m_damageFlg)
                {
                    m_golemLeft.m_alive = false;
                    DamageAction();
                }
            }
            if (m_golemRight)
            {
                if (m_golemRight.m_damageFlg)
                {
                    m_golemRight.m_alive = false;
                    DamageAction();
                }
            }
            if (m_golemMain)
            {
                if (m_golemMain.m_damageFlg)
                {
                    m_golemMain.m_alive = false;
                    DamageAction();
                }
            }
        }
        else
        {
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                m_time -= Time.deltaTime;

                if (m_hp <= 1)
                {
                    if (m_time <= 0.0f)
                    {
                        if (!m_lastAttack)
                        {
                            m_lastAttack = true;

                            m_golemMain.WakeUp();
                        }                        

                        m_golemMain.SpecialAttack();
                        if (m_golemMain.m_damageFlg)
                        {
                            DamageAction();
                        }
                    }
                    return;
                }

                // �_���[�W�A�j���[�V�������I�����鏈��
                if (m_time <= 0.0f)
                {
                    m_damageFlg = false;

                    if (m_golemLeft) m_golemLeft.WakeUp();
                    if (m_golemRight) m_golemRight.WakeUp();
                    if (m_golemMain) m_golemMain.WakeUp();
                }
            }
        }

        // �����̘r�����鎞�̏���
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


    public void DamageAction()
    {
        // HP�����炷��
        m_hp -= m_damagePoint;

        float ratio = (float)m_hp / (float)m_maxHp;
        m_hpGage.fillAmount = ratio;

        m_damageFlg = true;

        // �_���[�W�A�j���[�V�����̍Đ�
        if (m_golemLeft) m_golemLeft.HitDamage();
        if (m_golemRight) m_golemRight.HitDamage();
        if (m_golemMain) m_golemMain.HitDamage();

        m_time = m_damageTime;

        if (m_hp <= 1)
        {
            if (!m_lastAttack)
            {
                m_hp = 1;

                if (m_golemLeft) m_golemLeft.m_alive = false;
                if (m_golemRight) m_golemRight.m_alive = false;
            }
            else
            {
                m_hp = 0;

                m_alive = false;
                m_golemMain.m_alive = false;
            }

            m_time = 3.0f;
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


    public int AttackSet(float _dist, int _id = -1)
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
    public void HitDamage()
    {
        m_stop = true;
        //m_damageFlg = true;
        attackManager.ResetAnimation();
        attackManager.ChangeAnimation("Damage", true);

        if (m_weakCollider)
        {
            m_weakCollider.m_weakHit = false;
        }
    }


    // �U������N���オ�鏈��
    public void WakeUp()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ResetAnimation();
        attackManager.ChangeAnimation("Damage", false);
    }


    private void ResetAttack()
    {
        attackManager.ResetAttackAnimation();
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
}
