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
        // タグを持つボタンの上にマウスカーソルが来たときに音を鳴らす
        if (eventData.pointerEnter.CompareTag(m_buttonTag))
        {
            m_point.Play();
        }

        // タグを持つボタンをクリックしたときに音を鳴らす
        if (eventData.pointerClick.CompareTag(m_buttonTag))
        {
            m_click.Play();
        }
    }
}
