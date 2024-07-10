using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRange : MonoBehaviour
{
    [SerializeField] CapsuleCollider capsuleCollider;
    private string m_targetTag = "Player";

    const float m_displaySwitchTime = 1f;

    float m_displayTime = 1f;

    bool m_displaySwitch = false;
    bool m_keySwitch = false;
    [SerializeField] private string m_downTag = "KeyDown";
    [SerializeField] private string m_upTag = "KeyUp";

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if(child.CompareTag(m_downTag))
            {
                child.gameObject.SetActive(true);
            }
            else if(child.CompareTag(m_upTag))
            {
                child.gameObject.SetActive(false); 
            }
        }
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(false);
        //}
    }

    private void Update()
    {
        m_displayTime -= Time.deltaTime;
        if(m_displayTime <= 0f)
        {
            if (m_displaySwitch)
            {
                m_displayTime = m_displaySwitchTime;
                m_keySwitch = !m_keySwitch;
                SetChildrenActive();
            }
        }
        else if(!m_displaySwitch)
        {
            SetChildrenHidden();
        }
        else if(m_displaySwitch)
        {
            SetChildrenActive();
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(m_targetTag))
        {
            m_displaySwitch = true;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag(m_targetTag))
        {
            m_displaySwitch = false;
        }
    }

    private void SetChildrenActive()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(m_downTag))
            {
                child.gameObject.SetActive(m_keySwitch);
            }
            else if (child.CompareTag(m_upTag))
            {
                child.gameObject.SetActive(!m_keySwitch);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void SetChildrenHidden()
    {
        foreach (Transform child in transform)
        {
             child.gameObject.SetActive(false);
        }
    }
}