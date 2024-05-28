using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMovement : MonoBehaviour
{
    EffekseerEffectAsset effect; // エフェクトを取得する。

    // Start is called before the first frame update
    void Start()
    {
        effect = Resources.Load<EffekseerEffectAsset>("Simple_Ribbon_Sword");
    }

    public void PlayerSwordEffect()
    {
        // transformの位置でエフェクトを再生する
        Vector3 EffectPosition = transform.position;
        EffectPosition.y += 1f;
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, EffectPosition);

        // transformの回転を設定する。
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(-30, -90, 0);
        handle.SetRotation(EffectRotate);
    }
}
