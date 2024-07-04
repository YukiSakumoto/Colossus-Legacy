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

    // 攻撃・弱点スクリプトのアタッチ
    protected AttackManager attackManager;                      // 攻撃管理スクリプト
    [SerializeField] private List<Collider> attackColliders;    // 攻撃判定
    [SerializeField] protected WeakPoint m_weakCollider;        // 弱点判定スクリプト

    // 生存管理
    private enum HpState
    {
        Max,        // 元気！
        Half,       // 残り半分
        Crisis,     // ピンチ！
        Dead        // 死亡…
    }
    [SerializeField] HpState m_hpState = HpState.Max;                        // 体力状態
    protected bool m_alive = true;                          // 各パーツの生存フラグ
    [SerializeField] protected bool m_enable = true;        // ヒエラルキー有効管理フラグ

    // 各パーツ情報
    private GolemLeft m_golemLeft;      // 左腕
    private GolemRight m_golemRight;    // 右腕
    private GolemMain m_golemMain;      // 本体
    protected string m_nowAttackName = "";   // 各パーツの現在の攻撃名

    // ターゲットへの角度・距離指定用
    [SerializeField] protected GameObject m_myself;     // 自分
    [SerializeField] protected GameObject m_target;     // ターゲット
    [SerializeField] protected float m_dist = 0.0f;     // ターゲットまでの距離
    // デバッグ用
    [SerializeField] private TMPro.TMP_Text m_text;

    // 攻撃管理用
    [SerializeField] protected bool m_stop = false;          // 各パーツの処理を止めるフラグ
    protected bool m_attackWait = false;    // 攻撃待機状態フラグ

    // HP 処理
    [SerializeField] private int m_maxHp = 100;     // 最大体力
    [SerializeField] private int m_hp;              // 現在の体力
    [SerializeField] private Image m_hpGage;        // HPUI
    private float m_damageRatio = 1.0f;             // ダメージの減少値


    // 被ダメージ関連
    [SerializeField] protected bool m_damageFlg = false;                     // 攻撃を受けた際のフラグ
    [SerializeField] private float m_damageTime = 0.0f;     // 復活までの時間
    [SerializeField] public int m_damagePoint = 0;          // ダメージ量
    [SerializeField] private float m_time = 0.0f;           // 経過時間管理


    // ======================
    // ディゾルブ処理用
    // ======================
    protected SkinMesh m_skinMesh;                          // 影
    protected Dissolve m_dissolve;                          // ディゾルブ
    [SerializeField] private float m_dissolveSpeed = 0.1f;  // ディゾルブ処理がかかる時間
    private float m_dissolveRatio = 0.0f;                   // ディゾルブ処理の進行割合


    // ======================
    // カメラを揺らすよ
    // ======================
    protected CameraQuake m_camera;     // カメラを揺らすスクリプト


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

        // HP 減少処理
        HpDown();
        if (m_hpState == HpState.Dead)
        {
            if (!m_golemLeft && !m_golemRight && !m_golemMain)
            {
                foreach (Transform child in this.transform)
                {
                    Debug.Log("終わり");
                    //自分の子供をDestroyする
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
        // ダメージの処理を先に持ってくる（早期リターン）
        // =======================================================
        if (!m_damageFlg)
        {
            // どちらかの部位がダメージを受けた状態なら全ての部位をダメージ状態にする
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
        // ダメージ状態から復活するまでの処理
        else
        {
            if (m_golemLeft.m_damageFlg || m_golemRight.m_damageFlg || m_golemMain.m_damageFlg)
            {
                m_time -= Time.deltaTime;

                // ゴーレムがピンチ状態なら大技を放つ
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

                // ダメージアニメーションを終了する処理
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
        // 両方の腕がある時の攻撃処理（合掌攻撃）
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
        // HPを減らすよ
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

        // ダメージアニメーションの再生
        if (m_golemLeft) m_golemLeft.HitDamage();
        if (m_golemRight) m_golemRight.HitDamage();
        if (m_golemMain) m_golemMain.HitDamage();
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
    protected void HitDamage()
    {
        attackManager.ResetAttackAnimation();
        attackManager.ChangeAnimation("Damage", true);

        if (m_weakCollider)
        {
            m_weakCollider.m_weakHit = false;
        }
    }


    // 攻撃から起き上がる処理
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


    // アニメーション終了時に読み込み
    private void ResetAnimation()
    {
        m_stop = false;
        m_attackWait = false;
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


    public string GetNowAttackName()
    {
        return m_nowAttackName;
    }
}
