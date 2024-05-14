using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private AttackManager attackManager;

    public int hp = 100;



    void Start()
    {
        attackManager = GetComponent<AttackManager>();
        AttackManager.AttackData attackData = attackManager.m_instance;

        attackData.m_id = 0;
        attackData.m_coolTime = 120.0f;
        attackManager.AddAttack(attackData);
    }

    void Update()
    {
        
    }


    // 
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
