using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public int hp = 100;

    void Start()
    {
        AttackManager attackManager = GetComponent<AttackManager>();
        AttackManager.AttackData instance = attackManager.intstance;

        

        // �U���̒ǉ�
        
    }

    void Update()
    {

    }


    // 
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
