using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideDisplay : MonoBehaviour
{
    const float m_displaySwitchTime = 1f;

    float m_displayTime = 1f;

    bool m_displaySwitch = false;
    [SerializeField] private string m_downTag = "KeyDown";
    [SerializeField] private string m_upTag = "KeyUp";

    GameObject[] objectsDownTag;
    GameObject[] objectsUpTag;

    void Start()
    {
        objectsDownTag = GameObject.FindGameObjectsWithTag(m_downTag);
        objectsUpTag = GameObject.FindGameObjectsWithTag(m_upTag);
        // 各オブジェクトの表示を切り替え
        foreach (GameObject obj in objectsDownTag)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsUpTag)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        m_displayTime -= Time.deltaTime;
        if(m_displayTime < 0f)
        {
            m_displayTime = m_displaySwitchTime;
            ToggleVisibility();
            if (!m_displaySwitch)
            {
                m_displaySwitch = true;
            }
            else
            {
                m_displaySwitch = false;
            }
        }
    }

    // 表示を切り替えるメソッド
    private void ToggleVisibility()
    {
        // 指定されたタグを持つ全てのオブジェクトを取得
        if (m_displaySwitch)
        {
            // 各オブジェクトの表示を切り替え
            foreach (GameObject obj in objectsDownTag)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsUpTag)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in objectsUpTag)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsDownTag)
            {
                obj.SetActive(false);
            }
        }
    }
}