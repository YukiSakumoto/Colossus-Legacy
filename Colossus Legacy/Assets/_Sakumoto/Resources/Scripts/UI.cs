using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvas;


    private void Start()
    {
        m_canvas.alpha = 0.0f;
    }


    void Update()
    {
        if (!m_canvas) { return; }

        if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.Battle)
        {
            m_canvas.alpha += Time.deltaTime;
        }
        else if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.PlayerWin)
        {
            m_canvas.alpha -= Time.deltaTime * 0.5f;
        }
    }
}
