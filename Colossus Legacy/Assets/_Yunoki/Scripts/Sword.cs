using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private string m_targetTag = "EnemyWeak";

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == (m_targetTag))
        {
            //Destroy(_other.gameObject);
            Debug.Log("Sword Hit!");
        }
    }
}
