using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_sliderObj;
    Slider m_sliderValue;

    private float m_soundValue;

    void Start()
    {
        if (!m_sliderObj)
        {
            Debug.Log("Slider is Null");
        }
        else
        {
            m_sliderValue = m_sliderObj.GetComponent<Slider>();
        }
    }

    void Update()
    {
        m_soundValue = m_sliderValue.value;
    }

    public float Getm_soundValue
    {
        get { return m_soundValue;  }
    }
}