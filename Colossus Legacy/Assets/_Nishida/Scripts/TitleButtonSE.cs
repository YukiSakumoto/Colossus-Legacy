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

        // タグ付きのボタンを取得
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            if (button.CompareTag(m_buttonTag))
            {
                // イベントトリガーを追加
                EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = button.gameObject.AddComponent<EventTrigger>();
                }

                // ポインターエンターイベントを追加
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { PointSound(); });
                trigger.triggers.Add(entry);

                // ポインターエンターイベントを追加
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
        // タグを持つボタンの上にマウスカーソルが来たときに音を鳴らす
        if (eventData.pointerEnter.CompareTag(m_buttonTag))
        {
            Debug.Log("TitleButtonSE: ボタンの上にマウスカーソルが来た");
            m_point.Play();
        }

        // タグを持つボタンをクリックしたときに音を鳴らす
        if (eventData.pointerClick.CompareTag(m_buttonTag))
        {
            m_click.Play();
            Debug.Log("TitleButtonSE: ボタンをクリックした");
        }
    }

    void PointSound()
    {
        Debug.Log("TitleButtonSE: ボタンの上にマウスカーソルが来た");
        m_point.Play();
    }

    void ClickSound()
    {
        m_click.Play();
        Debug.Log("TitleButtonSE: ボタンをクリックした");
    }

    void VolumeControl()
    {
        m_soundVolume = m_sliderValue.value;
        m_point.volume = m_SEPointVolume * m_soundVolume;
        m_click.volume = m_SEClickVolume * m_soundVolume;
    }
}
