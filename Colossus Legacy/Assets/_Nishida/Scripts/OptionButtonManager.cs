using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButtonManager : MonoBehaviour
{
    private bool m_optionFlg = false;
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
        m_optionFlg = true;
    }

    public void Setm_optionFlg()
    {
        m_optionFlg = false;
    }

    public bool Getm_optionFlg
    {
        get { return m_optionFlg; }
    }
}
