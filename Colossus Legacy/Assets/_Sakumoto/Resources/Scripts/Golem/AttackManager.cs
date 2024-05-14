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

public class AttackManager : MonoBehaviour
{
    public class AttackData
    {
        public int m_id { get; set; }             // 攻撃ID
        public bool m_enable { get; set; }        // 攻撃モーションをしているか
        public bool m_isAttack { get; set; }      // 攻撃判定出現状態か
        public float m_coolTime { get; set; }     // クールタイム
        public float m_startTime {  get; set; }   // 攻撃判定開始フレーム
        public float m_endTime { get; set; }      // 攻撃判定終了フレーム
    }
    public AttackData m_instance { get; set; }

    private bool m_canAttack = false;     // 攻撃可能状態かの判定用
    private float m_coolDown = 0.0f;      // 次の攻撃までのクールダウン時間
    private List<AttackData> m_attackLists = new List<AttackData>();
    
    // 攻撃追加処理   ：   攻撃をリストに追加する
    public void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // 攻撃発生処理   ：   引数の ID の攻撃を発生させてクールダウン時間を設定
    public void Action(int _attackId = 0)
    {
        int id = SearchAttackId(_attackId);

        m_coolDown = m_attackLists[id].m_coolTime;
    }


    // リスト内の対象IDを探して、インデックス番号を返す
    int SearchAttackId(int _attackId)
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


    // インスタンスを初期化
    private void Awake()
    {
        m_instance = new AttackData();   
    }
}
