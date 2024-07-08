using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BottonOnMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image button;
    private TextMeshProUGUI text;

    private Color beforeColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    private Color afterColor = Color.white;


    private void OnEnable()
    {
        button = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        button.color = beforeColor;
        text.color = beforeColor;
    }


    public void OnPointerEnter(PointerEventData _eventData)
    {
        // マウスカーソルがボタンの上に来たとき、ボタンの色を明るくする
        button.color = afterColor;
        text.color = afterColor;
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        // マウスカーソルがボタンから離れたとき、ボタンの色を元に戻す
        button.color = beforeColor;
        text.color = beforeColor;
    }
}
