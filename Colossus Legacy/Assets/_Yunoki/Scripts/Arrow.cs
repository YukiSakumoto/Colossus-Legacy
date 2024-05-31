using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private string m_targetTag = "EnemyWeak";

    void Start()
    {
        // transform.localScale = new Vector3(10, 10, 10);
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag==(m_targetTag))
        {
            Debug.Log("Arrow Hit!");
            //Destroy(collision.gameObject);
            //Destroy(gameObject);
        }
    }
}
