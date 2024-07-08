using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundPlay : MonoBehaviour
{
    [SerializeField] GameObject m_soundPlayObj;
    SoundPlay m_soundPlayClass;

    void Start()
    {
        if(!m_soundPlayObj)
        {
            Debug.Log("SoundPlay is Null");
        }
        else
        {
            m_soundPlayClass = m_soundPlayObj.GetComponent<SoundPlay>();
        }
    }

    public void SoundSwordSwing()
    {
        m_soundPlayClass.SoundSwordSwing();
    }

    public void SoundBowCharge()
    {
        m_soundPlayClass.SoundBowCharge();
    }

    public void SoundBowShot()
    {
        m_soundPlayClass.SoundBowShot();
    }

    public void SoundBombThrow()
    {
        m_soundPlayClass.SoundBombThrow();
    }

    public void SoundRoll()
    {
        m_soundPlayClass.SoundRoll();
    }

    public void SoundDeath()
    {
        m_soundPlayClass.SoundDeath();
    }

    public void SoundDamageSmall()
    {
        m_soundPlayClass.SoundDamageSmall();
    }

    public void SoundDamageMiddle()
    {
        m_soundPlayClass.SoundDamageMiddle();
    }

    public void SoundDamageHeavy()
    {
        m_soundPlayClass.SoundDamageHeavy();
    }

    public void SoundDamageKnockBackStart()
    {
        m_soundPlayClass.SoundDamageKnockBackStart();
    }

    public void SoundDamageKnockBackEnd()
    {
        m_soundPlayClass.SoundDamageKnockBackEnd();
    }

    public void SoundDeflected()
    {
        m_soundPlayClass.SoundDeflected();
    }

    public void SoundSubExplosion()
    {
        m_soundPlayClass.SoundSubExplosion();
    }

    public void SoundFootStep()
    {
        m_soundPlayClass.SoundFootStep();
    }
}
