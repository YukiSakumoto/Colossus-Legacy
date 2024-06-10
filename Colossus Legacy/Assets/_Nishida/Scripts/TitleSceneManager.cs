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

    StartButtonManager m_startButtonManagerClass;
    OptionButtonManager m_optionButtonManagerClass;
    ExitButtonManager m_exitButtonManagerClass;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_startButtonManagerClass.Getm_startFlg)
        {
            // ゲームシーンをロードする
            //SceneManager.LoadScene("GameScene");
            Debug.Log("Startボタンが押されたよ");
            m_startButtonManagerClass.Setm_startFlg();
        }

        if (m_optionButtonManagerClass.Getm_optionFlg)
        {
            Debug.Log("Optionボタンが押されたぬ");
            m_optionButtonManagerClass.Setm_optionFlg();
        }

        if (m_exitButtonManagerClass.Getm_exitFlg)
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
    }
}
