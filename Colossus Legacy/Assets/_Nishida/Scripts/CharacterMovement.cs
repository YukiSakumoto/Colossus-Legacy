using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public float m_leftRightSpeed = 4f;  // キャラクターの移動速度
    public float m_rotationSpeed = 200f; // キャラクターの回転速度

    private Rigidbody m_rb; // リジッドボディ。

    private float m_rollCoolTime = 0f;         // 回避行動の実行時間
    private float m_rollStiffnessTime = 0f;    // 回避行動終了時の硬直時間
    private float m_weaponAttackCoolTime = 0f; // 攻撃モーションから移動に移れるまでの時間
    private float m_weaponChangeCoolTime = 0f; // 武器の種類を変える時のクールタイム

    private bool m_walkFlg = false;                      // 移動しているかの判定(AnimationMovementへの移送用)
    private bool m_weaponFlg = false;                    // 現在剣と弓のどちらを使用しているか判定(AnimationMovementへの移送用)
    private bool m_attackFlg = false;                    // 攻撃の管理(AnimationMovementへの移送用)
    private bool m_subAttackFlg = false;                 // サブ攻撃の管理(AnimationMovementへの移送用)
    private bool m_rollFlg = false;                      // 回避行動の管理(AnimationMovementへの移送用)
    private bool m_rollCoolTimeCheckFlg = false;         // 回避行動の実行時間中かの管理
    private bool m_rollStiffnessTimeCheckFlg = false;    // 回避行動の硬直時間中かの管理
    private bool m_rollFinishCheckFlg = false;           // 回避行動が終了しているかの管理
    private bool m_weaponAttackCoolTimeCheckFlg = false; // 攻撃モーションから移動に移れるまでの時間かの管理
    private bool m_weaponChangeCoolTimeCheckFlg = false; // 武器の種類を変える時のクールタイムかの管理

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // キーボードの左右入力
        float varticalInput = Input.GetAxis("Vertical"); // キーボードの上下入力

        // 移動の処理。回避行動、武器チェンジ中、攻撃中は移動不可
        if (!m_rollFinishCheckFlg && !m_weaponChangeCoolTimeCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
        {
            if (horizontalInput != 0f || varticalInput != 0f)
            {
                m_walkFlg = true;
            }
            else
            {
                m_walkFlg = false;
            }

            // 移動量計算
            Vector3 movement = new Vector3(horizontalInput, 0.0f, varticalInput) * m_leftRightSpeed;

            // Rigidbodyを使ってキャラクターを移動
            m_rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);

            // キャラクターの向きを入力方向に変更
            if (movement != Vector3.zero)
            {
                transform.forward = movement; // キャラクターを移動方向に向ける
            }
        }

        // 回避行動中の移動処理
        if (m_rollCoolTimeCheckFlg)
        {
            // Y軸の回転に合わせて移動方向を計算する
            Vector3 rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
            Vector3 movement = rotationDirection * (m_leftRightSpeed * 2.3f);

            // Rigidbodyを使ってオブジェクトを移動
            m_rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);

            // 移動方向が0でない場合にオブジェクトの向きを変更する
            if (movement != Vector3.zero)
            {
                transform.forward = movement.normalized;
            }
        }

        // 武器チェンジ時の処理
        if (Input.GetKey(KeyCode.F))
        {
            if (!m_weaponChangeCoolTimeCheckFlg)
            {
                if (!m_weaponFlg) // 弓に変更
                {
                    m_weaponFlg = true;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = 2f;
                }
                else // 剣に変更
                {
                    m_weaponFlg = false;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = 2f;
                }
            }
        }

        // 回避行動モーションの重複防止
        if (m_rollFlg)
        {
            m_rollFlg = false;
        }

        // 攻撃モーションの重複防止
        if (m_attackFlg)
        {
            m_attackFlg = false;
        }

        // サブ攻撃モーションの重複防止
        if (m_subAttackFlg)
        {
            m_subAttackFlg = false;
        }

        // 装備している武器で攻撃。使った武器によって硬直時間が異なる。
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
            {
                m_attackFlg = true;
                if (!m_weaponFlg) // 剣で攻撃
                {
                    m_weaponAttackCoolTime = 0.9f;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
                else // 弓で攻撃
                {
                    m_weaponAttackCoolTime = 2.0f;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
            }
        }

        // サブ攻撃。爆弾を投げる。
        if (Input.GetMouseButtonDown(1))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
            {
                m_subAttackFlg = true;
                m_weaponAttackCoolTime = 1.2f;
                m_weaponAttackCoolTimeCheckFlg = true;
            }
        }

        // 回避行動
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_rollFinishCheckFlg)
            {
                m_rollCoolTime = 0.9f;
                m_rollStiffnessTime = 0.5f;
                m_rollCoolTimeCheckFlg = true;
                m_rollFinishCheckFlg = true;
                m_rollFlg = true;
            }
        }

        // 回避行動時のモーション時間処理
        if (m_rollCoolTimeCheckFlg)
        {
            m_rollCoolTime -= Time.deltaTime;
            if (m_rollCoolTime <= 0)
            {
                m_rollStiffnessTimeCheckFlg = true;
                m_rollCoolTimeCheckFlg = false;
            }
        }

        // 回避行動時の硬直時間処理
        if (m_rollStiffnessTimeCheckFlg)
        {
            m_rollStiffnessTime -= Time.deltaTime;
            if (m_rollStiffnessTime <= 0)
            {
                m_rollStiffnessTimeCheckFlg = false;
                m_rollFinishCheckFlg = false;
            }
        }

        // 武器チェンジ時の硬直時間処理
        if (m_weaponChangeCoolTimeCheckFlg)
        {
            m_weaponChangeCoolTime -= Time.deltaTime;
            if (m_weaponChangeCoolTime <= 0)
            {
                m_weaponChangeCoolTimeCheckFlg = false;
            }
        }

        // 攻撃時の硬直時間処理
        if (m_weaponAttackCoolTimeCheckFlg)
        {
            m_weaponAttackCoolTime -= Time.deltaTime;
            if (m_weaponAttackCoolTime <= 0)
            {
                m_weaponAttackCoolTimeCheckFlg = false;
            }
        }
    }

    public bool Getm_walkFlg
    {
        get { return m_walkFlg; }
    }

    public bool Getm_rollFlg
    {
        get { return m_rollFlg; }
    }

    public bool Getm_weaponFlg
    {
        get { return m_weaponFlg; }
    }
    public bool Getm_attackFlg
    {
        get { return m_attackFlg; }
    }

    public bool Getm_subAttackFlg
    {
        get { return m_subAttackFlg; }
    }
}