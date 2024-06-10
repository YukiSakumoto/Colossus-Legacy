using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombHeight = 9;
    [SerializeField] float speed = 300;
    [SerializeField] float bombChargeTime = 1;
    [SerializeField] float bombExpTime = 5;

    private float cnt = 0;
    
    void Update()
    {
        cnt -= Time.deltaTime;
        if (cnt <= 0) { cnt = 0; }

        if (Input.GetMouseButtonDown(1))
        {
            if (cnt <= 0)
            {
                GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
                Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
                bombRb.AddForce(transform.up * bombHeight, ForceMode.Impulse);
                bombRb.AddForce(transform.forward * speed);
                Destroy(bomb, bombExpTime);
                cnt = bombChargeTime;
            }
        }
    }
}
