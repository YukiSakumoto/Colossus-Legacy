using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCollider : MonoBehaviour
{
    [SerializeField] ChestOpen chest;
    string m_targetTag = "Player";
    bool m_openFlg = false;
    private void OnTriggerEnter(Collider _other)
    {
        if (!m_openFlg)
        {
            if (_other.gameObject.CompareTag(m_targetTag))
            {
                chest.Open();
                m_openFlg = true;
            }
        }
    }
}
