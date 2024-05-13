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
    // 攻撃のデータ系
    public class AttackData
    {
        public int id { get; set; }             // 攻撃ID
        public bool enable { get; set; }        // 攻撃が発生しているか
        public bool isAttack { get; set; }      // 攻撃判定があるか
        public float coolTime { get; set; }     // クールタイム
    }
    public AttackData intstance { get; private set; }

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

        m_attackLists[id].enable = true;
        m_coolDown = m_attackLists[id].coolTime;
    }


    // リスト内から引数IDを探して、インデックス番号を返す
    private int SearchAttackId(int _attackId)
    {
        int result = 0;

        for (int i = 0; i < m_attackLists.Count; i++)
        {
            if (m_attackLists[i].id == _attackId)
            {
                result = i;
                break;
            }
        }

        return result;
    }


    // AttackManager のインスタンスを初期化
    private void Awake()
    {
        intstance = new AttackData();
    }
}
