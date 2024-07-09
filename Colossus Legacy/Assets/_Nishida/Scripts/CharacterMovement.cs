using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_swordObject;
    [SerializeField] private GameObject m_bowObject;
    [SerializeField] private Rigidbody m_rb; // リジッドボディ
    [SerializeField] private Image m_hpGage;
    //[SerializeField] private string m_targetTag = "EnemyAttack"; // 敵との当たり判定を行う時のタグ名設定
    //[SerializeField] private string m_bombTag = "BombAttack"; // 爆弾との当たり判定を行う時のタグ名設定
    [SerializeField] CharacterManager m_manager;
    //Sword m_swordClass;
    Bow m_bowClass;
    // ダメージ量
    enum Damage
    {
        small = 20,  // 小ダメージ
        medium = 50, // 中ダメージ
        big = 70,    // 大ダメージ
        death = 100  // 即死攻撃
    }
    // ダメージ時のノックバック量
    enum KnockBack
    {
        none = 0,    // ノックバックしない
        small = 2,   // 小ノックバック
        medium = 4,  // 中ノックバック
        big = 7,    // 大ノックバック
    }
    // 回復量
    enum Recovery
    {
        small = 20,  // 小回復
        medium = 50, // 中回復
        big = 70,    // 大回復
        full = 100   // 完全回復
    }

    private const int m_playerMaxLife = 100;   // 主人公の体力の上限値
    private const int m_playerCautionLife = 50;   // 主人公の体力の注意値(バー黄色)
    private const int m_playerDangerLife = 20;   // 主人公の体力の危険値(バー赤色)
    private const int m_rollTiredCountMax = 5; // 回避行動の移動減少量カウントの上限

    [SerializeField] private int m_playerLife = m_playerMaxLife; // 主人公の体力
    [SerializeField] private int m_rollTiredCount = 0;           // 主人公の回避行動を連続して使うと段々緩慢になっていくカウント

    private const float m_leftRightSpeed = 4f;                    // キャラクターの移動速度
    private const float m_rollCoolSetTime = 0.8f;                 // 回避行動の実行時間固定値
    private const float m_rollStiffnessSetTime = 0.5f;            // 回避行動終了時の硬直時間固定値
    private const float m_rollAcceleration = 2.4f;                // 回避行動の加速量固定値
    private const float m_rollTiredDecreaseBase = 0.25f;          // 回避行動の減速量設定
    private const float m_rollTiredDecreaseTimeBase = 3f;         // 回避行動の減速量回復時間固定値
    private const float m_swordAttackCoolSetTime = 0.8f;          // 剣で攻撃したときの硬直時間固定値
    private const float m_bowAttackCoolSetTime = 1.6f;            // 弓で攻撃したときの硬直時間固定値
    private const float m_subAttackCoolSetTime = 1f;              // サブ攻撃したときの硬直時間固定値
    private const float m_weaponChangeCoolSetTime = 1f;           // 武器チェンジ時のクールタイム固定値
    private const float m_damageCoolSetTime = 0.6f;               // ダメージを受けた後の硬直時間固定値
    private const float m_downCoolSetTime = 1.7f;                 // つぶされた後の硬直時間固定値
    private const float m_pushUpCoolSetTime = 2f;                 // カチ上げられた後の硬直時間固定値
    private const float m_invincibilitySetTime = 2f;              // ダメージを受けた後の無敵時間固定値
    private const float m_blownAwayStiffnessSetTime = 0.8f;       // 吹っ飛ぶダメージを受けたときの硬直時間固定値
    private const float m_swordMoveAcceleration = 4f;             // 剣で攻撃したときの前進固定値
    private const float m_swordMoveStiffnessSetTime = 0.4f;       // 攻撃する前の硬直時間固定値
    private const float m_swordMoveSetTime = 0.1f;                // 攻撃するときの移動時間固定値
    private const float m_swordSecondMoveStiffnessSetTime = 0.4f; // 2段攻撃したときの硬直時間固定値
    private const float m_swordSecondMoveSetTime = 0.1f;          // 2段攻撃したときの移動時間固定値
    private const float m_bowShotSetTime = 0.57f;                 // 弓攻撃を行うタイミング調節固定値

    private float m_rollCoolTime = 0f;           // 回避行動の実行時間
    private float m_rollStiffnessTime = 0f;      // 回避行動終了時の硬直時間
    private float m_rollTiredDecrease = 0f;      // 回避行動の減速量設定
    private float m_rollTiredDecreaseTime = 0f;  // 回避行動の減速量回復時間
    private float m_weaponAttackCoolTime = 0f;   // 攻撃モーションから移動に移れるまでの時間
    private float m_weaponChangeCoolTime = 0f;   // 武器の種類を変える時のクールタイム
    private float m_damageCoolTime = 0f;         // ダメージを受けた後の硬直時間
    private float m_invincibilityTime = 0f;      // ダメージを受けた後の無敵時間
    private float m_blownAwayStiffnessTime = 0f; // 吹っ飛ぶダメージを受けたときの硬直時間
    private float m_swordMoveStiffnessTime = 0f; // 剣を振って動くときの硬直時間
    private float m_swordMoveTime = 0f;          // 剣を振って動く時間
    private float m_bowShotTime = 0f;            // 弓攻撃を行うタイミング調節

    private bool m_walkAnimeFlg = false;                 // 移動しているかの判定(AnimationMovementへの移送用)
    private bool m_weaponFlg = false;                    // 現在剣と弓のどちらを使用しているか判定(AnimationMovementへの移送用)
    private bool m_attackAnimeFlg = false;               // 攻撃の管理(AnimationMovementへの移送用)
    private bool m_rollAnimeFlg = false;                 // 回避行動の管理(AnimationMovementへの移送用)
    private bool m_damageAnimeFlg = false;               // プレイヤー被ダメージ時の管理(AnimationMovementへの移送用)
    private bool m_blownAwayAnimeFlg = false;            // 吹っ飛ぶ攻撃を受けたときの管理(AnimationMovementへの移送用)
    private bool m_downAnimeFlg = false;                 // 潰される攻撃を受けたときの管理(AnimationMovementへの移送用)
    private bool m_pushUpAnimeFlg = false;               // カチ上げ攻撃を受けたときの管理(AnimationMovementへの移送用)
    private bool m_deathFlg = false;                     // 死亡時の管理(AnimationMovementへの移送用)
    private bool m_secondSwordAttackAnimeFlg = false;    // 2段攻撃を行う際の管理(AnimationMovementへの移送用)
    private bool m_joyAnimeFlg = false;                  // 喜んで動かなくなる管理(AnimationMovementへの移送用)
    private bool m_damageMotionFlg = false;              // ダメージモーション中の管理
    private bool m_damageBlownAwayFlg = false;           // 吹っ飛ぶモーション中の管理
    private bool m_damageBlownAwayStiffnessFlg = false;  // 吹っ飛ぶモーションの硬直時間の管理
    private bool m_invincibleFlg = false;                // 無敵時間中の管理
    private bool m_rollCoolTimeCheckFlg = false;         // 回避行動の実行時間中かの管理
    private bool m_rollStiffnessTimeCheckFlg = false;    // 回避行動の硬直時間中かの管理
    private bool m_rollFinishCheckFlg = false;           // 回避行動が終了しているかの管理
    private bool m_weaponAttackCoolTimeCheckFlg = false; // 攻撃モーションから移動に移れるまでの時間かの管理
    private bool m_weaponChangeCoolTimeCheckFlg = false; // 武器の種類を変える時のクールタイムかの管理
    private bool m_swordMoveFlg = false;                 // 剣を振る際の前移動
    private bool m_secondSwordAttackFlg = false;         // 2段攻撃を行う際の管理
    private bool m_bowShotFlg = false;                   // 弓攻撃を行う際の管理
    private bool m_swordMotionFlg = false;               // 剣攻撃モーション中の管理
    private bool m_secondSwordMotionFlg = false;         // 2段攻撃モーション中の管理
    private bool m_bowMotionFlg = false;                 // 弓攻撃モーション中の管理
    private bool m_subAttackMotionFlg = false;           // サブ攻撃モーション中の管理
    private bool m_joyFlg = false;                       // 喜んで動かなくなる管理

    private Vector3 m_KnockBackVec = Vector3.zero; // ノックバック量を代入する

    // Start is called before the first frame update
    void Start()
    {
        if(!m_manager)
        {
            Debug.Log("CharacterMovement:manager is Null");
        }

        if(!m_swordObject)
        {
            Debug.Log("CharacterMovement:Sword is Null");
        }
        else
        {
            m_swordObject.SetActive(true);
        }

        if (!m_bowObject)
        {
            Debug.Log("CharacterMovement:Bow is Null");
        }
        else
        {
            m_bowObject.SetActive(false);
            m_bowClass = m_bowObject.GetComponent<Bow>();
        }

        if (!m_rb)
        {
            Debug.Log("CharacterMovement:RigidBody is Null");
        }

        if(!m_hpGage)
        {
            Debug.Log("CharacterMovement:HP Gage is Null");
        }
        else
        {
            m_hpGage.color = Color.green;
        }

        //if(m_targetTag == "")
        //{
        //    Debug.Log("CharacterMovement:tag is Null");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_joyFlg && !m_deathFlg)
        {
            // 武器チェンジ時の処理
            if (Input.GetKey(KeyCode.F))
            {
                if (!m_weaponChangeCoolTimeCheckFlg) // 武器チェンジが連続で発生しないようフラグで管理
                {
                    if (!m_swordMotionFlg && !m_secondSwordMotionFlg && !m_bowMotionFlg && !m_subAttackMotionFlg) // 攻撃中は武器チェンジ出来ないようにする
                    {
                        if (!m_weaponFlg) // 弓に変更
                        {
                            m_swordObject.SetActive(false);
                            m_bowObject.SetActive(true);
                            m_weaponFlg = true;
                            m_weaponChangeCoolTimeCheckFlg = true;
                            m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                        }
                        else // 剣に変更
                        {
                            m_swordObject.SetActive(true);
                            m_bowObject.SetActive(false);
                            m_weaponFlg = false;
                            m_weaponChangeCoolTimeCheckFlg = true;
                            m_weaponChangeCoolTime = m_weaponChangeCoolSetTime;
                        }
                    }
                }
            }

            // モーション重複の管理系処理
            MotionProcessing();

            // マウス左クリックで装備している武器で攻撃。使った武器によって硬直時間が異なる。
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg) // 回避行動及び攻撃のモーション中は攻撃できない
                {
                    m_attackAnimeFlg = true;
                    if (!m_weaponFlg) // 剣で攻撃
                    {
                        m_weaponAttackCoolTime = m_swordAttackCoolSetTime;
                        m_weaponAttackCoolTimeCheckFlg = true;
                        m_swordMotionFlg = true;
                        m_swordMoveFlg = true;
                        m_swordMotionFlg = true;
                        m_swordMoveStiffnessTime = m_swordMoveStiffnessSetTime;
                        m_swordMoveTime = m_swordMoveSetTime;
                    }
                    else // 弓で攻撃
                    {
                        m_weaponAttackCoolTime = m_bowAttackCoolSetTime;
                        m_bowShotTime = m_bowShotSetTime;
                        m_weaponAttackCoolTimeCheckFlg = true;
                        m_bowMotionFlg = true;
                        m_bowShotFlg = true;
                    }
                }
            }

            // 剣を振った時に前に進む処理
            if (m_swordMoveFlg)
            {
                if (m_swordMoveStiffnessTime < m_swordMoveStiffnessSetTime)
                {
                    // モーション中にもう一度クリックすると2段攻撃
                    if (Input.GetMouseButtonDown(0))
                    {
                        m_secondSwordAttackAnimeFlg = true;
                        m_secondSwordAttackFlg = true;
                        m_secondSwordMotionFlg = true;
                    }
                }

                m_swordMoveStiffnessTime -= Time.deltaTime;
                if (m_swordMoveStiffnessTime < 0)
                {
                    m_swordMoveTime -= Time.deltaTime;
                    if (m_swordMoveTime < 0 || m_manager.Getm_hitFlg)
                    {
                        m_swordMoveFlg = false;
                        if (m_secondSwordAttackFlg) // 2段攻撃を行う用の変数設定
                        {
                            m_swordMoveStiffnessTime = m_swordSecondMoveStiffnessSetTime;
                            m_swordMoveTime = m_swordSecondMoveSetTime;
                        }
                    }
                }
            }
            else
            {
                // 2段攻撃を行う時のみ派生
                if (m_secondSwordAttackFlg)
                {
                    m_swordMoveStiffnessTime -= Time.deltaTime;
                    if (m_swordMoveStiffnessTime < 0)
                    {
                        m_swordMoveTime -= Time.deltaTime;
                        if (m_swordMoveTime < 0 || m_manager.Getm_hitFlg)
                        {
                            m_secondSwordAttackFlg = false;
                        }
                    }
                }
            }

            // サブ攻撃。マウスの右クリックで爆弾を投げる。
            if (m_manager.Getm_bombThrowCheckFlg)
            {
                if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg)
                {
                    m_weaponAttackCoolTime = m_subAttackCoolSetTime;
                    m_weaponAttackCoolTimeCheckFlg = true;
                    m_subAttackMotionFlg = true;
                }
            }

            // 回避行動
            if (Input.GetKey(KeyCode.Space))
            {
                if (!m_bowShotFlg && !m_deathFlg && !m_bowMotionFlg && !m_subAttackMotionFlg)
                {
                    if (!m_rollFinishCheckFlg)
                    {
                        m_rollCoolTime = m_rollCoolSetTime; // 回避行動中の時間設定
                        m_rollStiffnessTime = m_rollStiffnessSetTime; // 回避行動後の硬直時間設定
                        m_rollCoolTimeCheckFlg = true;
                        m_rollFinishCheckFlg = true;
                        m_rollAnimeFlg = true;
                        // 回避行動をするたびに段々スピードが下がる
                        if (m_rollTiredCount < m_rollTiredCountMax)
                        {
                            m_rollTiredCount++; // 減衰の量の増加
                        }
                        else
                        {
                            // 回避行動の減衰回数の限界
                            m_rollTiredCount = m_rollTiredCountMax;
                        }
                        m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase; // 減衰の回復にかかる時間設定 
                        m_rollTiredDecrease = m_rollTiredDecreaseBase * m_rollTiredCount; // 減衰量計算
                    }
                }
            }
        }
        else
        {
            m_walkAnimeFlg = false;
        }
        // デバッグ。Yキーを押すたびに回復
        //if(Input.GetKey(KeyCode.Y)) 
        //{
        //    Cure((int)Recovery.small);
        //}

        // 時間経過で発生したりフラグを変更する処理
        TimeProcessing();
    }

    private void FixedUpdate()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        //Vector3 position = transform.position;
        //if (position.y < 0)
        //{
        //    position.y = 0;
        //    transform.position = position;
        //}

        if (!m_joyFlg)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // キーボードの左右入力
            float varticalInput = Input.GetAxis("Vertical"); // キーボードの上下入力

            bool moveFlg = false;

            Vector3 movement = new(0,0,0);
            Vector3 rotationDirection = new(0,0,0);
            Vector3 newPosition = new(0, 0, 0);
            RaycastHit hit;

            // 移動の処理。回避行動、攻撃中、ダメージモーション中、死亡時は移動不可
            if (!m_rollFinishCheckFlg && !m_weaponAttackCoolTimeCheckFlg &&
                !m_damageMotionFlg && !m_damageBlownAwayStiffnessFlg && !m_deathFlg)
            {
                if (horizontalInput != 0f || varticalInput != 0f) // キー入力がされているときは歩きモーションになる
                {
                    m_walkAnimeFlg = true;
                }
                else
                {
                    m_walkAnimeFlg = false;
                }

                moveFlg = true;

                // 移動量計算
                movement = new Vector3(horizontalInput, 0.0f, varticalInput) * m_leftRightSpeed;

                // キャラクターの向きを入力方向に変更
                if (movement != Vector3.zero)
                {
                    transform.forward = movement; // キャラクターを移動方向に向ける
                }
            }
            else if (m_rollCoolTimeCheckFlg) // 回避行動中の移動処理
            {
                moveFlg = true;

                // Y軸の回転に合わせて移動方向を計算する
                rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
                movement = rotationDirection * (m_leftRightSpeed * (m_rollAcceleration - m_rollTiredDecrease));

                // 移動方向が0でない場合にオブジェクトの向きを変更する
                if (movement != Vector3.zero)
                {
                    transform.forward = movement.normalized;
                }
            }
            else if (m_damageBlownAwayFlg) // ダメージモーション中のノックバックの動きとか
            {
                moveFlg = true;

                // enumに小数点が入れられないので計算用
                float knockbackPower = 10f;

                // ノックバック量計算
                movement = m_KnockBackVec.normalized * ((float)KnockBack.medium * knockbackPower);
            }

            // 剣を振っている間の処理
            if (m_swordMoveFlg || (!m_swordMoveFlg && m_secondSwordAttackFlg))
            {
                if (m_swordMoveStiffnessTime < 0)
                {
                    if (m_swordMoveTime >= 0 && !m_manager.Getm_hitFlg)
                    {
                        moveFlg = true;

                        // Y軸の回転に合わせて移動方向を計算する
                        rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
                        movement = rotationDirection * (m_leftRightSpeed * m_swordMoveAcceleration);
                        // 移動方向が0でない場合にオブジェクトの向きを変更する
                        if (movement != Vector3.zero)
                        {
                            transform.forward = movement.normalized;
                        }
                    }
                }
            }

            if(moveFlg)
            {
                newPosition = transform.position + movement * Time.deltaTime;
                if (Physics.Raycast(transform.position, movement, out hit, movement.magnitude * Time.deltaTime))
                {
                    newPosition = hit.point;
                }
                // Rigidbodyを使ってキャラクターを移動
                m_rb.MovePosition(newPosition);
                m_rb.interpolation = RigidbodyInterpolation.Interpolate;
            }

            // 弓矢を放つ時の時間経過関係。
            if (m_bowShotFlg)
            {
                m_bowShotTime -= Time.deltaTime;
                if (m_bowShotTime <= 0)
                {
                    m_bowClass.Shot();
                    m_bowShotFlg = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        // 吹っ飛ぶモーションの重複防止
        if (m_blownAwayAnimeFlg)
        {
            m_blownAwayAnimeFlg = false;
        }

        if(m_downAnimeFlg)
        {
            m_downAnimeFlg = false;
        }

        if(m_pushUpAnimeFlg)
        {
            m_pushUpAnimeFlg = false;
        }

        // ダメージモーションの重複防止
        if (m_damageAnimeFlg)
        {
            m_damageAnimeFlg = false;
        }
    }

    // ダメージを受けたときの汎用処理
    public void Hit(int _damage, bool _knockBack, bool _down, bool _pushup)
    {
        // ダメージモーション中や無敵中、ゲームクリア時はダメージを受けない
        if (!m_damageMotionFlg && !m_damageBlownAwayFlg && !m_invincibleFlg && !m_joyFlg)
        {
            m_playerLife -= _damage;
            float ratio = (float)m_playerLife / (float)m_playerMaxLife;
            m_hpGage.fillAmount = ratio;
            if (m_playerLife <= m_playerDangerLife)
            {
                m_hpGage.color = Color.red;
            }
            else if (m_playerLife <= m_playerCautionLife)
            {
                m_hpGage.color = Color.yellow;
            }

            if (m_playerLife > 0) // ダメージを受けて体力が0以下にならなければダメージモーション + 無敵時間発生
            {
                // ダメージを受けた時ノックバックする場合
                if (_knockBack)
                {
                    m_blownAwayAnimeFlg = true;
                    m_damageBlownAwayFlg = true;
                    m_damageBlownAwayStiffnessFlg = true;
                    m_damageCoolTime = m_damageCoolSetTime;
                    m_blownAwayStiffnessTime = m_blownAwayStiffnessSetTime;
                }
                else if(_down) // ダメージを受けたとき潰される場合
                {
                    m_downAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_downCoolSetTime;
                }
                else if(_pushup) // ダメージを受けた時カチ上げられる場合
                {
                    m_pushUpAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_pushUpCoolSetTime;
                }
                else // ダメージを受けた時に大きなアクションをしない場合
                {
                    m_damageAnimeFlg = true;
                    m_damageMotionFlg = true;
                    m_damageCoolTime = m_damageCoolSetTime;
                }
                m_invincibilityTime = m_invincibilitySetTime;
            }
            else // 体力が0以下になった場合に死亡して動きも止める
            {
                m_deathFlg = true;
                GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.PlayerDead);
                Debug.Log("主人公死亡");
            }
        }
    }

    // Is Triggerが付いているColliderに接触したときの処理
    void OnTriggerEnter(Collider _other)
    {
        // ダメージモーション中や無敵中、ゲームクリア時はダメージを受けない
        if (!m_damageMotionFlg && !m_damageBlownAwayFlg && !m_invincibleFlg && !m_joyFlg)
        {
            if (_other.CompareTag("TreasureOpen"))
            {
                Setm_joyFlg();
            }
            else
            {
                // 触れたオブジェクトのTransformを取得
                Transform targetTransform = _other.transform;

                // オブジェクトが存在するかを確認
                if (targetTransform != null)
                {
                    // 攻撃を行ったオブジェクトの位置から攻撃を受けたオブジェクトの位置を引いて、攻撃を受けた方向のベクトルを計算
                    m_KnockBackVec = transform.position - targetTransform.position;
                }
                else
                {
                    // オブジェクトが無い
                    Debug.Log("当たったオブジェクトが無い。");
                }
            }
        }
    }

    void Cure(int _recover)
    {
        m_playerLife += _recover;
        if(m_playerLife > m_playerMaxLife)
        {
            m_playerLife = m_playerMaxLife;
        }

        float ratio = (float)m_playerLife / (float)m_playerMaxLife;
        m_hpGage.fillAmount = ratio;
        if(m_playerLife > m_playerCautionLife)
        {
            m_hpGage.color = Color.green;
        }
        else if(m_playerLife > m_playerDangerLife)
        {
            m_hpGage.color = Color.yellow;
        }
    }

    // モーション重複防止用のフラグ管理系処理
    void MotionProcessing()
    {
        // 回避行動モーションの重複防止
        if (m_rollAnimeFlg)
        {
            m_rollAnimeFlg = false;
        }

        // 攻撃モーションの重複防止
        if (m_attackAnimeFlg)
        {
            m_attackAnimeFlg = false;
        }

        // 2段攻撃モーションの重複防止
        if (m_secondSwordAttackAnimeFlg)
        {
            m_secondSwordAttackAnimeFlg = false;
        }

        // 喜びモーションの重複防止
        if (m_joyAnimeFlg) 
        {
            m_joyAnimeFlg = false;
        }
    }

    // 時間経過で発生したりフラグを変更する処理
    void TimeProcessing()
    {
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
                m_swordMotionFlg = false;
                m_secondSwordMotionFlg = false;
                m_bowMotionFlg = false;
                m_subAttackMotionFlg = false;
                m_weaponAttackCoolTimeCheckFlg = false;
            }
        }

        // ダメージモーション中の処理
        if (m_damageMotionFlg)
        {
            m_damageCoolTime -= Time.deltaTime;
            if (m_damageCoolTime <= 0)
            {
                m_damageMotionFlg = false;
                m_invincibleFlg = true;
            }
        }

        // 吹っ飛ぶモーション中の処理
        if (m_damageBlownAwayFlg)
        {
            m_damageCoolTime -= Time.deltaTime;
            if (m_damageCoolTime <= 0)
            {
                m_damageBlownAwayFlg = false;
                m_invincibleFlg = true;
            }
        }
        else if (m_damageBlownAwayStiffnessFlg) // 吹っ飛ぶモーション後の硬直時間
        {
            m_blownAwayStiffnessTime -= Time.deltaTime;
            if (m_blownAwayStiffnessTime < 0)
            {
                m_damageBlownAwayStiffnessFlg = false;
            }
        }
        else if (m_invincibleFlg) // 無敵時間中の処理
        {
            m_invincibilityTime -= Time.deltaTime;
            if (m_invincibilityTime <= 0)
            {
                m_invincibleFlg = false;
            }
        }

        // 回避行動移動量減少時間の回復の処理
        if (m_rollTiredCount > 0)
        {
            m_rollTiredDecreaseTime -= Time.deltaTime;
            if (m_rollTiredDecreaseTime <= 0)
            {
                m_rollTiredCount--;
                if (m_rollTiredCount > 0)
                {
                    m_rollTiredDecreaseTime = m_rollTiredDecreaseTimeBase;
                }
            }
        }
    }

    public bool Getm_walkAnimeFlg
    {
        get { return m_walkAnimeFlg; }
    }
    public bool Getm_rollAnimeFlg
    {
        get { return m_rollAnimeFlg; }
    }
    public bool Getm_weaponFlg
    {
        get { return m_weaponFlg; }
    }
    public bool Getm_attackAnimeFlg
    {
        get { return m_attackAnimeFlg; }
    }
    public bool Getm_secondSwordAttackAnimeFlg
    {
        get { return m_secondSwordAttackAnimeFlg; }
    }
    public bool Getm_damageAnimeFlg
    {
        get { return m_damageAnimeFlg; }
    }
    public bool Getm_blownAwayAnimeFlg
    {
        get { return m_blownAwayAnimeFlg; }
    }
    public bool Getm_downAnimeFlg
    {
        get { return m_downAnimeFlg; }
    }
    public bool Getm_pushUpAnimeFlg
    {
        get { return m_pushUpAnimeFlg; }
    }
    public bool Getm_deathFlg
    {
        get { return m_deathFlg; }
    }

    public bool Getm_swordMoveFlg
    {
        get { return m_swordMoveFlg; }
    }

    public bool Getm_secondSwordAttackFlg
    {
        get { return m_secondSwordAttackFlg; }
    }

    public bool Getm_joyFlg
    {
        get { return m_joyFlg; }
    }

    public bool Getm_joyAnimeFlg
    {
        get { return m_joyAnimeFlg; }
    }

    public int GetLife()
    {
        return m_playerLife;
    }

    void Setm_joyFlg()
    {
        m_joyFlg = true;
        m_joyAnimeFlg = true;
        GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.TreasureGet);
    }
}