using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleGolemLight : MonoBehaviour
{
    [SerializeField] Light m_light;
    [SerializeField] Image m_title;

    void Update()
    {
        float sinA = Mathf.Sin(Time.time);
        float sinB = Mathf.Sin(Time.time * 0.4f);
        float sinC = Mathf.Sin(Time.time * 0.8f);

        float speed = sinA * sinB * sinC;
        speed = (speed + 1.0f) * 0.5f;

        m_light.intensity = speed;

        Color col = new Color(1.0f, 0.9f, 0.95f, speed);
        m_title.color = col;
    }
}
