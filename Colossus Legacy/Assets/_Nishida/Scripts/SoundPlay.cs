using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource m_swordSwing;

    public void SoundSwordSwing()
    {
        m_swordSwing.Play();
    }
}
