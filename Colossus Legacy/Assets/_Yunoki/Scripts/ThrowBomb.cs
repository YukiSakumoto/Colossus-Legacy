using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using System;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] GameObject m_bombPrefab;
    [SerializeField] GameStatusManager m_gameStatusManager;
    [SerializeField] float m_bombHeight = 9;
    [SerializeField] float m_speed = 300;
    [SerializeField] float m_bombChargeTime = 1;
    [SerializeField] float m_bombExpTime = 5;
    float m_bombMotionTime = 0f;
    private const float m_bombMotionSetTime = 0.43f; 
    private bool m_bombThrowFlg = false;

    private float cnt = 0;

    private void Start()
    {
        if(!m_bombPrefab)
        {
            Debug.Log("ThrowBomb: bomb is Null");
        }

        if (!m_gameStatusManager)
        {
            Debug.Log("ThrowBomb: GameStatusManager is Null");
        }
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    m_gameStatusManager.DamagePlayerBeam();
        //    Debug.Log("ÉrÅ[ÉÄçUåÇ");
        //}
        //if (Input.GetKey(KeyCode.X))
        //{
        //    m_gameStatusManager.DamagePlayerBomb();
        //    Debug.Log("îöíeçUåÇ");
        //}
        //if (Input.GetKey(KeyCode.C))
        //{
        //    m_gameStatusManager.DamagePlayerDown();
        //    Debug.Log("âüÇµÇ¬Ç‘ÇµçUåÇ");
        //}
        //if (Input.GetKey(KeyCode.V))
        //{
        //    m_gameStatusManager.DamagePlayerPressHand();
        //    Debug.Log("çáè∂çUåÇ");
        //}
        //if (Input.GetKey(KeyCode.B))
        //{
        //    m_gameStatusManager.DamagePlayerPushUP();
        //    Debug.Log("ÉJÉ`è„Ç∞çUåÇ");
        //}

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
                float mathBig = 2f;
                float mathSmall = 1f;

                float rotateMathX = transform.rotation.y * mathBig;
                float rotateMathZ = Math.Abs(transform.rotation.y) * mathBig;

                if(rotateMathX > 0) 
                {
                    if(rotateMathX > mathSmall)
                    {
                        rotateMathX = mathBig - rotateMathX;
                    }
                }
                else
                {
                    if(rotateMathX < -mathSmall)
                    {
                        rotateMathX = -mathBig - rotateMathX;
                    }
                }

                if(rotateMathZ <= mathSmall)
                {
                    rotateMathZ = mathSmall - rotateMathZ;
                }
                else
                {
                    rotateMathZ = (-rotateMathZ) + mathSmall;
                }
                

                float initialPosAddX = 1f * rotateMathX;
                float initialPosAddY = 1f;
                float initialPosAddZ = 1f * rotateMathZ;
                float bombExpTimeAdd = 0.3f; 
                Vector3 initialPosition = transform.position;

                initialPosition.x += initialPosAddX; 
                initialPosition.y += initialPosAddY;
                initialPosition.z += initialPosAddZ;
                Bomb bombClass = m_bombPrefab.GetComponent<Bomb>();
                GameObject bomb = Instantiate(m_bombPrefab, initialPosition, Quaternion.identity);
                Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
                bombRb.AddForce(transform.up * m_bombHeight, ForceMode.Impulse);
                bombRb.AddForce(transform.forward * m_speed);
                bombClass.SetTime(m_bombExpTime);
                bombClass.m_gameStatusManager = m_gameStatusManager;
                Destroy(bomb, m_bombExpTime + bombExpTimeAdd);
                cnt = m_bombChargeTime;
                m_bombThrowFlg = false;
            }
        }
    }
}