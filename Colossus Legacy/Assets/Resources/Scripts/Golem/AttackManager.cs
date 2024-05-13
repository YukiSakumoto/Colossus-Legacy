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
        public int id = 0;                  // 攻撃ID
        public bool isAttack = false;       // 攻撃している状態か
        public bool enable = false;         // 攻撃判定
        public float coolTime = 0.0f;       // クールタイム
    }

    private bool m_canAttack = false;     // 攻撃可能状態かの判定用
    private float m_coolDown = 0.0f;      // 次の攻撃までのクールダウン時間
    private List<AttackData> m_attackLists = new List<AttackData>();
    
    // 攻撃追加処理   ：   攻撃をリストに追加する
    void AddAttack(AttackData _attack)
    {
        m_attackLists.Add(_attack);
    }


    // 攻撃発生処理   ：   引数の ID の攻撃を発生させてクールダウン時間を設定
    void Action(int _attackId = 0)
    {
        int id = SearchAttackId(_attackId);

        m_coolDown = m_attackLists[id].coolTime;
    }


    // リスト内の対象IDを探して、要素数を返す
    int SearchAttackId(int _attackId)
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
}
