using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGameScene : MonoBehaviour
{
    [SerializeField] CharacterMovement charaInfo;
    [SerializeField] Fade fade;
    private bool fadeOutFlg;

    void Start()
    {
        fadeOutFlg = false;
        fade.StartCoroutine(fade.FadeIn());
    }

    void Update()
    {
        if(charaInfo.Getm_deathFlg && !fadeOutFlg)
        {
            fade.StartCoroutine(fade.FadeOut());
            fadeOutFlg = true;
        }
    }
}
