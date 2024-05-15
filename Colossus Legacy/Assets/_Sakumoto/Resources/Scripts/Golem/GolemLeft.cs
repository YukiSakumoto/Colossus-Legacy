using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemLeft : Golem
{
    private AttackManager attackManager;
    public List<Collider> attackColliders;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        AttackData attackData = new AttackData();
        {
            attackData.m_id = 0;
            attackData.m_name = "SwingDown";
            attackData.m_dist = 10.0f;
            attackData.m_coolDown = 10.0f;
            attackManager.AddAttack(attackData);
        }

        {
            attackData.m_id = 1;
            attackData.m_name = "SwingDown";
            attackData.m_dist = 10.0f;
            attackData.m_coolDown = 10.0f;
            attackManager.AddAttack(attackData);
        }

        {
            attackData.m_id = 2;
            attackData.m_name = "SwingDown";
            attackData.m_dist = 10.0f;
            attackData.m_coolDown = 5.0f;
            attackManager.AddAttack(attackData);
        }
    }


    void Update()
    {
        attackManager.Action(10.0f);
    }



    private void AttackOn()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = true;
        }
    }


    private void AttackOff()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = false;
            attackManager.AnimationFin();
        }
    }

}
