using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private MeshRenderer mr;
    [SerializeField] private float m_maxAlpha = 0.5f;
    [SerializeField] private float m_minAlpha = 0.1f;
    [SerializeField] private float m_speed = 1.0f;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    protected void Update()
    {
        float alpha = m_minAlpha + (Mathf.Sin(Time.time * m_speed) * 0.5f + 0.5f) * (m_maxAlpha - m_minAlpha);
        mr.material.color = 
            new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, alpha);
    }
}
