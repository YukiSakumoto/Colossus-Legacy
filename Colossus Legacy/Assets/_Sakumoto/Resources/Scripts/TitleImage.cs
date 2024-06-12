using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleImage : MonoBehaviour
{
    public RectTransform m_rectTransform;
    void Start()
    {
    }

    void Update()
    {
        Vector3 tmpVec = m_rectTransform.position;
        tmpVec.y = 100.0f + (15.0f * (Mathf.Sin(Time.time) / 2 + 0.5f));
        tmpVec.y *= m_rectTransform.localScale.y;
        m_rectTransform.position = tmpVec;
    }
}
