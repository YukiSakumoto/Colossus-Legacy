using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeClear : MonoBehaviour
{
    private Image m_fade;
    private RectTransform m_trans;

    [SerializeField] private float m_fadeTime = 1.0f;
    private float m_nowTime = 0.0f;


    private void OnEnable()
    {
        m_fade = GetComponent<Image>();

        Color color = m_fade.color;
        color.a = 0.0f;
        m_fade.color = color;

        m_nowTime = 0.0f;

        m_trans = GetComponent<RectTransform>();
        m_trans.localScale = Vector3.zero;
    }


    void Update()
    {
        if (m_fade.color.a > 1.0f) { return; }

        Color color = m_fade.color;
        float ratio = m_nowTime / m_fadeTime;
        color.a = ratio;
        m_nowTime += Time.deltaTime;

        m_fade.color = color;

        m_trans.localScale = new Vector3(ratio, ratio, ratio);
    }
}
