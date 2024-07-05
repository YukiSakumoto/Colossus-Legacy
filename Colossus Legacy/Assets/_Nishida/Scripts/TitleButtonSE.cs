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
    [SerializeField] Slider m_sliderValue;
    TitleSceneManager m_titleSceneManagerClass;

    private float m_soundVolume = 0.5f;
    private float m_SEPointVolume = 1.0f;
    private float m_SEClickVolume = 1.0f;

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

        //m_sliderValue = GameObject.Find("Slider").GetComponent<Slider>();
        if (!m_sliderValue)
        {
            Debug.LogError("TitleButton: slider is Null");
        }

        m_titleSceneManagerClass = GameObject.Find("EventSystem").GetComponent<TitleSceneManager>();

        m_SEPointVolume = m_point.volume;
        m_SEClickVolume = m_click.volume;
        m_point.volume = m_SEPointVolume * m_soundVolume;
        m_click.volume = m_SEClickVolume * m_soundVolume;

        // �^�O�t���̃{�^�����擾
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            if (button.CompareTag(m_buttonTag))
            {
                // �C�x���g�g���K�[��ǉ�
                EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = button.gameObject.AddComponent<EventTrigger>();
                }

                // �|�C���^�[�G���^�[�C�x���g��ǉ�
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { PointSound(); });
                trigger.triggers.Add(entry);

                // �|�C���^�[�G���^�[�C�x���g��ǉ�
                EventTrigger.Entry click = new EventTrigger.Entry();
                click.eventID = EventTriggerType.PointerClick;
                click.callback.AddListener((eventData) => { ClickSound(); });
                trigger.triggers.Add(click);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_titleSceneManagerClass.Getm_optionManuFlg)
        {
            VolumeControl();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �^�O�����{�^���̏�Ƀ}�E�X�J�[�\���������Ƃ��ɉ���炷
        if (eventData.pointerEnter.CompareTag(m_buttonTag))
        {
            Debug.Log("TitleButtonSE: �{�^���̏�Ƀ}�E�X�J�[�\��������");
            m_point.Play();
        }

        // �^�O�����{�^�����N���b�N�����Ƃ��ɉ���炷
        if (eventData.pointerClick.CompareTag(m_buttonTag))
        {
            m_click.Play();
            Debug.Log("TitleButtonSE: �{�^�����N���b�N����");
        }
    }

    void PointSound()
    {
        Debug.Log("TitleButtonSE: �{�^���̏�Ƀ}�E�X�J�[�\��������");
        m_point.Play();
    }

    void ClickSound()
    {
        m_click.Play();
        Debug.Log("TitleButtonSE: �{�^�����N���b�N����");
    }

    void VolumeControl()
    {
        m_soundVolume = m_sliderValue.value;
        m_point.volume = m_SEPointVolume * m_soundVolume;
        m_click.volume = m_SEClickVolume * m_soundVolume;
    }
}
