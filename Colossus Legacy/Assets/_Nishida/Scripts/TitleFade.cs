using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleFade : MonoBehaviour
{
    [SerializeField] private Image m_backGround;
    [SerializeField] private Image m_logo;
    private const float m_displayStartTime = 0.5f;
    private const float m_displayEndTime = 1.5f;
    private const float m_hiddenStartTime = 3.5f;
    private const float m_hiddenEndTime = 4.5f;
    private const float m_titleDisplayTime = 5f;
    private const float m_alphaMax = 1f;
    public float m_logoAlpha;
    private float m_time;
    Color m_logoColor;
    
    // Start is called before the first frame update
    void Start()
    {
        m_backGround.enabled = true;
        m_logo.enabled = true;
        m_logoAlpha = 0f;
        m_time = 0f;
        m_logoColor = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_time < m_titleDisplayTime)
        {
            if (Input.GetMouseButton(0))
            {
                m_time = m_titleDisplayTime;
            }

            m_time += Time.deltaTime;
            if(m_time <= m_displayStartTime)
            {
                m_logoAlpha = 0f;
            }
            else if (m_time > m_displayStartTime && m_time < m_displayEndTime)
            {
                m_logoAlpha += Time.deltaTime;
                if (m_logoAlpha >= m_alphaMax)
                {
                    m_logoAlpha = m_alphaMax;
                }
            }
            else if (m_time < m_hiddenStartTime)
            {
                m_logoAlpha = m_alphaMax;
            }
            else if (m_time < m_hiddenEndTime)
            {
                m_logoAlpha -= Time.deltaTime;
                if (m_logoAlpha <= 0f)
                {
                    m_logoAlpha = 0f;
                }
            }
            m_logoColor.a = m_logoAlpha;
            m_logo.color = m_logoColor;
        }
        else
        {
            m_backGround.enabled = false;
            m_logo.enabled = false;
        }
    }
}
