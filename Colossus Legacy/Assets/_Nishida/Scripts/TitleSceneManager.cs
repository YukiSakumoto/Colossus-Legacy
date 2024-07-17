using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject m_startButtonObj;
    [SerializeField] private GameObject m_optionButtonObj;
    [SerializeField] private GameObject m_exitButtonObj;
    [SerializeField] private GameObject m_startManuObj;
    [SerializeField] private GameObject m_tutorialManuObj;
    [SerializeField] private GameObject m_optionManuObj;
    [SerializeField] private GameObject m_optionManuExitButtonObj;
    [SerializeField] private GameObject m_titleFadeObj;

    [SerializeField] private Canvas m_startManuCanvas;
    [SerializeField] private Canvas m_tutorialManuCanvas;
    [SerializeField] private Canvas m_optionManuCanvas;

    StartButtonManager m_startButtonManagerClass;
    OptionButtonManager m_optionButtonManagerClass;
    ExitButtonManager m_exitButtonManagerClass;
    StartManuManager m_startManuManagerClass;
    TutorialManuManager m_tutorialManuManagerClass;
    OptionManuManager m_optionManuManagerClass;
    OptionManuExitButtonManager m_optionManuExitButtonManagerClass;
    TitleFade m_titleFadeClass;

    //private float m_soundVolume = 0.5f;

    private bool m_startManuFlg = false; // スタートメニューを開いているかの管理
    private bool m_tutorialManuFlg = false; // チュートリアルメニューを開いているかの管理
    private bool m_optionManuFlg = false; // オプションメニューを開いているかの管理
    private bool m_hiddenCheckFlg = false; // フェードで画面が見えない状態かの管理
    //private bool m_optionManuCheckFlg = false; // オプションメニューを開いたことがあるかの管理


    // 追加 : 佐久本
    // Credit表記
    [SerializeField] private GameObject m_creditButtonObj;
    [SerializeField] private GameObject m_creditManuObj;
    [SerializeField] private GameObject m_creditExitButtonObj;
    [SerializeField] private Canvas m_creditCanvas;
    OptionButtonManager m_creditButtonManagerClass;
    OptionManuExitButtonManager m_creditManuExitButtonManagerClass;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_startButtonObj)
        {
            Debug.Log("StartButton Null");
        }
        else
        {
            m_startButtonManagerClass = m_startButtonObj.GetComponent<StartButtonManager>();
        }

        if (!m_optionButtonObj)
        {
            Debug.Log("OptionButton Null");
        }
        else
        {
            m_optionButtonManagerClass = m_optionButtonObj.GetComponent<OptionButtonManager>();
        }

        if (!m_exitButtonObj)
        {
            Debug.Log("ExitButton Null");
        }
        else
        {
            m_exitButtonManagerClass = m_exitButtonObj.GetComponent<ExitButtonManager>();
        }

        if (!m_creditButtonObj)
        {
            Debug.Log("CreditButton Null");
        }
        else
        {
            m_creditButtonManagerClass = m_creditButtonObj.GetComponent<OptionButtonManager>();
        }

        if (!m_startManuObj)
        {
            Debug.Log("StartManu Null");
        }
        else
        {
            m_startManuManagerClass = m_startManuObj.GetComponent<StartManuManager>();
        }

        if (!m_tutorialManuObj)
        {
            Debug.Log("TutorialManu Null");
        }
        else
        {
            m_tutorialManuManagerClass = m_tutorialManuObj.GetComponent<TutorialManuManager>();
        }

        if (!m_optionManuObj)
        {
            Debug.Log("OptionManu Null");
        }
        else
        {
            //m_optionManuCanvas = m_optionManuObj.GetComponent<Canvas>();
            m_optionManuManagerClass = m_optionManuObj.GetComponent<OptionManuManager>();
        }

        if (!m_creditManuObj)
        {
            Debug.Log("creditManu Null");
        }
        else
        {
            //m_optionManuCanvas = m_optionManuObj.GetComponent<Canvas>();
            //m_optionManuManagerClass = m_optionManuObj.GetComponent<OptionManuManager>();
        }

        if (!m_optionManuExitButtonObj)
        {
            Debug.Log("ManuExitTexture is Null");
        }
        else
        {
            m_optionManuExitButtonManagerClass = m_optionManuExitButtonObj.GetComponent<OptionManuExitButtonManager>();
        }

        if(!m_titleFadeObj)
        {
            Debug.LogError("TitleSceneManager: TitleFade is Null");
        }
        else
        {
            m_titleFadeClass = m_titleFadeObj.GetComponent<TitleFade>();
        }

        if (!m_creditExitButtonObj)
        {
            Debug.Log("CreditExitTexture is Null");
        }
        else
        {
            m_creditManuExitButtonManagerClass = m_creditExitButtonObj.GetComponent<OptionManuExitButtonManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_hiddenCheckFlg && m_titleFadeClass.Getm_hiddenFlg)
        {
            m_hiddenCheckFlg = true;
            m_startManuCanvas.gameObject.SetActive(false);
            m_tutorialManuCanvas.gameObject.SetActive(false);
            m_optionManuCanvas.gameObject.SetActive(false);
            m_creditCanvas.gameObject.SetActive(false);
        }

        if (m_startButtonManagerClass.Getm_startFlg) // Startが押されたらTraining Sceneをロードする
        {
            //SceneManager.LoadScene("Training Scene");
            Debug.Log("Startボタンが押されたよ");
            m_startManuCanvas.gameObject.SetActive(true);
            m_startButtonManagerClass.Setm_startFlg();
            m_startManuFlg = true;
        }

        if (m_optionButtonManagerClass.Getm_optionFlg) // Optionボタンが押されたらオプションメニューを開く
        {
            Debug.Log("Optionボタンが押されたぬ");
            m_optionButtonManagerClass.Setm_optionFlg();
            m_optionManuCanvas.gameObject.SetActive(true);
            m_optionManuFlg = true;
            //m_optionManuCheckFlg = true;
        }

        if (m_creditButtonManagerClass.Getm_optionFlg) // Creditボタンが押されたらクレジット表記を開く
        {
            Debug.Log("Creditボタンが押されわ");
            m_creditButtonManagerClass.Setm_optionFlg();
            m_creditCanvas.gameObject.SetActive(true);
        }

        if (m_exitButtonManagerClass.Getm_exitFlg) // Exitボタンが押されたらゲームを終了する
        {
            // ゲーム終了
            Application.Quit();

            // エディタで終了するとき用
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Debug.Log("Exitボタンが押されたかな");
            m_exitButtonManagerClass.Setm_exitFlg();
        }

        if (m_startManuFlg)
        {
            if (m_startManuManagerClass.Getm_difficultyClickFlg)
            {
                m_startManuManagerClass.Setm_difficultyClickFlg();
                m_startManuCanvas.gameObject.SetActive(false);
                m_tutorialManuCanvas.gameObject.SetActive(true);
                m_startManuFlg = false;
                m_tutorialManuFlg = true;
            }
            else if (m_startManuManagerClass.Getm_exitFlg)
            {
                m_startManuManagerClass.Setm_exitFlg();
                m_startManuCanvas.gameObject.SetActive(false);
                m_startManuFlg = false;
                m_tutorialManuFlg = false;
            }
        }

        if (m_tutorialManuFlg)
        {
            if(m_tutorialManuManagerClass.Getm_tutorialClickFlg)
            {
                m_tutorialManuManagerClass.Setm_tutorialClickFlg();
                m_tutorialManuCanvas.gameObject.SetActive(false);
                m_tutorialManuCanvas.gameObject.SetActive(false);
                m_tutorialManuFlg = false;
                if(m_tutorialManuManagerClass.Getm_tutorialChoiceFlg)
                {
                    SceneManager.LoadScene("Training Scene");
                }
                else
                {
                    SceneManager.LoadScene("GameScene");
                    if (GameManagerGameScene.Instance)
                    {
                        GameManagerGameScene.Instance.GameSceneStart();
                    }
                }
            }
            else if(m_tutorialManuManagerClass.Getm_exitFlg)
            {
                m_tutorialManuManagerClass.Setm_exitFlg();
                m_tutorialManuCanvas.gameObject.SetActive(false);
                m_tutorialManuFlg = false;
            }
        }

        if(m_optionManuExitButtonManagerClass.Getm_exitFlg)
        {
            m_optionManuExitButtonManagerClass.Setm_exitFlg();
            m_optionManuCanvas.gameObject.SetActive(false);
            m_optionManuFlg = false;
        }

        //add: 追加
        if (m_creditManuExitButtonManagerClass.Getm_exitFlg)
        {
            m_creditManuExitButtonManagerClass.Setm_exitFlg();
            m_creditCanvas.gameObject.SetActive(false);
        }
    }

    public bool Getm_optionManuFlg
    {
        get { return m_optionManuFlg; }
    }
}
