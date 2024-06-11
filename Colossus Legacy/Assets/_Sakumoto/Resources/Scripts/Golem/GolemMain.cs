using Effekseer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GolemMain : Golem
{
    [SerializeField] private List<Dissolve> m_dissolves;
    [SerializeField] private float m_dissolveSpeed = 0.1f;
    private float m_dissolveRatio = 0.0f;

    EffekseerEffectAsset m_effect;
    EffekseerHandle m_effectHandle;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // エフェクトの数
    public int m_effectNum = 1;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;

    [SerializeField] private float m_shrinkSpeed = 0.1f;

    [SerializeField] private List<Collider> m_laserCollider;
    [SerializeField] private Transform m_laserTransform;

    // ================================
    // ターゲットへの回転補正系
    // ================================
    bool m_trackingFlg = true;

    // 頭
    [SerializeField] private Transform m_headTrans;

    // 前方の基準となるローカル空間ベクトル
    [SerializeField] private Vector3 m_forward = Vector3.back;

    Quaternion m_nowRot;

    // 初期方向ベクトル
    private Vector3 m_initVec;
    private Quaternion m_initRot;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        // エフェクトを取得する。
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_initVec = m_forward;
        m_initRot.eulerAngles = m_initVec;
        m_initRot.eulerAngles += new Vector3(-90.0f, 180.0f, 0.0f);

        m_nowRot = m_initRot;
        m_headTrans.rotation = m_nowRot;
        //m_initVec += new Vector3(-90.0f, 0.0f, 0.0f);
    }


    void Update()
    {
        if (!m_alive) { return; }

        WeakHit();


        // ====================================
        // プレイヤーを追従する処理
        // ====================================
        if (m_trackingFlg)
        {
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
            Quaternion deltaDir = new Quaternion();
            deltaDir.eulerAngles = rot.eulerAngles - m_nowRot.eulerAngles;

            m_headTrans.eulerAngles += deltaDir.eulerAngles;
            m_nowRot = m_headTrans.rotation;
        }
        else
        {
            m_headTrans.rotation = m_initRot;
        }
    }


    public void BigLaserEffect()
    {
        // エフェクトの位置設定
        m_effectPos = m_laserTransform.position;

        // エフェクトの回転設定
        m_effectRot = transform.rotation;
        m_effectRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_effectHandle = EffekseerSystem.PlayEffect(m_effect, m_effectPos);
        m_effectHandle.SetRotation(m_effectRot);
    }


    private bool EndEffect()
    {
        bool result = false;

        m_effectScale -= Vector3.one * m_shrinkSpeed * Time.deltaTime;
        m_effectHandle.SetScale(m_effectScale);

        for (int i = 0; i < m_laserCollider.Count; i++)
        {
            m_laserCollider[i].enabled = false;
        }

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
        ArmorDestroy();
        m_laserTime -= Time.deltaTime;
        if (m_isLaser)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                for (int i = 0; i < m_laserCollider.Count; i++)
                {
                    m_laserCollider[i].enabled = true;
                }
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

                WeakOff();

                m_laserTime = m_shotTime;
                m_isLaser = true;
            }
        }
    }


    // 鎧破壊
    public void ArmorDestroy()
    {
        if (m_dissolves.Count > 0)
        {
            m_dissolveRatio += m_dissolveSpeed * Time.deltaTime;

            for (int i = 0; i < m_dissolves.Count; i++)
            {
                m_dissolves[i].SetDissolveAmount(m_dissolveRatio);
            }

            if (m_dissolveRatio >= 1.0f)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == "Armors")
                    {
                        m_dissolves.Clear();
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }
    }


    // 頭を正面に向ける
    private void TrackingOff()
    {
        m_trackingFlg = false;
        m_headTrans.rotation = m_initRot;
    }


    // 頭をターゲットに向ける
    private void TrackingOn()
    {
        m_trackingFlg = true;
    }
}
