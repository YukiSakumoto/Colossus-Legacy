using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject m_arrowObj; // 矢のオブジェクト
    [SerializeField] GameObject m_characterObj; // 主人公のオブジェクト

    [SerializeField] float m_shotTime = 1.0f;
    [SerializeField] float m_speed = 60f;

    [SerializeField] private GameStatusManager m_gameStatusManager;
    //[SerializeField] float m_rot = 90;

    private float m_shotTimeCnt;

    void Start()
    {
        m_shotTimeCnt = m_shotTime;
        // transform.localScale = new Vector3(10, 10, 10);
    }
    void Update()
    {

    }
    public void Shot()
    {
        float initialPosAddX = -0.1f;
        float initialPosAddY = 1.5f;
        Vector3 initialPosition = m_characterObj.transform.position;
        Quaternion initialRotation = m_characterObj.transform.rotation;

        initialPosition.x -= initialPosAddX;
        initialPosition.y += initialPosAddY;

        Vector3 initialRotationOffset = new Vector3(90, 0, 0);
        initialRotation *= Quaternion.Euler(initialRotationOffset);

        GameObject arrow = Instantiate(m_arrowObj, initialPosition, initialRotation);
        
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.velocity = (m_characterObj.transform.forward * m_speed);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.m_gameStatusManager = m_gameStatusManager;
        Debug.Log("Arrow Shot!");
        //Destroy(arrow, 5);
    }
}
