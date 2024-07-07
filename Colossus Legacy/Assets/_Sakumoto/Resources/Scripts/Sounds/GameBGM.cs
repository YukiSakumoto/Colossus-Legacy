using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGM : Singleton<GameBGM>
{
    private AudioSource m_BGM;

    [SerializeField] private AudioClip m_beforeBGM;
    [SerializeField] private AudioClip m_battleBGM;
    [SerializeField] private AudioClip m_winBGM;

    [SerializeField] private float m_maxVol = 0.3f;
    [SerializeField] private float m_fadeTime = 0.5f;
    private float m_time = 0.0f;

    public enum BGMState
    {
        None,
        Stop,
        Before,
        Battle,
        Win
    }
    [SerializeField] private BGMState m_nowState = BGMState.None;
    private BGMState m_nextState = BGMState.None;


    public enum BGMFade
    {
        In,
        Keep,
        Out,
    }
    private BGMFade m_fade = BGMFade.In;


    void Start()
    {
        m_BGM = GetComponent<AudioSource>();
        m_BGM.volume = 0.0f;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7)) { ChangeBGMState(BGMState.Stop); }
        if (Input.GetKeyDown(KeyCode.F8)) { ChangeBGMState(BGMState.Before); }
        if (Input.GetKeyDown(KeyCode.F9)) { ChangeBGMState(BGMState.Battle); }
        if (Input.GetKeyDown(KeyCode.F10)) { ChangeBGMState(BGMState.Win); }

        m_maxVol = GameManager.Instance.BGMVolume;

        // フェード処理
        if (m_fade == BGMFade.In)
        {
            if (FadeIn())
            {
                Debug.Log("???");
                m_fade = BGMFade.Keep;
            }
        }
        else if (m_fade == BGMFade.Out)
        {
            if (FadeOut())
            {
                if (m_nextState != BGMState.None)
                {
                    m_nowState = m_nextState;
                    m_nextState = BGMState.None;
                    m_BGM.Stop();

                    ChangeBGMFade(BGMFade.In);
                }
                else
                {
                    m_fade = BGMFade.Keep;
                }
            }
        }
        else if (m_fade == BGMFade.Keep)
        {
            if (m_BGM.volume != m_maxVol) { m_BGM.volume = m_maxVol; }
        }


        // BGMの切り替え処理
        if (m_nowState == BGMState.Before)
        {
            m_BGM.PlayOneShot(m_beforeBGM);
            m_nowState = BGMState.None;
        }
        else if (m_nowState == BGMState.Battle)
        {
            m_BGM.PlayOneShot(m_battleBGM);
            m_nowState = BGMState.None;
        }
        else if (m_nowState == BGMState.Win)
        {
            m_BGM.PlayOneShot(m_winBGM);
            m_nowState = BGMState.None;
        }
    }


    public void ChangeBGMState(BGMState _state, float _fadeTime = 1.0f)
    {
        if (m_nowState == _state || m_nextState == _state) { return; }

        m_nextState = _state;
        m_fadeTime = _fadeTime;
        ChangeBGMFade(BGMFade.Out);
    }


    public void ChangeBGMFade(BGMFade _fade)
    {
        m_time = 0.0f;
        m_fade = _fade;
    }


    private bool FadeIn()
    {
        m_time += Time.deltaTime;
        float ratio = m_maxVol * (m_time / m_fadeTime);
        if (m_BGM.volume <= m_maxVol) { m_BGM.volume = ratio; }
        else { m_BGM.volume = m_maxVol; return true; }

        return false;
    }


    private bool FadeOut()
    {
        m_time += Time.deltaTime;
        float ratio = m_maxVol * (m_time / m_fadeTime);

        if (m_BGM.volume > 0.0f) { m_BGM.volume = m_maxVol - ratio; }
        else { m_BGM.volume = 0.0f; return true; }

        return false;
    }
}
