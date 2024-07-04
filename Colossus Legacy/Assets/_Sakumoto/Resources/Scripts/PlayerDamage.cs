using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] List<Golem> m_golems;

    GameStatusManager m_statusManager;


    void Start()
    {
        m_statusManager = GameObject.FindWithTag("GameManager").GetComponent<GameStatusManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            for (int i = 0; i < m_golems.Count; i++)
            {
                if (!m_golems[i]) { continue; }

                if (CheckChildren(i, m_golems[i].transform, other.transform))
                {
                    break;
                }
            }
        }
    }


    private bool CheckChildren(int _golemIdx, Transform _parent, Transform _other)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);
            if (child == _other)
            {
                if (m_golems[_golemIdx].GetNowAttackName() == "SwingDown")
                {
                    m_statusManager.DamagePlayerDown();
                }
                else if (m_golems[_golemIdx].GetNowAttackName() == "Palms")
                {
                    m_statusManager.DamagePlayerPressHand();
                }
                else if (m_golems[_golemIdx].GetNowAttackName() == "Protrusion")
                {
                    m_statusManager.DamagePlayerPushUP();
                }
                else if (m_golems[_golemIdx].GetNowAttackName() == "Rampage")
                {
                    m_statusManager.DamagePlayerDown();
                }
                else if (m_golems[_golemIdx].GetNowAttackName() == "BigLaser")
                {
                    m_statusManager.DamagePlayerBeam();
                }

                return true;
            }

            // 子オブジェクトがさらに子オブジェクトを持っている場合、再帰的に探索
            if (CheckChildren(_golemIdx, child, _other))
            {
                return true;
            }
        }
        return false;
    }
}
