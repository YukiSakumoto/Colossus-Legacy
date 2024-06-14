using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using System;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] GameObject m_bombPrefab;
    [SerializeField] float m_bombHeight = 9;
    [SerializeField] float m_speed = 300;
    [SerializeField] float m_bombChargeTime = 1;
    [SerializeField] float m_bombExpTime = 5;
    float m_bombMotionTime = 0f;
    private const float m_bombMotionSetTime = 0.43f; 
    private bool m_bombThrowFlg = false;

    private float cnt = 0;
    
    void Update()
    {
        cnt -= Time.deltaTime;
        if (cnt <= 0) { cnt = 0; }

        if (!m_bombThrowFlg)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (cnt <= 0)
                {
                    m_bombThrowFlg = true;
                    m_bombMotionTime = m_bombMotionSetTime;
                }
            }
        }
        else
        {
            m_bombMotionTime -= Time.deltaTime;
            if (m_bombMotionTime <= 0)
            {
                float rotateMathX = Math.Abs(transform.rotation.y);
                float rotateMathZ = Math.Abs(transform.rotation.y);
                float initialPosAddX = 0.3f * rotateMathX;
                float initialPosAddY = 0.5f;
                float initialPosAddZ = 1f * rotateMathZ;
                Vector3 initialPosition = transform.position;
                initialPosition.x += initialPosAddX; 
                initialPosition.y += initialPosAddY;
                initialPosition.z += initialPosAddZ;
                GameObject bomb = Instantiate(m_bombPrefab, initialPosition, Quaternion.identity);
                Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
                bombRb.AddForce(transform.up * m_bombHeight, ForceMode.Impulse);
                bombRb.AddForce(transform.forward * m_speed);
                Destroy(bomb, m_bombExpTime);
                cnt = m_bombChargeTime;
                m_bombThrowFlg = false;
            }
        }
    }
}