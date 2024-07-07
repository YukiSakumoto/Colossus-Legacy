using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderStart : MonoBehaviour
{
    [SerializeField] private Slider m_BGMSlider;
    [SerializeField] private Slider m_SESlider;


    void OnEnable()
    {
        if (m_BGMSlider) { m_BGMSlider.value = GameManager.Instance.BGMVolume; }
        if (m_SESlider) { m_SESlider.value = GameManager.Instance.soundVolume; }
    }
}
