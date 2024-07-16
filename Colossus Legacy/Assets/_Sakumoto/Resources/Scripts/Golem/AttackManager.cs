using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public struct AttackData
{
    public int m_id { get; set; }               // 攻撃ID
    public string m_name { get; set; }          // 攻撃名（アニメーションの名前）
    public Vector2 m_dist { get; set; }         // 攻撃者を起点とした攻撃の発動距離 (x : 感知最長距離   y : 感知最短距離)
    public float m_coolDown { get; set; }       // 攻撃後の硬直時間
    public bool m_waitFlg { get; set; }         // 待機フラグ（他の攻撃を待つ時間などに使用）
}


public class AttackManager : MonoBehaviour
{
    public Animator m_animator;
    private bool m_isAttackAnimation = false;

    [SerializeField] private int m_nowId = -1;
    [SerializeField] private bool m_canAttack = false;   // 攻撃可能状態か
    [SerializeField] private float m_coolDown = 0.0f;    // 次の攻撃までのクールダウン時間（秒数）
    [SerializeField] private List<AttackData> m_attackLists = new List<AttackData>();


    void Start()
    {
        m_animator = GetComponent<Animator>();
    }


    void Update()
    {
        // アニメーション中ならスキップ
        if (m_isAttackAnimation) { return; }

        // クールダウン時間を減少
        m_coolDown -= Time.deltaTime;
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
    public void AddAttack(int _id, string _name, Vector2 _dist, float _coolDown, bool _flg = false)
    {
        AttackData attack = new();
        attack.m_id = _id;
        attack.m_name = _name;
        attack.m_coolDown = _coolDown;
        attack.m_waitFlg = _flg;

        // 追加された Vector2 の距離を降順に並び替え
        attack.m_dist = _dist;
        if (attack.m_dist.x < attack.m_dist.y)
        {
            Vector2 tmp = new(attack.m_dist.y, attack.m_dist.x);
            attack.m_dist = tmp;
        }

        AddAttack(attack);
    }


    // 攻撃の削除処理
    public void DeleteAttack(int _id)
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            AttackData attack = m_attackLists[i];
            if (attack.m_id == _id)
            {
                int idx = m_attackLists[i].m_id;
                m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
                m_attackLists.Remove(attack);
            }
        }
    }


    // 攻撃の削除処理
    public void DeleteAttack(string _name)
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            AttackData attack = m_attackLists[i];
            if (attack.m_name == _name)
            {
                int idx = m_attackLists[i].m_id;
                m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
                m_attackLists.Remove(attack);
            }
        }
    }


    public void DeleteAll()
    {
        ResetAnimation();
        m_attackLists.Clear();
    }


    // 攻撃発生処理   →   引数：プレイヤーとの距離
    public int Action(float _dist)
    {
        // 待機状態なら現在の攻撃IDを返してリターン
        if (m_attackLists[SearchAttackId(m_nowId)].m_waitFlg && m_nowId != -1) { return m_nowId; }

        if (!m_canAttack) { return m_nowId; }      // クールダウンがまだなら早期リターン

        // 発動可能な攻撃を取得
        List<int> attacks = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            // 感知最長 > ターゲットとの距離 > 感知最短 なら
            if (m_attackLists[i].m_dist.x >= _dist && m_attackLists[i].m_dist.y <= _dist)
            {
                attacks.Add(m_attackLists[i].m_id);
            }
        }
        if (attacks.Count == 0) { return -1; }  // 距離内に攻撃対象がいなかったら早期リターン


        // 取得した一覧からランダムで攻撃
        int randIndex = Random.Range(0, attacks.Count);
        m_nowId = attacks[randIndex];
        m_coolDown = m_attackLists[SearchAttackId(m_nowId)].m_coolDown;

        m_canAttack = false;


        if (!m_attackLists[SearchAttackId(m_nowId)].m_waitFlg)
        {
            m_isAttackAnimation = true;
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
        }

        return m_nowId;
    }

    public int Action(float _dist, int _id)
    {
        // 待機状態なら現在の攻撃IDを返してリターン
        //if (!_startFlg) { return m_nowId; }

        // クールダウンがまだなら早期リターン
        if (!m_canAttack) { return m_nowId; }

        // 指定した攻撃が範囲外ならリターン
        if (m_attackLists[SearchAttackId(_id)].m_dist.x < _dist ||
            m_attackLists[SearchAttackId(_id)].m_dist.y > _dist) { Debug.Log("うにんにん"); return -1; }

        m_nowId = _id;
        m_coolDown = m_attackLists[SearchAttackId(m_nowId)].m_coolDown;
        m_canAttack = false;

        // 待機状態が必要でないならそのままアニメーションを再生
        if (!m_attackLists[SearchAttackId(m_nowId)].m_waitFlg)
        {
            m_isAttackAnimation = true;
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, m_isAttackAnimation);
        }

        return m_nowId;
    }


    // アニメーションをスタートする
    public void AttackStart()
    {
        m_isAttackAnimation = true;
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, true);
    }


    // アニメーションの終了（他スクリプトのアニメーションから呼び出し）
    public void AnimationFin()
    {
        if (m_nowId < -1) { return; }
        m_isAttackAnimation = false;
        m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, false);
        m_nowId = -1;
    }


    // 現在のアニメーションを中断して別のアニメーションの再生
    public void ChangeAnimation(string _animationName, bool _flg, int _id = -1)
    {
        if (m_nowId != -1)
        {
            m_animator.SetBool(m_attackLists[SearchAttackId(m_nowId)].m_name, false);
        }
        m_animator.SetBool(_animationName, _flg);

        m_nowId = _id;
        if (m_nowId == -1)
        {
            m_isAttackAnimation = false;
        }
    }


    public void ResetAttackAnimation()
    {
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            int idx = m_attackLists[i].m_id;
            m_animator.SetBool(m_attackLists[SearchAttackId(idx)].m_name, false);
        }
        m_nowId = -1;
        m_isAttackAnimation = false;
    }


    // 全アニメーションを停止
    public void ResetAnimation()
    {
        ResetAttackAnimation();
        m_animator.SetBool("Damage", false);
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
            return m_attackLists[SearchAttackId(m_nowId)].m_coolDown - m_coolDown;
        }
        else
        {
            return m_coolDown;
        }
    }


    public string GetAttackName()
    {
        if (m_nowId != -1)
        {
            return m_attackLists[SearchAttackId(m_nowId)].m_name;
        }
        return "";
    }

    public string GetAttackName(int _id)
    {
        return m_attackLists[SearchAttackId(_id)].m_name;
    }


    public bool IsAttackRange(int _id, float _targetDist)
    {
        if (m_attackLists[SearchAttackId(_id)].m_dist.x < _targetDist ||
            m_attackLists[SearchAttackId(_id)].m_dist.y > _targetDist) { return false; }

        return true;
    }


    public List<int> GetAttackIdList()
    {
        List<int> idList = new List<int>();
        for (int i = 0; i < m_attackLists.Count; i++)
        {
            idList.Add(m_attackLists[SearchAttackId(i)].m_id);
        }

        return idList;
    }


    public Vector2 GetAttackDist(int _id)
    {
        return m_attackLists[SearchAttackId(_id)].m_dist;
    }



    public void SetAttackSpeed(float _val)
    {
        m_animator.SetFloat("AttackSpeed", _val);
    }
}
