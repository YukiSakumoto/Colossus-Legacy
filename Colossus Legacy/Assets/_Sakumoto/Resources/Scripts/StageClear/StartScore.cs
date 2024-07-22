using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScore : MonoBehaviour
{
    [SerializeField] private GameObject m_clear;
    [SerializeField] private GameObject m_playerUI;

    void Start()
    {
        m_clear.SetActive(false);
    }

    private void Update()
    {
        if (GameStatusManager.Instance.m_debugFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.Score);
            }
        }

        if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.Score)
        {
            m_clear.SetActive(true);
            m_playerUI.SetActive(false);
        }
    }
}
