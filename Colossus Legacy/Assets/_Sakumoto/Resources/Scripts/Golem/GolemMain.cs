using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMain : Golem
{

    void Start()
    {
        attackManager = GetComponent<AttackManager>();
    }


    void Update()
    {
        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("�_���[�W���󂯂܂����I");
            m_damageFlg = true;
        }
    }
}
