using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSoundPlay : MonoBehaviour
{
    [SerializeField] private GameObject m_titleSceneManagerObj;
    [SerializeField] private GameObject m_sliderObj;
    
    [SerializeField] private AudioSource m_BGM;

    Slider m_sliderValue;

    TitleSceneManager m_titleSceneManagerClass;

    private float m_soundVolume = 0.5f;
    private float m_BGMVolume = 1.0f;

    private void Start()
    {
        if(!m_titleSceneManagerObj)
        {
            Debug.Log("TitleSceneManager is Null");
        }
        else
        {
            m_titleSceneManagerClass = m_titleSceneManagerObj.GetComponent<TitleSceneManager>();
        }

        if (!m_sliderObj)
        {
            Debug.Log("Slider is Null");
        }
        else
        {
            m_sliderValue = m_sliderObj.GetComponent<Slider>();
        }

        m_BGMVolume = m_BGM.volume;
        m_BGM.volume = m_BGMVolume * m_soundVolume;
        PlayBGM();
    }

    private void Update()
    {
        if (m_titleSceneManagerClass.Getm_optionManuFlg)
        {
            VolumeControl();
        }
    }

    void VolumeControl()
    {
        m_soundVolume = m_sliderValue.value;
        m_BGM.volume = m_BGMVolume * m_soundVolume;
    }

    public void PlayBGM()
    {
        m_BGM.Play();
    }

    public float Getm_soundVolume
    {
        get { return m_soundVolume; }
    }
}
