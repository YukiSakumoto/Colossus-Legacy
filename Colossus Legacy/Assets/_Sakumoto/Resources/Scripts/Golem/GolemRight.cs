using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GolemRight : Golem
{
    public int m_nowAttackId = -1;
    private int m_nextAttackId = -1;

    public GameObject m_hand;
    private GameObject m_instantiateObj;
    private float m_protrusionNowTime = 0.0f;


    void Start()
    {
        attackManager = GetComponent<AttackManager>();

        attackManager.AddAttack(0, "SwingDown", 50.0f, 1.0f);
        attackManager.AddAttack(1, "SwingDown", 50.0f, 5.0f);
        attackManager.AddAttack(2, "Palms", 30.0f, 5.0f, true);
        attackManager.AddAttack(3, "Protrusion", 100.0f, 8.0f);
    }


    void Update()
    {
        if (!m_alive) { return; }

        if (m_instantiateObj)
        {
            m_protrusionNowTime += Time.deltaTime;
            if (m_protrusionNowTime <= 0.17f + 0.5f && m_protrusionNowTime >= 0.5f)
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y += 0.12f;
                m_instantiateObj.transform.position = pos;
            }
            else if (m_protrusionNowTime >= 0.83f + 0.5f)
            {
                Vector3 pos = m_instantiateObj.transform.position;
                pos.y -= 0.12f;
                m_instantiateObj.transform.position = pos;
            }
        }


        if (m_stop) { return; }

        m_nowAttackId = AttackSet(DistanceToTarget(), m_nextAttackId);

        if (m_nowAttackId == m_nextAttackId)
        {
            m_nextAttackId = -1;
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (m_weakCollider.enabled)
            {
                m_damageFlg = true;
            }
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


    private void ProtrusionActionOn()
    {
        m_protrusionNowTime = 0.0f;

        Vector3 targetPos = m_target.transform.position;
        targetPos.y -= 2.5f;

        Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        if (!m_hand) { Debug.Log("ハンドがないよ"); return; }
        m_instantiateObj = Instantiate(m_hand, targetPos, rot, this.transform);
    }


    private void ProtrusionActionOff()
    {
        if (!m_instantiateObj) { Debug.Log("ハンドがないよ"); return; }
        Destroy(m_instantiateObj);
    }


    public void SetNextAttackId(int _id) { m_nextAttackId = _id; }
}