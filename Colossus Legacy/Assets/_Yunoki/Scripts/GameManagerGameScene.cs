using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerGameScene : MonoBehaviour
{
    public static GameManagerGameScene Instance { get; private set; }

    CharacterManager m_charaInfo;
    Fade m_fade;
    private bool fadeOutFlg;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneReset();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if(m_charaInfo.Getm_deathFlg && !fadeOutFlg)
        {
            m_fade.StartCoroutine(m_fade.FadeOut());
            fadeOutFlg = true;
        }
    }

    private void SceneReset()
    {
        fadeOutFlg = false;

        GameObject characterObj = GameObject.Find("HumanMale_Character");
        GameObject fadeObj = GameObject.Find("Canvas");

        m_charaInfo = characterObj.GetComponent<CharacterManager>();
        m_fade = fadeObj.GetComponent<Fade>();

        m_fade.StartCoroutine(m_fade.FadeIn());
    }


    public void GameSecneFin()
    {
        m_fade.StartCoroutine(m_fade.FadeOut(false));
        this.enabled = false;
        //if (gameObject != null)
        //{
        //    Destroy(gameObject);
        //}
    }


    public void GameSceneStart()
    {
        this.enabled = true;
    }
}
