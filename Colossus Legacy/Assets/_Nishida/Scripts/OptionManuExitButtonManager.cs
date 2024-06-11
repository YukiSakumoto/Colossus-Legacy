using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManuExitButtonManager : MonoBehaviour
{
    private bool m_exitFlg = false;
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnImageClick);
        }
    }

    void OnImageClick()
    {
        m_exitFlg = true;
    }
    public void Setm_exitFlg()
    {
        m_exitFlg = false;
    }

    public bool Getm_exitFlg
    {
        get { return m_exitFlg; }
    }
}
