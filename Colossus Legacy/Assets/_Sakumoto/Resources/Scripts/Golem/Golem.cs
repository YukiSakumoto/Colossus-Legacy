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

    protected bool m_stop = false;          // 両腕を同時に合わせるためのフラグ
    protected bool m_attackWait = false;

    [SerializeField] private int m_maxHp = 100;
    public int m_hp;               // ゴーレムの体力
    [SerializeField] private Image m_hpGage;

    protected bool m_damageFlg = false;     // 攻撃を受けた際のフラグ
    [SerializeField] private float m_damageTime = 0.0f;
    [SerializeField] public int m_damagePoint = 0;

    [SerializeField] private float m_time = 0.0f;
    [SerializeField] protected float m_dist = 0.0f;

    private bool m_lastAttack = false;
    [SerializeField] protected bool m_alive = true;
    [SerializeField] protected bool m_enable = true;

    // ======================
    // ディゾルブ処理用
    // ======================
    protected SkinMesh m_skinMesh;
    protected Dissolve m_dissolve;
    [SerializeField] private float m_dissolveSpeed = 0.1f;
    private float m_dissolveRatio = 0.0f;


    // ======================
    // カメラを揺らすよ
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
                        //自分の子供をDestroyする
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

        // ダメージの処理を先に持ってくる（早期リターン）
        if (!m_damageFlg)
        {
            // どちらかの部位がダメージを受けた状態なら全ての部位をダメージ状態にする
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

                // ダメージアニメーションを終了する処理
                if (m_time <= 0.0f)
                {
                    m_damageFlg = false;

                    if (m_golemLeft) m_golemLeft.WakeUp();
                    if (m_golemRight) m_golemRight.WakeUp();
                    if (m_golemMain) m_golemMain.WakeUp();
                }
            }
        }

        // 両方の腕がある時の処理
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
        // HPを減らすよ
        m_hp -= m_damagePoint;

        float ratio = (float)m_hp / (float)m_maxHp;
        m_hpGage.fillAmount = ratio;

        m_damageFlg = true;

        // ダメージアニメーションの再生
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


    // ゴーレムの弱点が攻撃された時の処理
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


    // ターゲットとの距離を取得して返す
    public float DistanceToTarget()
    {
        if (m_target || m_myself)
        {
            Vector3 targetPos = m_target.transform.position;
            Vector3 myPos = m_myself.transform.position;

            m_dist = Vector3.Distance(targetPos, myPos);
        }

        // デバッグ用
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


    // 攻撃判定生成
    private void AttackOn()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = true;
        }
    }


    // 攻撃判定消去
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


    // 弱点判定発生
    protected void WeakOn()
    {
        if (!m_weakCollider) { return; }
        Collider col = m_weakCollider.GetComponent<Collider>();
        col.enabled = true;
    }


    // 弱点判定消去
    protected void WeakOff()
    {
        if (!m_weakCollider) { return; }
        Collider col = m_weakCollider.GetComponent<Collider>();
        col.enabled = false;
    }


    // 攻撃を受けた際の処理
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


    // 攻撃から起き上がる処理
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


    // アニメーション終了時に読み込み
    private void ResetAnimation()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ResetAnimation();
    }


    // 死亡処理
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
