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
    [SerializeField] private GameObject m_optionManuObj;
    [SerializeField] private GameObject m_optionManuExitButtonObj;

    [SerializeField] private Canvas m_optionManuCanvas;

    StartButtonManager m_startButtonManagerClass;
    OptionButtonManager m_optionButtonManagerClass;
    ExitButtonManager m_exitButtonManagerClass;
    OptionManuManager m_optionManuManagerClass;
    OptionManuExitButtonManager m_optionManuExitButtonManagerClass;

    private float m_soundVolume = 0.5f;

    private bool m_optionManuFlg = false; // �I�v�V�������j���[���J���Ă��邩�̊Ǘ�
    private bool m_optionManuCheckFlg = false; // �I�v�V�������j���[���J�������Ƃ����邩�̊Ǘ�

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

        if (!m_optionManuObj)
        {
            Debug.Log("OptionManu Null");
        }
        else
        {
            //m_optionManuCanvas = m_optionManuObj.GetComponent<Canvas>();
            m_optionManuManagerClass = m_optionManuObj.GetComponent<OptionManuManager>();
            m_optionManuCanvas.gameObject.SetActive(false);
        }

        if (!m_optionManuExitButtonObj)
        {
            Debug.Log("ManuExitTexture is Null");
        }
        else
        {
            m_optionManuExitButtonManagerClass = m_optionManuExitButtonObj.GetComponent<OptionManuExitButtonManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_startButtonManagerClass.Getm_startFlg) // Start�������ꂽ��Training Scene�����[�h����
        {
            if (m_optionManuCheckFlg) // �I�v�V�������j���[���J�������Ƃ��������特�ʂ�ݒ肷��
            {
                m_soundVolume = m_optionManuManagerClass.Getm_soundValue;
            }
            GameManager.Instance.soundVolume = m_soundVolume;
            SceneManager.LoadScene("Training Scene");
            Debug.Log("Start�{�^���������ꂽ��");
            m_startButtonManagerClass.Setm_startFlg();
        }

        if (m_optionButtonManagerClass.Getm_optionFlg) // Option�{�^���������ꂽ��I�v�V�������j���[���J��
        {
            Debug.Log("Option�{�^���������ꂽ��");
            m_optionButtonManagerClass.Setm_optionFlg();
            m_optionManuCanvas.gameObject.SetActive(true);
            m_optionManuFlg = true;
            m_optionManuCheckFlg = true;
        }

        if (m_exitButtonManagerClass.Getm_exitFlg) // Exit�{�^���������ꂽ��Q�[�����I������
        {
            // �Q�[���I��
            Application.Quit();

            // �G�f�B�^�ŏI������Ƃ��p
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Debug.Log("Exit�{�^���������ꂽ����");
            m_exitButtonManagerClass.Setm_exitFlg();
        }

        if(m_optionManuExitButtonManagerClass.Getm_exitFlg)
        {
            m_optionManuExitButtonManagerClass.Setm_exitFlg();
            m_optionManuCanvas.gameObject.SetActive(false);
            m_optionManuFlg = false;
        }
    }

    public bool Getm_optionManuFlg
    {
        get { return m_optionManuFlg; }
    }
}
