using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaLaser : MonoBehaviour
{
    [SerializeField] private float m_time = 1.0f;
    private float m_deltaTime;

    [SerializeField] private Vector3 m_minScale = new Vector3();
    [SerializeField] private Vector3 m_maxScale = new Vector3();

    private void Start()
    {
        m_deltaTime = 0.0f;
    }

    void Update()
    {
        Vector3 scale = this.transform.localScale;

        scale.x = m_deltaTime / m_time;
        if (scale.x > m_time) { m_deltaTime = 0.0f; scale.x = 0.0f; }

        scale.x *= m_maxScale.x;
        if (scale.x < m_minScale.x) { scale = m_minScale; }

        this.transform.localScale = scale;

        m_deltaTime += Time.deltaTime;
    }
}
