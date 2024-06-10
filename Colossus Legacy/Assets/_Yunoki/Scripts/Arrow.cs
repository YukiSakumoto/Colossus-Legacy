using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private string m_targetTag = "EnemyWeak";

    float arrowLiveTime = 8f;

    void Start()
    {
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        arrowLiveTime -= Time.deltaTime;
        if (arrowLiveTime <= 0)
        {
            Debug.Log("Arrow Hit!");
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        if(_other.gameObject.CompareTag(m_targetTag))
        {
            Debug.Log("Arrow is EnemyWeak Hit!");
            Destroy(gameObject);
        }

        //if (_other.gameObject.CompareTag("Untagged"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
