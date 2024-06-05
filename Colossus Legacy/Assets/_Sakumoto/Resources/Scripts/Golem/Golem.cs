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
    [SerializeField] protected Collider m_weakCollider;

    [SerializeField] private GolemLeft m_golemLeft;
    [SerializeField] private GolemRight m_golemRight;
    [SerializeField] private GolemMain m_golemMain;

    [SerializeField] protected GameObject m_myself;
    [SerializeField] protected GameObject m_target;

    [SerializeField] private TMPro.TMP_Text m_text;

    protected bool m_stop = false;          // 両腕を同時に合わせるためのフラグ

    [SerializeField] protected int m_hp = 100;               // ゴーレムの体力

    protected bool m_damageFlg = false;     // 攻撃を受けた際のフラグ
    [SerializeField] private float m_damageTime = 0.0f;
    [SerializeField] private int m_damagePoint = 0;

    private float m_time = 0.0f;

    private bool m_lastAttack = false;
    protected bool m_alive = true;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        m_golemLeft = GameObject.Find("Golem_Left").GetComponent<GolemLeft>();
        m_golemRight = GameObject.Find("Golem_Right").GetComponent<GolemRight>();
        m_golemMain = GameObject.Find("Golem_Main").GetComponent<GolemMain>();
    }

    void Update()
    {
        if (!m_alive) { return; }

        // Null チェック
        if (!m_golemLeft || !m_golemRight || !m_golemMain)
        {
            if (!m_golemLeft) { Debug.Log("LNull!!"); }
            if (!m_golemRight) { Debug.Log("RNull!!"); }
            if (!m_golemMain) { Debug.Log("MNull!!"); }

            return;
        }

        // ダメージの処理を先に持ってくる（早期リターン）
        if (!m_damageFlg)
        {
            // どちらかの部位がダメージを受けた状態なら全ての部位をダメージ状態にする
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                DamageAction();

                return;
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

                            m_golemMain.ArmorDestroy();
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

                    m_golemLeft.WakeUp();
                    m_golemRight.WakeUp();
                    m_golemMain.WakeUp();
                }
            }
        }


        // 
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

        if (m_golemLeft.GetStop() && m_golemRight.GetStop())
        {
            m_golemLeft.AttackStart();
            m_golemRight.AttackStart();
        }
    }


    public void DamageAction()
    {
        // HPを減らすよ
        m_hp -= m_damagePoint;

        m_damageFlg = true;

        // ダメージアニメーションの再生
        m_golemLeft.HitDamage();
        m_golemRight.HitDamage();
        m_golemMain.HitDamage();

        m_time = m_damageTime;

        if (m_hp <= 1)
        {
            if (!m_lastAttack)
            {
                m_hp = 1;
            }
            else
            {
                Debug.Log("ステージクリア！");
                m_hp = 0;
                m_alive = false;
            }

            m_time = 3.0f;
        }
    }


    // ターゲットとの距離を取得して返す
    public float DistanceToTarget()
    {
        float dist = 0.0f;

        if (m_target || m_myself)
        {
            Vector3 targetPos = m_target.transform.position;
            Vector3 myPos = m_myself.transform.position;

            dist = Vector3.Distance(targetPos, myPos);
        }

        // デバッグ用
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
        m_stop = false;
        attackManager.AnimationFin();

        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
        }
    }

    protected void WeakOn()
    {
        m_weakCollider.enabled = true;
    }

    protected void WeakOff()
    {
        m_weakCollider.enabled = false;
    }


    // 攻撃を受けた際の処理
    public void HitDamage()
    {
        m_stop = true;
        m_damageFlg = true;
        attackManager.ChangeAnimation("Damage", m_damageFlg);
    }


    // 攻撃から起き上がる処理
    public void WakeUp()
    {
        m_stop = false;
        m_damageFlg = false;
        attackManager.ChangeAnimation("Damage", m_damageFlg);
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
}
