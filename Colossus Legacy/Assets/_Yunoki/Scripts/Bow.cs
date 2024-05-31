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
        // transform.localScale = new Vector3(10, 10, 10);
    }
    void Update()
    {
        m_shotTimeCnt += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && m_shotTimeCnt > m_shotTime)
        {
            Shot();
            m_shotTimeCnt = 0;
        }

        //-----------------------------------------------------------------

        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Rotate(0, m_rot, 0, Space.Self);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Rotate(0, -m_rot, 0, Space.Self);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Rotate(0, 0, m_rot, Space.Self);
        //}

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Rotate(0, 0, -m_rot, Space.Self);
        //}

        //if (Input.GetKey(KeyCode.F))
        //{
        //    transform.rotation = Quaternion.Euler(0, 90, 0);
        //}

        //-----------------------------------------------------------------
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
