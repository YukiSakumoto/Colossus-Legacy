using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMovement : MonoBehaviour
{
    EffekseerEffectAsset SwordEffect; // 剣を振った時のエフェクト
    [SerializeField] GameObject m_swordObj;
    [SerializeField] CharacterManager m_manager;
    //Sword m_swordClass;

    EffekseerHandle effectHandle;

    private const float m_hitStopSetTime = 0.2f;

    private float m_hitStopTime = 0f;

    private bool m_hitStopFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        SwordEffect = Resources.Load<EffekseerEffectAsset>("Simple_Ribbon_Sworder");
        if(!m_swordObj)
        {
            Debug.Log("EffectMovement:sword is Null");
        }

        if (!m_manager)
        {
            Debug.Log("EffectMovement:manager is Null");
        }
    }

    public void PlayerSwordEffect()
    {
        // transformの位置でエフェクトを再生する
        Vector3 EffectPosition = transform.position;
        EffectPosition.y += 1f;
        effectHandle = EffekseerSystem.PlayEffect(SwordEffect, EffectPosition);

        // transformの回転を設定する。
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(-30, -90, 0);
        effectHandle.SetRotation(EffectRotate);
    }

    public void PlayerSword2Effect()
    {
        // transformの位置でエフェクトを再生する
        Vector3 EffectPosition = transform.position;
        EffectPosition.y += 1f;
        effectHandle = EffekseerSystem.PlayEffect(SwordEffect, EffectPosition);

        // transformの回転を設定する。
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(40, -90, 0);
        effectHandle.SetRotation(EffectRotate);
    }

    private void Update()
    {
        if (m_manager.Getm_hitFlg)
        {
            m_hitStopTime = m_hitStopSetTime;
            effectHandle.paused = true;
            m_hitStopFlg = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_hitStopFlg)
        {
            m_hitStopTime -= Time.deltaTime;
            if (m_hitStopTime < 0)
            {
                effectHandle.paused = false;
                m_hitStopFlg = false;
            }
        }
    }
}
