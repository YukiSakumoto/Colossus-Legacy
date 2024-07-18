using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManuManager : MonoBehaviour
{
    [SerializeField] Button m_easyButton;
    [SerializeField] Button m_hardButton;
    [SerializeField] Button m_superhardButton;
    [SerializeField] Button m_exitButton;
    [SerializeField] GameObject m_superhardObj;

    bool m_cheatSuperHardFlg = false;
    bool m_difficultyClickFlg = false;
    bool m_exitFlg = false;

    void Start()
    {
        if(m_easyButton != null)
        {
            m_easyButton.onClick.AddListener(EasyButtonClick);
        }
        else
        {
            Debug.LogError("StartManuManager: easyButton is Null");
        }

        if (m_hardButton != null)
        {
            m_hardButton.onClick.AddListener(HardButtonClick);
        }
        else
        {
            Debug.LogError("StartManuManager: hardButton is Null");
        }

        if (GameManager.Instance.clearFlg && m_superhardObj)
        {
            if (m_superhardButton != null)
            {
                m_superhardButton.onClick.AddListener(SuperHardButtonClick);
            }
            else
            {
                Debug.LogError("StartManuManager: superhardButton is Null");
            }
        }
        else
        {
            m_superhardObj.SetActive(false);
        }

        if (m_exitButton != null)
        {
            m_exitButton.onClick.AddListener(ExitButtonClick);
        }
        else
        {
            Debug.LogError("StartManuManager: exitButton is Null");
        }
    }

    private void Update()
    {
        if (!m_cheatSuperHardFlg)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_superhardObj.SetActive(true);
                m_cheatSuperHardFlg = true;
            }
        }
    }

    void EasyButtonClick()
    {
        GameManager.Instance.SetDifficulty(GameManager.Difficulty.Easy);
        m_difficultyClickFlg = true;
    }

    void HardButtonClick()
    {
        GameManager.Instance.SetDifficulty(GameManager.Difficulty.Hard);
        m_difficultyClickFlg = true;
    }

    void SuperHardButtonClick()
    {
        GameManager.Instance.SetDifficulty(GameManager.Difficulty.SuperHard);
        m_difficultyClickFlg = true;
    }

    void ExitButtonClick()
    {
        m_exitFlg = true;
    }

    public bool Getm_difficultyClickFlg
    {
        get { return m_difficultyClickFlg; }
    }

    public bool Getm_exitFlg
    {
        get { return m_exitFlg; }
    }

    public void Setm_difficultyClickFlg()
    {
        m_difficultyClickFlg = false;
    }

    public void Setm_exitFlg()
    {
        m_exitFlg = false;
    }
}