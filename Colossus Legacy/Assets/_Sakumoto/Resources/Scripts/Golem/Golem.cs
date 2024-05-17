using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Golem : MonoBehaviour
{
    protected AttackManager attackManager;

    [SerializeField] private GolemLeft m_golemLeft;     // Unityでアタッチ済み
    [SerializeField] private GolemRight m_golemRight;   // Unityでアタッチ済み

    public bool m_stop = false;     // 両腕を同時に合わせるためのフラグ

    public int hp = 100;

    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        m_golemLeft = GameObject.Find("Golem_Left").GetComponent<GolemLeft>();
        m_golemRight = GameObject.Find("Golem_Right").GetComponent<GolemRight>();
    }

    void Update()
    {
        // Null チェック
        if (!m_golemLeft || !m_golemRight)
        {
            if (!m_golemLeft) { Debug.Log("LNull!!"); }
            if (!m_golemRight) { Debug.Log("RNull!!"); }
            
            return;
        }

        if (m_golemLeft.AttackWait())
        {
            m_golemRight.AttackSet(2);
        }
        m_golemRight.AttackWait();
        {
            m_golemLeft.AttackSet(2);
        }

        if (m_golemLeft.GetStop() && m_golemRight.GetStop())
        {
            Debug.Log("OK!");
            m_golemLeft.AttackStart();
            m_golemRight.AttackStart();
        }
    }
}
