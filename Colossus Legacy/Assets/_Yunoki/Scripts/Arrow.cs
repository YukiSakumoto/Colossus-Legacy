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
<<<<<<< HEAD
            Debug.Log("Arrow Hit!");
=======
            Destroy(gameObject);
        }

        if (GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
            Vector3 initialRotationOffset = new Vector3(90, 0, 0);
            transform.rotation *= Quaternion.Euler(initialRotationOffset);
>>>>>>> origin/main
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
