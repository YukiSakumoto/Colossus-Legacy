using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManuManager : MonoBehaviour
{
    [SerializeField] Button m_yesButton;
    [SerializeField] Button m_noButton;
    [SerializeField] Button m_exitButton;

    bool m_tutorialClickFlg = false;
    bool m_tutorialChoiceFlg = false;
    bool m_exitFlg = false;

    void Start()
    {
        if (m_yesButton != null)
        {
            m_yesButton.onClick.AddListener(YesButtonClick);
        }
        else
        {
            Debug.LogError("TutorialManuManager: yesButton is Null");
        }

        if (m_noButton != null)
        {
            m_noButton.onClick.AddListener(NoButtonClick);
        }
        else
        {
            Debug.LogError("TutorialManuManager: noButton is Null");
        }

        if (m_exitButton != null)
        {
            m_exitButton.onClick.AddListener(ExitButtonClick);
        }
        else
        {
            Debug.LogError("TutorialManuManager: exitButton is Null");
        }
    }

    void YesButtonClick()
    {
        m_tutorialChoiceFlg = true;
        m_tutorialClickFlg = true;
    }

    void NoButtonClick()
    {
        m_tutorialChoiceFlg = false;
        m_tutorialClickFlg = true;
    }

    void ExitButtonClick()
    {
        m_exitFlg = true;
    }

    public bool Getm_tutorialChoiceFlg
    {
        get { return m_tutorialChoiceFlg; }
    }

    public bool Getm_tutorialClickFlg
    {
        get { return m_tutorialClickFlg; }
    }

    public bool Getm_exitFlg
    {
        get { return m_exitFlg; }
    }

    public void Setm_tutorialClickFlg()
    {
        m_tutorialClickFlg = false;
    }

    public void Setm_exitFlg()
    {
        m_exitFlg = false;
    }
}