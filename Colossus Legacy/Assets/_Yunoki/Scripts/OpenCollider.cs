using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCollider : MonoBehaviour
{
    [SerializeField] ChestOpen chest;
    bool m_openFlg = false;
    private void Update()
    {
        if (!m_openFlg)
        {
            if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.TreasureGet)
            {
                chest.Open();
                m_openFlg = true;
            }
        }
    }
}
