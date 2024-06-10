using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject m_arrowObj; // ��̃I�u�W�F�N�g
    [SerializeField] GameObject m_characterObj; // ��l���̃I�u�W�F�N�g

    [SerializeField] float m_shotTime = 1.0f;
    [SerializeField] float m_speed = 60f;
    //[SerializeField] float m_rot = 90;

    private float m_shotTimeCnt;

    void Start()
    {
        m_shotTimeCnt = m_shotTime;
    }
    void Update()
    {
        m_shotTimeCnt -= Time.deltaTime;
        if(m_shotTimeCnt <=0) { m_shotTimeCnt=0; }

        if (Input.GetMouseButtonDown(0) && m_shotTimeCnt <= 0)
        {
            Shot();
            m_shotTimeCnt = m_shotTime;
        }
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
        Debug.Log("Arrow Shot!");
        //Destroy(arrow, 5);
    }
}
