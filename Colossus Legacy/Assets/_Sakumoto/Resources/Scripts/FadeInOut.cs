using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;
    public bool m_fadeFlg = false;

    void Start()
    {
        m_text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        m_fadeFlg = true;
    }
    

    void Update()
    {
        if (m_fadeFlg)
        {
            float alpha = Mathf.Sin(Time.time) * 0.4f + 0.7f;
            m_text.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }
}
