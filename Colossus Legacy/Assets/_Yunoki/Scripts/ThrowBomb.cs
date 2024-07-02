using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] Image m_chargeImage;
    [SerializeField] GameObject m_bombPrefab;
    [SerializeField] float m_bombHeight = 9;
    [SerializeField] float m_speed = 300;
    [SerializeField] float m_bombChargeTime = 1;
    [SerializeField] float m_bombExpTime = 5;
    private const float m_chargeMaxTime = 1.5f;
    private const float m_bombMotionSetTime = 0.43f;
    private float m_chargeCancelSetTime = 0.3f;
    private float m_bombMotionTime = 0f;
    private float m_chargeTime = 0f;
    private float m_chargePower = 0f;
    private float m_chargeCancelTime = 0f;
    private float m_ratio = 0f;
    private bool m_bombThrowFlg = false;
    private bool m_keyPushFlg = false;
    private bool m_bombChargeFlg = false;
    private bool m_bombThrowCheckFlg = false;

    private CharacterManager m_characterManager;
    private GameStatusManager m_gameStatusManager;
    private float cnt = 0;

    private void Start()
    {
        if (!m_chargeImage)
        {
            Debug.Log("ThrowBomb: Image is Null");
        }
        else
        {
            m_chargeImage.fillAmount = 0f;
            m_chargeImage.color = Color.cyan;
        }

        if (!m_bombPrefab)
        {
            Debug.Log("ThrowBomb: bomb is Null");
        }

        m_characterManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        m_gameStatusManager = GameObject.FindWithTag("GameManager").GetComponent<GameStatusManager>();
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
        m_bombThrowCheckFlg = false;

        cnt -= Time.deltaTime;
        if (cnt <= 0) { cnt = 0; }

        m_ratio = m_chargeTime / m_chargeMaxTime;
        m_chargeImage.fillAmount = m_ratio;

        if (!m_characterManager.Getm_joyFlg)
        {
            if (!m_bombThrowFlg)
            {
                if (m_chargeCancelTime <= 0)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        m_keyPushFlg = true;
                    }

                    if (m_keyPushFlg && Input.GetMouseButton(1))
                    {
                        m_bombChargeFlg = true;
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        m_keyPushFlg = false;
                    }
                }
                else
                {
                    m_chargeCancelTime -= Time.deltaTime;
                }

                if (m_keyPushFlg)
                {
                    m_chargeTime += Time.deltaTime;
                    if (m_chargeTime >= m_chargeMaxTime)
                    {
                        m_chargeTime = m_chargeMaxTime;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        m_keyPushFlg = false;
                        m_bombChargeFlg = false;
                        m_chargeTime = 0f;
                        m_chargeCancelTime = m_chargeCancelSetTime;
                    }
                }
                else
                {
                    if (m_bombChargeFlg)
                    {
                        if (cnt <= 0)
                        {
                            m_bombThrowFlg = true;
                            m_bombChargeFlg = false;
                            m_bombThrowCheckFlg = true;
                            m_chargePower = m_chargeTime / m_chargeMaxTime;
                            m_chargeTime = 0f;
                            m_bombMotionTime = m_bombMotionSetTime;
                        }
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

                    if (rotateMathX > 0)
                    {
                        if (rotateMathX > mathSmall)
                        {
                            rotateMathX = mathBig - rotateMathX;
                        }
                    }
                    else
                    {
                        if (rotateMathX < -mathSmall)
                        {
                            rotateMathX = -mathBig - rotateMathX;
                        }
                    }

                    if (rotateMathZ <= mathSmall)
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
                    bombRb.AddForce(transform.up * m_bombHeight * m_chargePower, ForceMode.Impulse);
                    bombRb.AddForce(transform.forward * m_speed * m_chargePower);
                    bombClass.SetTime(m_bombExpTime);
                    bombClass.m_gameStatusManager = m_gameStatusManager;
                    Destroy(bomb, m_bombExpTime + bombExpTimeAdd);
                    cnt = m_bombChargeTime;
                    m_chargePower = 0f;
                    m_bombThrowFlg = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        
    }

    public bool Getm_bombThrowCheckFlg
    {
        get { return m_bombThrowCheckFlg; }
    }
}