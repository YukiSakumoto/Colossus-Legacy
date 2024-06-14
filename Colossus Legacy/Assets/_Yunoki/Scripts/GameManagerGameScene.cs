using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGameScene : MonoBehaviour
{
    private CharacterMovement charaInfo;
    private Fade fade;
    private bool fadeOutFlg;
    // Start is called before the first frame update
    void Start()
    {
        fadeOutFlg = false;
        charaInfo= GameObject.Find("HumanMale_Character").GetComponent<CharacterMovement>();
        fade=GameObject.Find("Canvas").GetComponent<Fade>();
        fade.StartCoroutine(fade.FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        if(charaInfo.Getm_deathFlg && !fadeOutFlg)
        {
            fade.StartCoroutine(fade.FadeOut());
            fadeOutFlg= true;
        }
    }
}
