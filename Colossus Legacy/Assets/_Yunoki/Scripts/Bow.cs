using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject arrowObj;
    [SerializeField] float shotTime = 1.0f;
    private float shotTimeCnt;
    [SerializeField] float speed = 10;

    [SerializeField] float rot=1;

    void Start()
    {
        shotTimeCnt = shotTime;
        // transform.localScale = new Vector3(10, 10, 10);
    }
    void Update()
    {
        shotTimeCnt += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shotTimeCnt > shotTime)
        {
            Shot();
            shotTimeCnt = 0;
        }

        //-----------------------------------------------------------------

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rot, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rot, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(0, 0, rot, Space.Self);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(0, 0, -rot, Space.Self);
        }

        if (Input.GetKey(KeyCode.F))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        //-----------------------------------------------------------------
    }
    void Shot()
    {
        GameObject arrow = Instantiate(arrowObj, transform.right* 0.4f, Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z + 90));
        
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.velocity = (transform.right * -speed);
        Destroy(arrow, 5);
    }
}
