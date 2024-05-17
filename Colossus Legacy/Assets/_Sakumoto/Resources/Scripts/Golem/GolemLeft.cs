using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GolemLeft : Golem
{
    public List<Collider> attackColliders;

    public int m_nowAttackId = -1;
    private int m_nextAttackId = -1;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        attackManager.AddAttack(0, "SwingDown", 10.0f, 3.0f);
        attackManager.AddAttack(1, "SwingDown", 10.0f, 5.0f);
        attackManager.AddAttack(2, "Palms", 10.0f, 3.0f, true);
    }


    void Update()
    {
        if (m_stop) { return; }

        m_nowAttackId = AttackSet(m_nextAttackId);

        if (m_nowAttackId == m_nextAttackId)
        {
            m_nextAttackId = -1;
        }
    }


    public int AttackSet(int _id = -1)
    {
        int resultId = -1;

        if (_id == -1) resultId = attackManager.Action(10.0f);
        else resultId = attackManager.Action(10.0f, _id);

        return resultId;
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


    public bool GetStop() { return m_stop; }


    public void SetNextAttackId(int _id) { m_nextAttackId = _id; }


    // çUåÇîªíËê∂ê¨
    private void AttackOn()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = true;
        }
    }


    // çUåÇîªíËè¡ãé
    private void AttackOff()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
            attackManager.AnimationFin();
        }
    }
}
