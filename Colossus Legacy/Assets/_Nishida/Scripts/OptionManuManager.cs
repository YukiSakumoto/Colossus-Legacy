using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_soundSliderObj;
    [SerializeField] private GameObject m_BGMSliderObj;
    private Slider m_soundSliderValue;
    private Slider m_BGMSliderValue;

    //private float m_soundValue;

    void Start()
    {
        if (!m_soundSliderObj)
        {
            Debug.Log("Slider is Null");
        }
        else
        {
            m_soundSliderValue = m_soundSliderObj.GetComponent<Slider>();
            m_soundSliderValue.maxValue = GameManager.Instance.GetMaxSoundVol();
        }

        if (!m_BGMSliderObj)
        {
            Debug.Log("Slider is Null");
        }
        else
        {
            m_BGMSliderValue = m_BGMSliderObj.GetComponent<Slider>();
            m_BGMSliderValue.maxValue = GameManager.Instance.GetMaxBGMVol();
        }
    }

    void Update()
    {
        //m_soundValue = m_sliderValue.value;
        GameManager.Instance.soundVolume = m_soundSliderValue.value;
        GameManager.Instance.BGMVolume = m_BGMSliderValue.value;
    }

    //public float Getm_soundValue
    //{
    //    get { return m_soundValue;  }
    //}
}