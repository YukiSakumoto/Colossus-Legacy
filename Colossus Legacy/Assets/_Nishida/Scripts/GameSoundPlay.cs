using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource m_BGM;

    private float m_soundVolume;
    // Start is called before the first frame update
    void Start()
    {
        m_soundVolume = GameManager.Instance.soundVolume;

        if (!m_BGM)
        {
            Debug.Log("BGM is Null");
        }
        else
        {
            m_BGM.volume *= m_soundVolume;
            PlayBGM();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGM()
    {
        m_BGM.Play();
    }
}
