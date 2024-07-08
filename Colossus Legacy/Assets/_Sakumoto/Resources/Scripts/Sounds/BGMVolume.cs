using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolume : MonoBehaviour
{
    private AudioSource m_audio;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        m_audio.volume = GameManager.Instance.BGMVolume;
    }
}
