using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody m_rb; // リジッドボディ。

    private string m_targetParentTag = "Enemy";

    enum Damage
    {
        small = 20,
        medium = 50,
        big = 70,
        death = 100
    }
    enum Recovery
    {
        small = 20,
        medium = 50,
        big = 70,
        full = 100
    }

    private const int m_playerMaxLife = 100;
    private const int m_rollTiredCountMax = 5;

    public int m_playerLife = m_playerMaxLife; // 主人公の体力
    private int m_rollTiredCount = 0;           // 主人公の回避行動を連続して使うと段々緩慢になっていくカウント

    private const float m_leftRightSpeed = 4f;           // キャラクターの移動速度
    private const float m_rollCoolSetTime = 0.8f;        // 回避行動の実行時間固定値
    private const float m_rollStiffnessSetTime = 0.5f;   // 回避行動終了時の硬直時間固定値
    private const float m_rollAcceleration = 2.4f;       // 回避行動の加速量固定値
    private const float m_rollTiredDecreaseBase = 0.25f;  // 回避行動の減速量設定
    private const float m_rollTiredDecreaseTimeBase = 3f;// 回避行動の減速量回復時間固定値
    private const float m_swordAttackCoolSetTime = 0.9f; // 剣で攻撃したときの硬直時間固定値
    private const float m_bowAttackCoolSetTime = 1.4f;   // 弓で攻撃したときの硬直時間固定値
    private const float m_subAttackCoolSetTime = 1.2f;   // サブ攻撃したときの硬直時間固定値
    private const float m_weaponChangeCoolSetTime = 1f;  // 武器チェンジ時のクールタイム固定値
    private const float m_damageCoolSetTime = 0.6f;      // ダメージを受けた後の硬直時間固定値
    private const float m_invincibilitySetTime = 2f;     // ダメージを受けた後の無敵時間固定値

    private float m_rollCoolTime = 0f;         // 回避行動の実行時間
    private float m_rollStiffnessTime = 0f;    // 回避行動終了時の硬直時間
    private float m_rollTiredDecrease = 0f;    // 回避行動の減速量設定
    private float m_rollTiredDecreaseTime = 0f;// 回避行動の減速量回復時間
    private float m_weaponAttackCoolTime = 0f; // 攻撃モーションから移動に移れるまでの時間
    private float m_weaponChangeCoolTime = 0f; // 武器の種類を変える時のクールタイム
    private float m_damageCoolTime = 0f;       // ダメージを受けた後の硬直時間
    private float m_invincibilityTime = 0f;    // ダメージを受けた後の無敵時間

    private bool m_walkFlg = false;                      // 移動しているかの判定(AnimationMovementへの移送用)
    public bool m_weaponFlg = false;                    // 現在剣と弓のどちらを使用しているか判定(AnimationMovementへの移送用)
    private bool m_attackFlg = false;                    // 攻撃の管理(AnimationMovementへの移送用)
    private bool m_subAttackFlg = false;                 // サブ攻撃の管理(AnimationMovementへの移送用)
    private bool m_rollFlg = false;                      // 回避行動の管理(AnimationMovementへの移送用)
    private bool m_damageFlg = false;                    // プレイヤー被ダメージ時の管理(AnimationMovementへの移送用)
    private bool m_deathFlg = false;                     // 死亡時の管理(AnimationMovementへの移送用)
    private bool m_damageMotionFlg = false;              // ダメージモーション中の管理
    private bool m_invincibleFlg = false;                // 無敵時間中の管理
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

        // 移動の処理。回避行動、攻撃中、ダメージモーション中、死亡時は移動不可
        if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg &&
            !m_damageMotionFlg && !m_deathFlg)
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
            Vector3 movement = rotationDirection * (m_leftRightSpeed * (m_rollAcceleration - m_rollTiredDecrease));

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
                    m_weaponChangeCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                }
                else // 剣に変更
                {
                    m_weaponFlg = false;
                    m_weaponChangeCoolTimeCheckFlg = true;
                    m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
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

        if (m_damageFlg)
        {
            m_damageFlg = false;
        }

        // 装備している武器で攻撃。使った武器によって硬直時間が異なる。
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
            {
                m_attackFlg = true;
                if (!m_weaponFlg) // 剣で攻撃
                {
                    m_weaponAttackCoolTime = m_swordAttackCoolSetTime;
                    m_weaponAttackCoolTimeCheckFlg = true;
                }
                else // 弓で攻撃
                {
                    m_weaponAttackCoolTime = m_bowAttackCoolSetTime;
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
                m_weaponAttackCoolTime = m_subAttackCoolSetTime;
                m_weaponAttackCoolTimeCheckFlg = true;
            }
        }

        // 回避行動
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_rollFinishCheckFlg)
            {
                m_rollCoolTime = m_rollCoolSetTime;
                m_rollStiffnessTime = m_rollStiffnessSetTime;
                m_rollCoolTimeCheckFlg = true;
                m_rollFinishCheckFlg = true;
                m_rollFlg = true;
                // 回避行動をするたびに段々スピードが下がる
                if (m_rollTiredCount < m_rollTiredCountMax)
                {
                    m_rollTiredCount++;
                }
                else
                {
                    m_rollTiredCount = m_rollTiredCountMax;
                }
                m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase;
                m_rollTiredDecrease = m_rollTiredDecreaseBase * m_rollTiredCount;
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

        // ダメージモーション中の処理
        if(m_damageMotionFlg)
        {
            m_damageCoolTime -= Time.deltaTime;
            if(m_damageCoolTime <= 0)
            {
                m_damageMotionFlg = false;
                m_invincibleFlg = true;
            }
        }

        // 無敵時間中の処理
        if(m_invincibleFlg)
        {
            m_invincibilityTime -= Time.deltaTime;
            if(m_invincibilityTime <= 0)
            {
                m_invincibleFlg = false;
            }
        }

        // 回避行動移動量減少時間の回復の処理
        if(m_rollTiredCount > 0)
        {
            m_rollTiredDecreaseTime -= Time.deltaTime;
            if(m_rollTiredDecreaseTime <= 0)
            {
                m_rollTiredCount--;
                if(m_rollTiredCount > 0)
                {
                    m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase;
                }
            }
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        // ダメージモーション中や無敵中はダメージを受けない
        if (!m_damageMotionFlg && !m_invincibleFlg)
        {
            // 触れたオブジェクトの親オブジェクトを取得
            Transform parentTransform = _other.transform.parent?.parent;

            // 親オブジェクトが存在するかを確認
            if (parentTransform != null)
            {
                if (parentTransform.gameObject.CompareTag(m_targetParentTag))
                {
                    Debug.Log("hit at " + parentTransform.name);
                    int damage = (int)Damage.medium;
                    hit(damage);
                }
                else
                {
                    // 当たったオブジェクトの親オブジェクトの親オブジェクトにタグが設定されていない場合にコンソールに表示
                    Debug.Log(parentTransform.name + " is does not have the " + m_targetParentTag);
                }
            }
            else
            {
                // 親オブジェクトの親オブジェクトに当たるオブジェクトが無い
                Debug.Log("No parent object found");
            }
        }
    }

    void hit(int _damage)
    {
        m_playerLife -= _damage;
        if (m_playerLife > 0)
        {
            m_damageFlg = true;
            m_damageMotionFlg = true;
            m_damageCoolTime = m_damageCoolSetTime;
            m_invincibilityTime = m_invincibilitySetTime;
        }
        else
        {
            m_deathFlg = true;
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
    public bool Getm_damageFlg
    {
        get { return m_damageFlg; }
    }
    public bool Getm_deathFlg
    {
        get { return m_deathFlg; }
    }
}