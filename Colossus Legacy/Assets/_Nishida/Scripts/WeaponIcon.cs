using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIcon : MonoBehaviour
{
    CharacterManager m_manager;
    GameObject m_swordObj;
    GameObject m_bowObj;

    void Start()
    {
        GameObject characterObj = GameObject.Find("HumanMale_Character");
        m_swordObj = GameObject.Find("SwordIcon");
        m_bowObj = GameObject.Find("BowIcon");

        if(!characterObj)
        {
            Debug.Log("WeaponIcon: characterObj is Null");
        }
        else
        {
            m_manager = characterObj.GetComponent<CharacterManager>();
        }

        if (!m_swordObj)
        {
            Debug.Log("WeaponIcon: swordObj is Null");
        }
        else
        {
            m_swordObj.SetActive(true);
        }

        if (!m_bowObj)
        {
            Debug.Log("WeaponIcon: bowObj is Null");
        }
        else
        {
            m_swordObj.SetActive(false);
        }
    }

    void Update()
    {
        if(!m_manager.Getm_weaponFlg)
        {
            m_swordObj.SetActive(true);
            m_bowObj.SetActive(false);
        }
        else
        {
            m_swordObj.SetActive(false);
            m_bowObj.SetActive(true);
        }
    }
}
