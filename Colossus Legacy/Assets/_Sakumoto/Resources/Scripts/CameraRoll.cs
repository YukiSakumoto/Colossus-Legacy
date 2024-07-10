using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoll : MonoBehaviour
{
    [SerializeField] private GameObject m_target;
    [SerializeField] private float m_speed = 10.0f;

    [SerializeField] private float m_finTime = 5.0f;

    void Update()
    {
        if (GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.TreasureGet ||
            GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.Score ||
            GameEvent.Instance.m_nowEvent == GameEvent.GameEventState.GameFin)
        {
            transform.RotateAround
                (m_target.transform.position, Vector3.up, m_speed * Time.deltaTime);

            m_finTime -= Time.deltaTime;
            if (m_finTime <= 0.0f)
            {
                GameEvent.Instance.ChangeEvent(GameEvent.GameEventState.Score);
            }
        }
    }
}
