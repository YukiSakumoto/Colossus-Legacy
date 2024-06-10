using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject m_arrowObj;
    [SerializeField] GameObject m_characterObj;

    [SerializeField] float m_shotTime = 1.0f;
    [SerializeField] float m_speed = 30;
    [SerializeField] float m_rot = 90;

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
    void Shot()
    {
        GameObject arrow = Instantiate(m_arrowObj, m_characterObj.transform.right, Quaternion.Euler(
            m_characterObj.transform.eulerAngles.x, m_characterObj.transform.eulerAngles.y, m_characterObj.transform.eulerAngles.z + m_rot));
        
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.velocity = (m_characterObj.transform.right * -m_speed);
        Debug.Log("Arrow Shot!");
        //Destroy(arrow, 5);
    }
}
