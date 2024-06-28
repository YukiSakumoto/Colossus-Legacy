using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerGameScene : MonoBehaviour
{
    [SerializeField] CharacterManager m_charaInfo;
    [SerializeField] Fade m_fade;
    private bool fadeOutFlg;

    void Start()
    {
        if(!m_charaInfo)
        {
            Debug.Log("GameManagerGameScene:manager is Null");
        }

        if (!m_fade)
        {
            Debug.Log("GameManagerGameScene:fade is Null");
        }
        else
        {
            fadeOutFlg = false;
            m_fade.StartCoroutine(m_fade.FadeIn());
        }
    }

    void Update()
    {
        if(m_charaInfo.Getm_deathFlg && !fadeOutFlg)
        {
            m_fade.StartCoroutine(m_fade.FadeOut());
            fadeOutFlg = true;
        }
    }
}
