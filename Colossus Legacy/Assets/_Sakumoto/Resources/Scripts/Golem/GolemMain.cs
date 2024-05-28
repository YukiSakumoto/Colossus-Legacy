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
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("ダメージを受けました！");
            m_damageFlg = true;
        }
    }
}
