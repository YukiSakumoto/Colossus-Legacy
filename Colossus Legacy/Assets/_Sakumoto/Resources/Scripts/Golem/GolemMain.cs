using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMain : Golem
{
    EffekseerEffectAsset m_effect;
    EffekseerHandle m_effectHandle;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // エフェクトの数（透明度が高かったため、エフェクトを大量に放つことでデカレーザーにしてみる）
    public int m_effectNum = 1;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;

    [SerializeField] private Collider m_laserCollider;

    // ================================
    // ターゲットへの回転補正系
    // ================================
    // 頭
    [SerializeField] private Transform m_headTrans;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 m_forward = Vector3.forward;

    // 初期方向ベクトル
    private Vector3 m_initVec;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        // エフェクトを取得する。
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_initVec = Vector3.back;
    }


    void Update()
    {
        if (!m_alive) { return; }

        WeakHit();

        // ====================================
        // プレイヤーを追従する処理
        // ====================================
        if (!m_target || !m_headTrans) { return; }

        // ターゲットへの向きベクトル計算
        Vector3 dir = m_target.transform.position - m_headTrans.position;
        // ターゲットの方向への回転
        Quaternion lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
        // 回転補正
        Quaternion offsetRotation = Quaternion.FromToRotation(m_forward, Vector3.forward);

        // ターゲット方向への回転の順に自身の向きを操作
        // ※実際のプレイヤーへの向きが入る
        Quaternion rot = lookAtRotation * offsetRotation;
        // モデルの向き調整
        rot.eulerAngles += new Vector3(-90.0f, 0.0f, 0.0f);

        //// 初期角度とモデル角度の差分
        //Vector3 deltaDir = m_initVec - rot.eulerAngles;

        //m_headTrans.eulerAngles += deltaDir;
        m_headTrans.rotation = rot;
    }


    public void BigLaserEffect()
    {
        // エフェクトの位置設定
        m_effectPos = transform.position;
        m_effectPos.y += 4.0f;
        m_effectPos.x -= 0.5f;

        // エフェクトの回転設定
        m_effectRot = transform.rotation;
        m_effectRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_effectHandle = EffekseerSystem.PlayEffect(m_effect, m_effectPos);
        m_effectHandle.SetRotation(m_effectRot);
    }


    private bool EndEffect()
    {
        bool result = false;

        m_effectScale *= 0.95f;
        m_effectHandle.SetScale(m_effectScale);

        m_laserCollider.enabled = false;

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
        m_laserTime -= Time.deltaTime;
        if (m_isLaser)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                m_laserCollider.enabled = true;
            }
        }


        if (m_laserTime <= 0.0f)
        {
            // クールダウン中…
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
            // ビームを放つ！
            else
            {
                if (m_stop) return;

                BigLaserEffect();

                m_laserTime = m_shotTime;
                m_isLaser = true;
            }
        }
    }


    // 鎧破壊
    public void ArmorDestroy()
    {
        // できればディゾルブでやってみたい！
        foreach (Transform child in transform)
        {
            if (child.name == "Armors")
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
