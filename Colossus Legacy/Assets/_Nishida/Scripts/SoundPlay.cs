using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource m_swordSwing;
    [SerializeField] private AudioSource m_bowCharge;
    [SerializeField] private AudioSource m_bowShot;
    [SerializeField] private AudioSource m_bombThrow;
    [SerializeField] private AudioSource m_roll;
    [SerializeField] private AudioSource m_death;
    [SerializeField] private AudioSource m_damageSmall;
    [SerializeField] private AudioSource m_damageMiddle;
    [SerializeField] private AudioSource m_damageHeavy;
    [SerializeField] private AudioSource m_damageKnockBackStart;
    [SerializeField] private AudioSource m_damageKnockBackEnd;
    [SerializeField] private AudioSource m_damageCrush;
    [SerializeField] private AudioSource m_deflected;
    [SerializeField] private AudioSource m_subExplosion;
    [SerializeField] private AudioSource m_footStep;
    [SerializeField] private AudioSource m_treasureOpenBegin;
    [SerializeField] private AudioSource m_treasureOpening;

    private float m_soundVolume = 0;

    private void Start()
    {
        m_soundVolume = GameManager.Instance.soundVolume;

        m_swordSwing.volume *= m_soundVolume;
        m_bowCharge.volume *= m_soundVolume;
        m_bowShot.volume *= m_soundVolume;
        m_bombThrow.volume *= m_soundVolume;
        m_roll.volume *= m_soundVolume;
        m_death.volume *= m_soundVolume;
        m_damageSmall.volume *= m_soundVolume;
        m_damageMiddle.volume *= m_soundVolume;
        m_damageHeavy.volume *= m_soundVolume * 2f;
        m_damageKnockBackStart.volume *= m_soundVolume;
        m_damageKnockBackEnd.volume *= m_soundVolume;
        m_damageCrush.volume *= m_soundVolume;
        m_deflected.volume *= m_soundVolume;
        m_subExplosion.volume *= m_soundVolume;
        m_footStep.volume *= m_soundVolume;
        m_treasureOpenBegin.volume *= m_soundVolume;
        m_treasureOpening.volume *= m_soundVolume;
    }   

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

    public void SoundBombThrow()
    {
        m_bombThrow.Play();
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

    public void SoundDamageCrush()
    {
        m_damageCrush.Play();
    }

    public void SoundDeflected()
    {
        m_deflected.Play();
    }

    public void SoundSubExplosion()
    {
        m_subExplosion.Play();
    }

    public void SoundFootStep()
    {
        m_footStep.Play();
    }

    public void SoundTreasureOpenBegin()
    {
        m_treasureOpenBegin.Play();
    }

    public void SoundTreasureOpening()
    {
        m_treasureOpening.Play();
    }
}