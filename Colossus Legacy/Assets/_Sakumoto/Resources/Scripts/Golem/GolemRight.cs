using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GolemRight : Golem
{
    public int m_nowAttackId = -1;
    private int m_nextAttackId = -1;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        attackManager.AddAttack(0, "SwingDown", 50.0f, 1.0f);
        attackManager.AddAttack(1, "SwingDown", 50.0f, 5.0f);
        attackManager.AddAttack(2, "Palms", 30.0f, 5.0f, true);
    }


    void Update()
    {
        if (!m_alive) { return; }
        if (m_stop) { return; }

        m_nowAttackId = AttackSet(DistanceToTarget(), m_nextAttackId);

        if (m_nowAttackId == m_nextAttackId)
        {
            m_nextAttackId = -1;
        }
    }


    public bool AttackWait()
    {
        if (m_nowAttackId == 2)
        {
            m_stop = true;
        }

        return m_stop;
    }


    public void AttackStart()
    {
        m_stop = false;
        m_nextAttackId = -1;
        attackManager.AttackStart();
    }


    public void SetNextAttackId(int _id) { m_nextAttackId = _id; }
}