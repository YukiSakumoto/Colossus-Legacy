using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private string m_targetWeakTag = "EnemyWeak";
    [SerializeField] private string m_targetBodyTag = "Enemy";

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == (m_targetWeakTag))
        {
            //Destroy(_other.gameObject);
            Debug.Log("Sword Hit! Enemy Ni Damage!");
        }

        if (_other.gameObject.tag == (m_targetBodyTag))
        {
            //Destroy(_other.gameObject);
            Debug.Log("Sword Deflected! Damage ga Hairanaiyo!");
        }
    }
}
