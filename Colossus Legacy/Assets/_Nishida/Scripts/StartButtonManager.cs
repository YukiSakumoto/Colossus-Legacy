using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonManager : MonoBehaviour
{
    private bool m_startFlg = false;

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
        m_startFlg = true;
    }

    public void Setm_startFlg()
    {
        m_startFlg = false;
    }

    public bool Getm_startFlg
    {
        get{ return m_startFlg; }
    }
}
