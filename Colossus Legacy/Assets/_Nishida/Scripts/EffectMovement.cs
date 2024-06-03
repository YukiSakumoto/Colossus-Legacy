using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMovement : MonoBehaviour
{
    EffekseerEffectAsset SwordEffect; // 剣を振った時のエフェクト
    EffekseerEffectAsset aEffect;

    // Start is called before the first frame update
    void Start()
    {
        SwordEffect = Resources.Load<EffekseerEffectAsset>("Simple_Ribbon_Sworder");
    }

    public void PlayerSwordEffect()
    {
        // transformの位置でエフェクトを再生する
        Vector3 EffectPosition = transform.position;
        Vector3 rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        EffectPosition.y += 1f;
        EffekseerHandle handle = EffekseerSystem.PlayEffect(SwordEffect, EffectPosition);

        // transformの回転を設定する。
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(-30, -90, 0);
        handle.SetRotation(EffectRotate);
    }

    public void PlayerSword2Effect()
    {
        // transformの位置でエフェクトを再生する
        Vector3 EffectPosition = transform.position;
        Vector3 rotationDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        EffectPosition.y += 1f;
        EffekseerHandle handle = EffekseerSystem.PlayEffect(SwordEffect, EffectPosition);

        // transformの回転を設定する。
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(40, -90, 0);
        handle.SetRotation(EffectRotate);
    }
}
