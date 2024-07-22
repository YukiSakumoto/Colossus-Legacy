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


    [SerializeField] private Button m_deleteButton;
    [SerializeField] private GameObject m_reallyDeleteObj;
    [SerializeField] private Button m_reallyDeleteYesButton;
    [SerializeField] private Button m_reallyDeleteNoButton;


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

        if (!m_deleteButton)
        {
            Debug.Log("DeleteButton is Null");
        }
        else
        {
            m_deleteButton.onClick.AddListener(DeleteButtonClick);

            if (GameManager.Instance.clearFlg)
            {
                if (!m_deleteButton.gameObject.activeSelf)
                {
                    m_deleteButton.gameObject.SetActive(true);
                }
            }
            else
            {
                if (m_deleteButton.gameObject.activeSelf)
                {
                    m_deleteButton.gameObject.SetActive(false);
                }
            }
        }

        if (m_reallyDeleteObj)
        {
            if (m_reallyDeleteObj.activeSelf)
            {
                m_reallyDeleteObj.SetActive(false);
            }
            m_reallyDeleteYesButton.onClick.AddListener(ReallyDeleteYesClick);
            m_reallyDeleteNoButton.onClick.AddListener(ReallyDeleteNoClick);
        }
    }

    void Update()
    {
        //m_soundValue = m_sliderValue.value;
        GameManager.Instance.soundVolume = m_soundSliderValue.value;
        GameManager.Instance.BGMVolume = m_BGMSliderValue.value;

        if (GameStatusManager.Instance.m_debugFlg)
        {
            if (GameManager.Instance.clearFlg)
            {
                m_deleteButton.gameObject.SetActive(true);
            }
            else
            {
                m_deleteButton.gameObject.SetActive(false);
            }
        }
    }

    void DeleteButtonClick()
    {
        m_reallyDeleteObj.SetActive(true);
    }

    void ReallyDeleteYesClick()
    {
        GameManager.Instance.SetclearFlg(false);
        
        m_reallyDeleteObj.SetActive(false);
        m_deleteButton.gameObject.SetActive(false);
    }

    void ReallyDeleteNoClick()
    {
        m_reallyDeleteObj.SetActive(false);
    }

    //public float Getm_soundValue
    //{
    //    get { return m_soundValue;  }
    //}
}