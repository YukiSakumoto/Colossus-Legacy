using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleButtonSE : MonoBehaviour
{
    [SerializeField] string m_buttonTag = "Button";
    [SerializeField] AudioSource m_point;
    [SerializeField] AudioSource m_click;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_point)
        {
            Debug.LogError("TitleButton: pointSound is Null");
        }

        if (!m_click)
        {
            Debug.LogError("TitleButton: clickSound is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �^�O�����{�^���̏�Ƀ}�E�X�J�[�\���������Ƃ��ɉ���炷
        if (eventData.pointerEnter.CompareTag(m_buttonTag))
        {
            m_point.Play();
        }

        // �^�O�����{�^�����N���b�N�����Ƃ��ɉ���炷
        if (eventData.pointerClick.CompareTag(m_buttonTag))
        {
            m_click.Play();
        }
    }
}
