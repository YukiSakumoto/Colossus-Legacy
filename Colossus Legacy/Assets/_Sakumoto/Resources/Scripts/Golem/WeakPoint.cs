using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public bool m_weakHit { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SwordAttack") ||
            other.gameObject.CompareTag("ArrowAttack"))
        {
            Debug.Log("�S�[�����̎�_���U�����ꂽ�I");
            m_weakHit = true;
        }
    }
}
