using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource m_swordSwing;
    [SerializeField] private AudioSource m_bowCharge;
    [SerializeField] private AudioSource m_bowShot;
    [SerializeField] private AudioSource m_roll;
    [SerializeField] private AudioSource m_death;
    [SerializeField] private AudioSource m_damageSmall;
    [SerializeField] private AudioSource m_damageMiddle;
    [SerializeField] private AudioSource m_damageHeavy;
    [SerializeField] private AudioSource m_damageKnockBackStart;
    [SerializeField] private AudioSource m_damageKnockBackEnd;

    public void SoundSwordSwing()
    {
        m_swordSwing.Play();
    }

    public void SoundBowCharge()
    {
        m_bowCharge.Play();
    }

    public void SoundBowShot()
    {
        m_bowShot.Play();
    }

    public void SoundRoll()
    {
        m_roll.Play();
    }

    public void SoundDeath()
    {
        m_death.Play();
    }

    public void SoundDamageSmall()
    {
        m_damageSmall.Play();
    }

    public void SoundDamageMiddle()
    {
        m_damageMiddle.Play();
    }

    public void SoundDamageHeavy()
    {
        m_damageHeavy.Play();
    }
    
    public void SoundDamageKnockBackStart()
    {
        m_damageKnockBackStart.Play();
    }

    public void SoundDamageKnockBackEnd()
    {
        m_damageKnockBackEnd.Play();
    }
}
