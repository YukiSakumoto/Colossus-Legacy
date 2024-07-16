using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightWeak : MonoBehaviour
{
    Light m_light;
    [SerializeField] float m_maxIntensity = 5.0f;
    [SerializeField] float m_speed = 1.0f;

    void Start()
    {
        m_light = GetComponent<Light>();
        m_light.intensity = m_maxIntensity;
    }

    
    void Update()
    {
        float ratio = Mathf.Sin(Time.time * m_speed) * 0.5f + 0.7f;
        m_light.intensity = ratio * m_maxIntensity;
    }


    private void OnDisable()
    {
        m_light.intensity = m_maxIntensity;
    }
}
