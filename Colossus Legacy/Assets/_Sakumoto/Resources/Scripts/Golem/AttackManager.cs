using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


// -----------------------------
// AttackManager : 使い方
// 1. 使いたいスクリプトでAttackManagerを呼び出す
// 2. AddAttack() に追加したい攻撃の ID を引数で渡す
// 3. 
// -----------------------------

public struct AttackData
{
    public int m_id { get; set; }               // 攻撃ID
    public string m_name { get; set; }          // 攻撃名（アニメーションの名前）
    public float m_dist { get; set; }           // 攻撃者を起点とした攻撃の発動距離
    public float m_coolDown { get; set; }       // 攻撃後の硬直時間
}


public class AttackManager : MonoBehaviour
{
    public Animator m_animator;
    private bool m_isAttackAnimation = false;

    private int m_nowId = -1;
    private bool m_canAttack = false;   // 攻撃可能状態か
    private float m_coolDown = 0.0f;                // 次の攻撃までのクールダウン時間（秒数）
    private List<AttackData> m_attackLists = new List<AttackData>();


    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    void Update()
    {
        // アニメーション中ならスキップ
        if (m_isAttackAnimation) { return; }

        // クールダウン時間を減少
        m_coolDown　-= Time.deltaTime;
        if (m_coolDown < 0.0f)
        {
            m_coolDown = 0.0f;

            if (m_nowId == -1)
            {
                m_canAttack = true;
            }
        }
    }


    // 攻撃追加処理   ：   攻撃をリストに追加する
    public void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // 攻撃発生処理   →   引数：プレイヤーとの距離
    public int Action(float _dist)
    {
        if (!m_canAttack) { return -1; }      // クールダウンがまだなら早期リターン

        // 発動可能な攻撃を取得
        List<int> attacks = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].m_dist >= _dist)
            {
                attacks.Add(m_attackLists[i].m_id);
            }
        }
        if (attacks.Count == 0) { Debug.Log("距離外"); return -1; }  // 距離内に攻撃対象がいなかったら早期リターン

        // 取得した一覧からランダムで攻撃
        int randIndex = Random.Range(0, attacks.Count);
        m_nowId = m_attackLists[randIndex].m_id;
        m_coolDown = m_attackLists[randIndex].m_coolDown;

        m_isAttackAnimation = true;
        m_animator.SetBool(m_attackLists[m_nowId].m_name, m_isAttackAnimation);

        m_canAttack = false;

        return m_nowId;
    }


    // アニメーションの終了（他スクリプトのアニメーションから呼び出し）
    public void AnimationFin()
    {
        if (m_nowId < 0) { return; }
        m_isAttackAnimation = false;
        m_animator.SetBool(m_attackLists[m_nowId].m_name, m_isAttackAnimation);
        m_nowId = -1;
    }


    // リスト内の対象IDを探して、インデックス番号を返す
    private int SearchAttackId(int _attackId)
    {
        int result = 0;

        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].m_id == _attackId)
            {
                result = i;
                break;
            }
        }

        return result;
    }


    // _delta : true で経過時間取得　false で残り時間取得
    public float GetCoolDown(bool _delta = true)
    {
        if (_delta)
        {
            return m_attackLists[m_nowId].m_coolDown - m_coolDown;
        }
        else
        {
            return m_coolDown;
        }
    }


    //// インスタンスを初期化
    //private void Awake()
    //{
    //    m_instance = new AttackData();   
    //}
}
