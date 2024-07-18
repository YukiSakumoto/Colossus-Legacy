using Effekseer;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.PackageManager.Requests;
#endif
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class GolemMain : Golem
{
    [SerializeField] private GameObject m_armors;
    private Dissolve m_armorDissolves;
    private SkinMesh m_armorSkinMesh;

    [SerializeField] private float m_armorDissolveSpeed = 0.2f;
    private float m_armorDissolveRatio = 0.0f;

    EffekseerEffectAsset m_effect;
    EffekseerHandle m_effectHandle;

    Vector3 m_effectPos;
    Quaternion m_effectRot;
    Vector3 m_effectScale = Vector3.one;

    // エフェクトの数
    public int m_effectNum = 1;

    public float m_shotTime = 10.0f;
    public float m_coolTime = 10.0f;

    [SerializeField] private float m_endTime = 1.5f;
    private float m_scaleTime = 0.0f;

    private float m_laserTime = 2.0f;
    private bool m_isLaser = false;
    private bool m_laserCol = false;

    //[SerializeField] private float m_shrinkSpeed = 0.1f;

    [SerializeField] private List<Collider> m_laserCollider;
    [SerializeField] private Transform m_laserTransform;

    [SerializeField] private GameObject m_attackAreaLaser;
    [SerializeField] private Transform m_areaLaser;


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

    // サウンド
    GolemSounds m_sound;

    [SerializeField] float m_bodyRotTime = 0.0f;



    void Start()
    {
        m_skinMesh = GetComponent<SkinMesh>();
        m_dissolve = GetComponent<Dissolve>();
        m_camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraQuake>();
        m_weakLight.enabled = false;

        if (m_armors)
        {
            m_armorDissolves = m_armors.GetComponent<Dissolve>();
            m_armorSkinMesh = m_armors.GetComponent<SkinMesh>();
        }

        attackManager = GetComponent<AttackManager>();

        // エフェクトを取得する。
        m_effect = Resources.Load<EffekseerEffectAsset>("BigLaser");

        m_initVec = m_forward;
        m_initRot.eulerAngles = m_initVec;
        m_initRot.eulerAngles += new Vector3(-90.0f, 180.0f, 0.0f);

        m_nowRot = m_initRot;
        m_headTrans.rotation = m_nowRot;
        //m_initVec += new Vector3(-90.0f, 0.0f, 0.0f);

        m_sound = GetComponent<GolemSounds>();
    }


    void Update()
    {
        if (!m_alive)
        {
            PartsDestroy();
            if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.Battle)
            {
                GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.PlayerWin);
            }
            return;
        }

        // ====================================
        // プレイヤーを追従する処理
        // ====================================
        if (m_trackingFlg)
        {
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

            // 初期角度とモデル角度の差分
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

        m_scaleTime = m_endTime;
    }


    private bool EndEffect()
    {
        DestroyAttackArea();
        bool result = false;

        if (m_scaleTime > 0) { m_scaleTime -= Time.deltaTime; }

        float ratio = (1.0f - (m_endTime - m_scaleTime ) / m_endTime);
        Vector3 scale = new Vector3(ratio, ratio, ratio);

        m_effectScale = scale;
        m_effectHandle.SetScale(m_effectScale);

        if (m_laserCol)
        {
            for (int i = 0; i < m_laserCollider.Count; i++)
            {
                m_laserCollider[i].enabled = false;
            }
            m_laserCol = false;
        }

        if (m_scaleTime <= 0.0f)
        {
            m_effectHandle.Stop();
            m_effectScale = Vector3.one;

            WeakOn();

            result = true;
        }

        return result;
    }


    public void SpecialAttack()
    {
        ArmorDestroy();
        m_laserTime -= Time.deltaTime;
        if (m_isLaser && !m_laserCol)
        {
            if (m_laserTime < m_shotTime - 2.2f)
            {
                for (int i = 0; i < m_laserCollider.Count; i++)
                {
                    m_laserCollider[i].enabled = true;
                }
                m_laserCol = true;
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

                    m_bodyRotTime = 0.0f;
                }
            }
            // ビームを放つ！
            else
            {
                if (m_stop) return;

                m_nowAttackName = "BigLaser";

                BigLaserEffect();
                m_sound.PlayLaserCharge();
                Invoke("LaserShotSound", 2.0f);

                WeakOff();

                m_laserTime = m_shotTime;
                m_isLaser = true;

                m_bodyRotTime = -2.0f;

                AttackAreaLaser();
            }
        }
        else
        {
            if (m_isLaser)
            {
                BodyRotation(m_target.transform);
            }
            else
            {
                BodyInit();
            }
        }
    }


    // 鎧破壊
    public void ArmorDestroy()
    {
        if (!m_armors) { return; }
        if (!m_armorDissolves) { return; }

        m_armorDissolveRatio += m_armorDissolveSpeed * Time.deltaTime;
        m_armorDissolves.SetDissolveAmount(m_armorDissolveRatio);

        if (m_armorDissolveRatio >= 1.0f)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Armors")
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        if (!m_armorSkinMesh) { return; }
        m_armorSkinMesh.SetSkinMeshShadow(false);
    }


    // 頭を正面に向ける
    private void TrackingOff()
    {
        m_trackingFlg = false;
    }


    // 頭をターゲットに向ける
    private void TrackingOn()
    {
        m_trackingFlg = true;
    }


    private void LaserShotSound()
    {
        m_sound.PlayLaserShot();
        m_sound.PlayLaserKeep();
        m_camera.StartShake(10.0f, 1.0f, 30.0f);
    }


    private void OnDestroy()
    {
        m_effectHandle.Stop();
    }


    private void BodyRotation(Transform _target)
    {
        if (m_bodyRotTime < 0) { m_bodyRotTime += Time.deltaTime; return; }

        Quaternion beforeRot = new();
        beforeRot.eulerAngles = Vector3.forward;

        Quaternion afterRot = beforeRot;
        if (_target.position.x > this.transform.position.x)
        {
            afterRot.eulerAngles -= new Vector3(0.0f, 60.0f, 0.0f);
        }
        else
        {
            afterRot.eulerAngles += new Vector3(0.0f, 60.0f, 0.0f);
        }

        float timeRatio = 0.0f;
        timeRatio = m_bodyRotTime / 10.0f;

        this.transform.rotation = Quaternion.Lerp(beforeRot, afterRot, timeRatio);
        this.transform.rotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_effectRot = Quaternion.Lerp(beforeRot, afterRot, timeRatio);
        m_effectHandle.SetRotation(m_effectRot);
        m_effectHandle.SetLocation(m_laserTransform.position);

        m_bodyRotTime += Time.deltaTime;
    }


    private void BodyInit()
    {

        if (m_bodyRotTime < 0) { m_bodyRotTime += Time.deltaTime; return; }

        Quaternion beforeRot = this.transform.rotation;
        beforeRot *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        Quaternion afterRot = beforeRot;
        afterRot.eulerAngles = Vector3.forward;

        float timeRatio = 0.0f;
        timeRatio = m_bodyRotTime / m_coolTime;

        this.transform.rotation = Quaternion.Lerp(beforeRot, afterRot, timeRatio);
        this.transform.rotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_bodyRotTime += Time.deltaTime;
    }


    private void AttackAreaLaser()
    {
        Vector3 targetPos = m_areaLaser.position;

        m_attackAreaIns = Instantiate(m_attackAreaLaser, targetPos, new Quaternion(), this.transform);
    }
}
