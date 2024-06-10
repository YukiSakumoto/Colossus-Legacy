using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private string m_targetTag = "EnemyWeak";

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag==(m_targetTag))
        {
            Debug.Log("Arrow Hit!");
        }
    }
}
