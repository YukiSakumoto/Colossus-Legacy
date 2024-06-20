using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GolemSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip m_swingSE;
    public AudioClip m_golemMoveSE;
    public AudioClip m_stone;
    public AudioClip m_laserCharge;
    public AudioClip m_laserShot;
    public AudioClip m_laserKeep;


    public void PlaySwing()
    {
        audioSource.PlayOneShot(m_swingSE);
    }


    public void PlayGolemMove()
    {
        audioSource.PlayOneShot(m_golemMoveSE);
    }


    public void PlayStone()
    {
        audioSource.PlayOneShot(m_stone);
    }


    public void PlayLaserCharge()
    {
        audioSource.PlayOneShot(m_laserCharge);
    }


    public void PlayLaserShot()
    {
        audioSource.PlayOneShot(m_laserShot);
    }


    public void PlayLaserKeep()
    {
        audioSource.PlayOneShot(m_laserKeep);
    }


    public void StopAllSound()
    {
        audioSource.Stop();
    }
}
